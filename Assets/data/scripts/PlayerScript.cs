using Cysharp.Threading.Tasks;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour {

	[Header("Debug")]
	public bool debug;



	[Header("Player settings")]
	public string state;
	private string lastState;
	public bool preventInteraction;
	public bool canMove;
	public bool lockCamera;
	private bool debounce;

	[Header("Components")]
	public FirstPersonController firstPersonController;
	public StarterAssetsInputs inputs;
	public PlayerInput playerInput;
	public AudioSource bgMusic;
	public AudioSource sfxSource;
	public Animator animator;

	public InteractableScript interactScript;
	private InteractableScript lastInteractScript;
	private bool lastInteractScriptEnabled;
	private Transform currentHitObject;
	public CursorLockMode cursorMode = CursorLockMode.Locked;
	private CursorLockMode lastCursorMode;

	public Vector2 interactionTextVisiblePosition = new Vector3(0, 20, 0);
	public Vector2 interactionTextHiddenPosition = new Vector3(0, -25, 0);

	private GameController gc;



	private async void Start() {

		//Set the game ontroller
		gc = FindFirstObjectByType<GameController>();

		//Enable the bg music
		bgMusic.enabled = true;

		//Play it
		bgMusic.Play();

		//Set the interaction ui message to the default hidden position
		gc.uiInteractionMessageRectTransform.anchoredPosition = new Vector3(0, -25, 0);

		//Mark the player's start state
		SetState("playing");
	}

	private void Update() {

		_.SetLockMode(cursorMode);


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


		//Check for interactions
		CheckForInteractable();

	}

	//Set the players current state
	private void SetState(string newState) {
		state = newState;
	}

	private void CheckForInteractable() {

		if (!preventInteraction) {

			//Do a ray cast
			if (Physics.Raycast(gc.cam.transform.position, gc.cam.transform.forward, out var hitInfo, gc.interactionDistance)) {

				//Is compatible
				if (hitInfo.transform.CompareTag("Interactable")) {

					//Was it different to the thing we were already looking at? 
					if (hitInfo.transform != currentHitObject) {

						//Update the current object test item
						currentHitObject = hitInfo.transform;

						//Get the interaction script
						var interact = currentHitObject.GetComponent<InteractableScript>();

						//Was there still no interaction script found?
						if (interact is null) {

							//Search the parent
							interact = currentHitObject.GetComponentInParent<InteractableScript>();
						}

						//Did we find one, and was it enabled?
						if (interact is not null && interact.enabled) {

							//Mark the old item as not in view
							if (interactScript is not null) {
								interactScript.inView = false;
							}

							//Update the interactable item shorthand
							interactScript = interact;

							//Update the currently hit object with the interaction scripts transform, in case it's a parent
							currentHitObject = interactScript.transform;


							//Is the interaction in a valid state
							if (interactScript.IsValid()) {
								gc.uiInteractionMessage.SetText(CompileMessageText(interactScript.validInteractionText));
							}
							else {
								gc.uiInteractionMessage.SetText(CompileMessageText(interactScript.invalidInteractionText));
							}

						}
						else {
							if (interactScript is not null) {
								interactScript.inView = false;
							}
							interactScript = null;
							currentHitObject = null;
						}
					}
				}

				//Not compatible
				else {
					if (interactScript is not null) {
						interactScript.inView = false;
					}
					interactScript = null;
					currentHitObject = null;
				}

			}

			//Nothing found
			else {
				if (interactScript is not null) {
					interactScript.inView = false;
				}
				interactScript = null;
				currentHitObject = null;
			}


			//Check for a change flag on interactables
			if (interactScript is not null && interactScript.enabled) {

				//Is the interaction in a valid state
				if (interactScript.IsValid()) {
					gc.uiInteractionMessage.SetText(CompileMessageText(interactScript.validInteractionText));
				}
				else {
					gc.uiInteractionMessage.SetText(CompileMessageText(interactScript.invalidInteractionText));
				}

			}
		}

		//Not allowed to interact, so clear currently interacted stuff too
		else {
			if (interactScript is not null) {
				interactScript.inView = false;
			}

			interactScript = null;
			currentHitObject = null;
		}

		
		//Did the interact script value change?
		if (lastInteractScriptEnabled != interactScript) {
			lastInteractScriptEnabled = interactScript;
			UpdateInteractionText();
		}

	}

	private void UpdateInteractionText() {

		//Is the interactable script set, and enabled?
		if (interactScript is not null && interactScript.enabled && !preventInteraction) {

			//Mark it as "in view"
			interactScript.inView = true;

			if (gc.uiInteractionMessageRectTransform.anchoredPosition != interactionTextVisiblePosition) {

				//Slide the text up onto the screen
				_.Translate(gc.uiInteractionMessageRectTransform, interactionTextVisiblePosition, 5f, EasingFunction.Ease.EaseOutQuad);
			}
		}

		//Either interact script not set or is disabled, and we aren't possessing an object
		else {

			//If the script is set, but is disabled
			if (interactScript is not null) {

				//Mark it as not in view
				interactScript.inView = false;
			}

			if (gc.uiInteractionMessageRectTransform.anchoredPosition != interactionTextHiddenPosition) {

				//Slide the text down off the screen
				_.Translate(gc.uiInteractionMessageRectTransform, interactionTextHiddenPosition, 5f, EasingFunction.Ease.EaseOutQuad);

			}
		}

	}

	private string CompileMessageText(string text) {
		var interactionKey = gc.keyInteract;

		if (interactScript is not null && interactScript.interactionKey != KeyCode.None) {
			interactionKey = interactScript.interactionKey;
		}
		return text.Replace("[keycode]", interactionKey.ToString());
	}

	private void OnDestroy() {

	}

	private void OnApplicationFocus(bool hasFocus) {
		_.SetLockMode(cursorMode);
	}

	private void OnApplicationPause(bool pauseStatus) {
		_.SetLockMode(cursorMode);
	}
}
