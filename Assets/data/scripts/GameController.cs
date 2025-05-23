//using Cinemachine;
using FMOD.Studio;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {

	[Header("Camera")]
	public Camera cam;

	[Header("UI")]
	public Transform uiCanvas;
	public GameObject reticule;
	public RectTransform uiInteractionMessageRectTransform;
	public TextMeshProUGUI uiInteractionMessage;

	public static float currentSFXVolume;
	public static float currentMusicVolume;
	public static Bus SFXBus;
	public static Bus MusicBus;




	public string state;
	private string lastState;

	[Header("Keys")]
	public KeyCode keyInteract;
	public static KeyCode _keyInteract;

	void Awake() {

		_keyInteract = keyInteract;

	}

	async void Start() {

		//Set main cam
		cam = Camera.main;

		SetState("playing");
	}

	// Update is called once per frame
	private void Update() {

		//Has the state changed?
		if (state != lastState) {

			//Update the last state variable
			lastState = state;

			//Run this once, on change
			switch (state) {
				case "playing":
					break;
			}
		}


		//Run this every frame
		switch (state) {
			case "playing":
				break;
		}

	}

	// Update is called once per frame
	private void SetState(string newState) {
		state = newState;
	}



	public static void SetSFXVolume(float volume) {

		currentSFXVolume = volume;

		PlayerPrefs.SetFloat("SFXVolume", volume);
		PlayerPrefs.Save();

		//Set the volume on the master bus
		SFXBus.setVolume(currentSFXVolume);
	}

	public static void SetMusicVolume(float volume) {

		currentMusicVolume = volume;

		//Set the volume on the master bus
		MusicBus.setVolume(currentMusicVolume);

		PlayerPrefs.SetFloat("MusicVolume", volume);
		PlayerPrefs.Save();
	}
}
