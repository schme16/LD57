//using Cinemachine;
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


	[Header("Settings")]
	public float interactionDistance = 1;

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

}
