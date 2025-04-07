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

		await PlayMessage(cellPhoneBuzz);

		foreach (var message in audio) {

			await PlayMessage(message);

		}

	}


	private async UniTask PlayMessage(StudioEventEmitter audio, CancellationToken cancellationToken = default) {

		try {

			/*// Create the event instance
			EventInstance eventInstance = RuntimeManager.CreateInstance(audio);*/

			// Start the event
			audio.EventInstance.start();
			audio.Play();

			// Create a completion source that will be triggered when the event finishes
			var completionSource = new UniTaskCompletionSource();

			// Poll the playback state until it's no longer playing
			// Also continuously update the 3D position
			await UniTask.Create(async () => {

				audio.EventDescription.getLength(out var totalLengthInMilliseconds);
				audio.EventInstance.getTimelinePosition(out var currentPosition);

				while (currentPosition <= totalLengthInMilliseconds) {
					audio.EventInstance.getTimelinePosition(out currentPosition);

					Debug.Log($"{currentPosition} / {totalLengthInMilliseconds}");

					// Update the 3D attributes to the current player position
					audio.EventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(PlayerScript.player.gameObject));

					completionSource.TrySetResult();
					await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellationToken); // Update position more frequently (50 times per second)
				}

				// Clean up the event instance
				audio.EventInstance.release();

			});

			await completionSource.Task;
		}
		catch (Exception ex) {
			Debug.LogError($"Error playing FMOD audio: {ex.Message}");
		}
	}
}
