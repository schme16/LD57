using System;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;

public class TriggerSoundOnEnter : MonoBehaviour {

	public StudioEventEmitter audio;
	public bool canReplay;
	public int delayBetweenReplays;
	public bool canPlay;


	private async void OnTriggerEnter(Collider other) {
		if (canPlay) {
			canPlay = false;
			await _.PlayAudio(audio);

			if (canReplay) {
				await UniTask.Delay(delayBetweenReplays);
				canPlay = true;
			}
		}
	}
}
