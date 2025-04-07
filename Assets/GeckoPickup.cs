using FMODUnity;
using UnityEngine;

public class GeckoPickup : MonoBehaviour {

	public InteractableScript interact;
	public StudioEventEmitter[] audio;
	public bool canInteract = true;

	void Start() {
		interact.IsValid = () => canInteract;

		interact.OnInteract.AddListener(async () => {
			canInteract = false;
			await _.PlayAudio(audio[Random.Range(0, audio.Length)]);
			canInteract = true;
		});
	}

	void Update() {

	}
}
