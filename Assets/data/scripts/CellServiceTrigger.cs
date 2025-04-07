using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using System.Threading;
using FMOD.Studio;

public class CellServiceTrigger : MonoBehaviour {

	public bool canTrigger;
	//public List<EventReference> audio;
	public List<StudioEventEmitter> audio;

	public StudioEventEmitter cellPhoneBuzz;
	public int audioEndOffsetMS;


	private void OnTriggerEnter(Collider other) {
		if (enabled && other.CompareTag("Player") && canTrigger) {
			if (canTrigger) {
				PlayerScript.player.AnswerPhone();
				PlayAllMessages();
			}
		}
	}



	private async void PlayAllMessages() {
		canTrigger = false;

		await _.PlayAudio(cellPhoneBuzz);

		foreach (var message in audio) {

			await _.PlayAudio(message);

		}

		PlayerScript.player.HangupPhone();

	}

}
