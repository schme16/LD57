using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMODUnity;
using StarterAssets;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour {

	[Header("Debug")]
	public bool debug;



	[Header("Settings")]
	public string state;
	private string lastState;
	public bool preventInteraction;
	public bool lockMovement;
	private bool lastLockMovement;
	public bool lockCamera;
	private bool lastLockCamera;
	public bool lockMouse = true;
	public float interactionDistance = 1.8f;
	public float pickupForce = 150f;
	public LayerMask interactionLayers;
	public float footstepAudioVolume = 0.5f;
	public Vector2 interactionTextVisiblePosition = new Vector3(0, 20, 0);
	public Vector2 interactionTextHiddenPosition = new Vector3(0, -25, 0);
	public bool inWater;

	[Header("Components")]
	public CharacterController controller;
	public FirstPersonController firstPersonController;
	public StarterAssetsInputs inputs;
	public PlayerInput playerInput;
	public AudioSource bgMusic;
	public AudioSource sfxSource;
	public Animator animator;
	public Transform HeldItemHolder;
	public List<EventReference> footstepsStone;
	public List<EventReference> footstepsWater;
	public EventReference playerLandStone;
	public EventReference playerLandWater;


	[Header("Tracking")]
	public InteractableScript interactScript;
	public InteractableScript lastInteractScript;
	public bool lastInteractScriptEnabled;
	public Transform currentHitObject;
	public PickupAndHold currentlyHeldObject;


	private bool debounce;
	private GameController gc;
	private float _moveSpeed;
	private float _sprintSpeed;
	private float lastGrounded;
	public static PlayerScript player;


	private void Start() {

		//Set the static access item
		player = this;

		//Set the game ontroller
		gc = FindFirstObjectByType<GameController>();

		//Set the initial movement and sprint speed values
		_moveSpeed = firstPersonController.MoveSpeed;
		_sprintSpeed = firstPersonController.SprintSpeed;

		//Set the initial walk and camera state
		UpdateLockWalk(true);
		UpdateLockCamera(true);

		//Set the inital value for the held item
		currentlyHeldObject = null;




		//Enable the bg music
		//TODO: make this a setting!
		//bgMusic.enabled = true;

		//Play it
		//bgMusic.Play();

		//Set the interaction ui message to the default hidden position
		gc.uiInteractionMessageRectTransform.anchoredPosition = new Vector3(0, -25, 0);

		//Mark the player's start state
		SetState("playing");

		firstPersonController.OnPlayerLand.AddListener(Landed);
		firstPersonController.OnPlayerWalk.AddListener(Walked);
	}

	private void Update() {

		_.SetLockMode(lockMouse ? CursorLockMode.Locked : CursorLockMode.Confined);


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

		//Checks if the player movement state has changed
		UpdateLockWalk();

		//Checks if the camera movement state has changed
		UpdateLockCamera();

		//Check for interactions
		CheckForInteractable();

		//Are we currently holding an object?
		if (currentlyHeldObject is not null) {
			Debug.Log(6);

			//Did we try to release it?
			if (Input.GetKeyDown(GameController._keyInteract)) {
				Debug.Log(7);

				//Drop it
				DropHeldObject();
			}

			//Didn't try and release it
			else {

				Debug.Log(8);
				//Move it into position 
				MoveHeldObject();
			}

		}

	}

	private void SetState(string newState) {
		state = newState;
	}

	private void CheckForInteractable() {

		Debug.Log(1);
		if (!preventInteraction && currentlyHeldObject is null) {
			Debug.Log(2);

			//Do a ray cast
			if (Physics.Raycast(gc.cam.transform.position + (gc.cam.transform.forward * 0.3f), gc.cam.transform.forward, out var hitInfo, interactionDistance, interactionLayers)) {
				Debug.Log(3);


				//Is compatible
				if (hitInfo.transform.CompareTag("Interactable")) {
					Debug.Log(4);

					//Was it different to the thing we were already looking at? 
					if (hitInfo.transform != currentHitObject) {
						Debug.Log(5);
						//Update the current object test item
						currentHitObject = hitInfo.transform;

						//Get the interaction script
						//Was there still no interaction script found?
						//Search the parent
						var interact = currentHitObject.GetComponent<InteractableScript>() ?? currentHitObject.GetComponentInParent<InteractableScript>();

						//Did we find one, and was it enabled?
						if (interact is not null && interact.enabled) {

							/*//Mark the old item as not in view
							if (interactScript is not null) {
								interactScript.inView = false;
							}*/

							//Update the interactable item shorthand
							interactScript = interact;

							//Update the currently hit object with the interaction scripts transform, in case it's a parent
							currentHitObject = interactScript.transform;

							//Is the interaction in a valid state
							//Set the UI text to the compiled version set in the script
							gc.uiInteractionMessage.SetText(interactScript.IsValid() ? CompileMessageText(interactScript.validInteractionText) : CompileMessageText(interactScript.invalidInteractionText));

						}
						else {
							/*if (interactScript is not null) {
								interactScript.inView = false;
							}*/
							interactScript = null;
							currentHitObject = null;
						}
					}
				}

				//Not compatible
				else {
					/*if (interactScript is not null) {
						interactScript.inView = false;
					}*/
					interactScript = null;
					currentHitObject = null;
				}

			}

			//Nothing found
			else {
				/*if (interactScript is not null) {
					interactScript.inView = false;
				}*/
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
			/*if (interactScript is not null) {
				interactScript.inView = false;
			}*/

			interactScript = null;
			currentHitObject = null;
		}


		//Did the interact script value change?
		if (lastInteractScriptEnabled != interactScript) {
			UnityEngine.Debug.Log(12);
			lastInteractScriptEnabled = interactScript;
			UpdateInteractionText();
		}

	}

	private void UpdateInteractionText() {

		//Is the interactable script set, and enabled?
		if (interactScript is not null && interactScript.enabled && !preventInteraction) {

			/*//Mark it as "in view"
			interactScript.inView = true;*/

			if (gc.uiInteractionMessageRectTransform.anchoredPosition != interactionTextVisiblePosition) {

				//Slide the text up onto the screen
				_.Translate(gc.uiInteractionMessageRectTransform, interactionTextVisiblePosition, 5f, EasingFunction.Ease.EaseOutQuad);
			}
		}

		//Either interact script not set or is disabled, and we aren't possessing an object
		else {

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

	private void UpdateLockWalk(bool force = false) {
		if (lastLockMovement != lockMovement || force) {
			lastLockMovement = lockMovement;
			if (lockMovement) {
				//Backup the movement speed
				_moveSpeed = firstPersonController.MoveSpeed;

				//Set it to 0
				firstPersonController.MoveSpeed = 0;

				//Backup the sprint speed
				_sprintSpeed = firstPersonController.SprintSpeed;

				//Set it to 0
				firstPersonController.SprintSpeed = 0;

			}
			else {

				//Restore the movement speed
				firstPersonController.MoveSpeed = _moveSpeed;

				//Restore the sprint speed
				firstPersonController.SprintSpeed = _sprintSpeed;
			}
		}
	}

	private void UpdateLockCamera(bool force = false) {
		if (lastLockCamera != lockCamera || force) {
			lastLockCamera = lockCamera;
			inputs.cursorInputForLook = !lockCamera;
		}
	}

	public bool CanHold() {
		return currentlyHeldObject is null || currentlyHeldObject.IsUnityNull();
	}

	public async void HoldObject(PickupAndHold obj) {

		//Set the held object
		currentlyHeldObject = obj;

		//Parent it to the holder
		currentlyHeldObject.transform.SetParent(HeldItemHolder);


		//If it has a rigidbody
		if (currentlyHeldObject.rb is not null) {

			//Disable gravity
			currentlyHeldObject.rb.useGravity = false;

			//Set the drag to 10
			currentlyHeldObject.rb.linearDamping = 10;

			//Freeze the rotation
			currentlyHeldObject.rb.constraints = RigidbodyConstraints.FreezeRotation;
		}

		if (currentlyHeldObject.collider is not null) {
			currentlyHeldObject.collider.excludeLayers = currentlyHeldObject.rbMaskWhenHeld;
		}

		debounce = true;
		await UniTask.DelayFrame(1);
		debounce = false;
	}

	public async void DropHeldObject() {

		if (!debounce) {
			//Has a rigidbody
			if (currentlyHeldObject.rb is not null) {

				//Disable gravity
				currentlyHeldObject.rb.useGravity = true;

				//Set the drag to 10
				currentlyHeldObject.rb.linearDamping = 1;

				//Freeze the rotation
				currentlyHeldObject.rb.constraints = RigidbodyConstraints.None;
			}

			if (currentlyHeldObject.collider is not null) {
				if (!currentlyHeldObject.isInsidePlayer) {
					currentlyHeldObject.collider.excludeLayers = currentlyHeldObject.rbInitialMask;
				}
			}

			currentlyHeldObject.transform.SetParent(null);

			currentlyHeldObject = null;
		}
	}

	public void MoveHeldObject() {

		//If the item is further than 0.1f away from the holder
		if (Vector3.Distance(currentlyHeldObject.transform.position, HeldItemHolder.transform.position) > 0.1f) {
			var moveDirection = (HeldItemHolder.position - currentlyHeldObject.transform.position);

			currentlyHeldObject.rb.AddForce(moveDirection * pickupForce);
		}
	}

	private void Landed() {
		if (lastGrounded < Time.time - 0.65f) {
			lastGrounded = Time.time;
			var sfxList = inWater ? playerLandWater : playerLandStone;

			AudioManager.instance.PlaceOneShot(sfxList, transform.TransformPoint(controller.center));
			//_.PlayClipAtPoint(sfxList, transform.TransformPoint(controller.center), footstepAudioVolume, Random.Range(0.75f, 1.15f));
		}
	}

	private void Walked() {


		var sfxList = inWater ? footstepsWater : footstepsStone;
		AudioManager.instance.PlaceOneShot(sfxList[Random.Range(0, sfxList.Count)], transform.TransformPoint(controller.center));

		//_.PlayClipAtPoint(sfxList[Random.Range(0, sfxList.Count)], transform.TransformPoint(controller.center), footstepAudioVolume, Random.Range(0.75f, 1.15f));
	}

	private void OnDestroy() {

	}



	private void OnApplicationFocus(bool hasFocus) {
		_.SetLockMode(lockMouse ? CursorLockMode.Locked : CursorLockMode.Confined);
	}

	private void OnApplicationPause(bool pauseStatus) {
		_.SetLockMode(lockMouse ? CursorLockMode.Locked : CursorLockMode.Confined);
	}
}
