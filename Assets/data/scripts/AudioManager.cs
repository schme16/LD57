using System;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance { get; private set; }

	private void Awake() {
		if (instance != null) {
			Debug.Log("AudioManager loaded twice");
		}
		else {
			instance = this;
		}
	}

	public void PlaceOneShot(EventReference sound, Vector3 worldPosition) {
		RuntimeManager.PlayOneShot(sound, worldPosition);
	}
}
