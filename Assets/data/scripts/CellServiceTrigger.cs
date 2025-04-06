using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using System.Threading;
using FMOD.Studio;

public class CellServiceTrigger : MonoBehaviour {

	public bool canTrigger;
	public List<EventReference> audio;
	
	public EventReference cellPhoneBuzz;


	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && canTrigger) {
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


	private async UniTask PlayMessage(EventReference audio, CancellationToken cancellationToken = default) {

		try {

			// Create the event instance
			EventInstance eventInstance = RuntimeManager.CreateInstance(audio);

			// Start the event
			eventInstance.start();

			// Create a completion source that will be triggered when the event finishes
			var completionSource = new UniTaskCompletionSource();

			// Poll the playback state until it's no longer playing
			// Also continuously update the 3D position
			await UniTask.Create(async () => {

				PLAYBACK_STATE playbackState = PLAYBACK_STATE.PLAYING;

				while (playbackState != PLAYBACK_STATE.STOPPED && !cancellationToken.IsCancellationRequested) {

					// Update the 3D attributes to the current player position
					eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(PlayerScript.player.gameObject));
					eventInstance.getPlaybackState(out playbackState);

					if (playbackState == PLAYBACK_STATE.STOPPED) {

						completionSource.TrySetResult();
						break;

					}

					await UniTask.Delay(20, cancellationToken: cancellationToken); // Update position more frequently (50 times per second)
				}

				// Clean up the event instance
				eventInstance.release();

			});

			await completionSource.Task;
		}
		catch (Exception ex) {
			Debug.LogError($"Error playing FMOD audio: {ex.Message}");
		}
	}
}
