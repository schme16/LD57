using Cysharp.Threading.Tasks;
using FMODUnity;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
	public Vector3 mobileInView;
	public Vector3 mobileOutOfView;
	public bool shovelOut;
	private bool lastShovelOut;
	public ScreenFade fadeOut;


	[Header("Components")]
	public CharacterController controller;
	public FirstPersonController firstPersonController;
	public StarterAssetsInputs inputs;
	public PlayerInput playerInput;
	public AudioSource bgMusic;
	public AudioSource sfxSource;
	public Animator animator;
	public Transform HeldItemHolder;
	public StudioEventEmitter footstepsStone;
	public StudioEventEmitter footstepsWater;
	public StudioEventEmitter playerLandStone;
	public StudioEventEmitter playerLandWater;
	public Transform mobileModel;
	public Renderer mobileScreenModel;
	public StudioEventEmitter phoneCallEmitter;
	public Material bossCallingMaterial;
	public Material bossAnsweredMaterial;
	public ShovelScript shovel;
	public Animator shovelAnimator;

	[Header("Tracking")]
	public InteractableScript interactScript;
	public InteractableScript lastInteractScript;
	public bool lastInteractScriptEnabled;
	public Transform currentHitObject;
	public PickupAndHold currentlyHeldObject;
	private PickupAndHold lastHeldObject;


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

		SetPhoneScreenMaterials(bossCallingMaterial);

		mobileModel.localPosition = mobileOutOfView;

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


		//Does shovel state switching
		if (lastShovelOut != shovelOut) {
			lastShovelOut = shovelOut;

			if (shovelOut) {
				Debug.Log(111);
				shovelAnimator.SetTrigger("bring out");
			}
			else {

				Debug.Log(2222);
				shovelAnimator.SetTrigger("put away");
			}
		}

		//handles the shovel hit
		if (shovelOut && !shovel.hitting && Input.GetKeyDown(KeyCode.Mouse0)) {
			shovelAnimator.SetTrigger("hit");
		}


		//Are we currently holding an object?
		if (currentlyHeldObject is not null) {

			//Did we try to release it?
			if (Input.GetKeyDown(GameController._keyInteract)) {

				//Drop it
				DropHeldObject();
			}

			//Didn't try and release it
			else {

				//Move it into position 
				MoveHeldObject();
			}

		}

	}

	private void SetState(string newState) {
		state = newState;
	}

	private void CheckForInteractable() {

		if (!preventInteraction && currentlyHeldObject is null) {

			//Do a ray cast
			if (Physics.Raycast(gc.cam.transform.position + (gc.cam.transform.forward * 0.3f), gc.cam.transform.forward, out var hitInfo, interactionDistance, interactionLayers)) {


				//Is compatible
				if (hitInfo.transform.CompareTag("Interactable")) {

					//Was it different to the thing we were already looking at? 
					if (hitInfo.transform != currentHitObject) {

						//Update the current object test item
						currentHitObject = hitInfo.transform;

						//Get the interaction script
						//Was there still no interaction script found?
						//Search the parent
						var interact = currentHitObject.GetComponent<InteractableScript>() ?? currentHitObject.GetComponentInParent<InteractableScript>();

						//Did we find one, and was it enabled?
						if (interact is not null && interact.enabled) {


							//Update the interactable item shorthand
							interactScript = interact;

							//Update the currently hit object with the interaction scripts transform, in case it's a parent
							currentHitObject = interactScript.transform;



						}
						else {
							interactScript = null;
							currentHitObject = null;
						}
					}
				}

				//Not compatible
				else {
					interactScript = null;
					currentHitObject = null;
				}

			}

			//Nothing found
			else {
				interactScript = null;
				currentHitObject = null;
			}

		}

		//Not allowed to interact, so clear currently interacted stuff too
		else {

			interactScript = null;
			currentHitObject = null;
		}


		/*//Did the interact script value change?
		if (lastInteractScriptEnabled != interactScript || lastHeldObject != currentlyHeldObject) {

			//Sync the comparitor
			lastInteractScriptEnabled = interactScript;

			//Sync the comparitor
			lastHeldObject = currentlyHeldObject;
		}*/

		//Update the UI text
		UpdateInteractionText();

	}

	private void UpdateInteractionText() {

		//Is the interactable script set, and enabled?
		if ((interactScript is not null && interactScript.enabled && !preventInteraction) || currentlyHeldObject is not null) {


			var interact = interactScript ?? currentlyHeldObject.interact;
			if (interact is not null) {
				//Is the interaction in a valid state
				gc.uiInteractionMessage.SetText(CompileMessageText(interact.text));
			}

			if (gc.uiInteractionMessageRectTransform.anchoredPosition != interactionTextVisiblePosition) {

				//Slide the text up onto the screen
				_.Translate(gc.uiInteractionMessageRectTransform, interactionTextVisiblePosition, 7f, EasingFunction.Ease.EaseOutQuad);
			}
		}

		//Either interact script not set or is disabled, and we aren't possessing an object
		else {

			if (gc.uiInteractionMessageRectTransform.anchoredPosition != interactionTextHiddenPosition) {

				//Slide the text down off the screen
				_.Translate(gc.uiInteractionMessageRectTransform, interactionTextHiddenPosition, 7f, EasingFunction.Ease.EaseOutQuad);

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
				inputs.move = Vector2.zero;

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
			inputs.look = Vector2.zero;

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

			_.PlayAudio(sfxList);
		}
	}

	private void Walked() {


		var sfxList = inWater ? footstepsWater : footstepsStone;
		_.PlayAudio(sfxList);
	}

	public void AnswerPhone() {
		SetPhoneScreenMaterials(bossCallingMaterial);
		_.TranslateLocal(mobileModel, mobileInView, 1);
		lockMovement = true;
		lockCamera = false;
		lockMouse = true;
	}

	public void ShowCallAnsweredFromBoss() {
		SetPhoneScreenMaterials(bossAnsweredMaterial);
	}


	public void HangupPhone() {
		_.TranslateLocal(mobileModel, mobileOutOfView, 1);
	}

	public void SetPhoneScreenMaterials(Material mat) {
		mobileScreenModel.material = mat;

	}

	private void OnDestroy() {

	}



	private void OnApplicationFocus(bool hasFocus) {
		_.SetLockMode(lockMouse ? CursorLockMode.Locked : CursorLockMode.Confined);
	}

	private void OnApplicationPause(bool pauseStatus) {
		_.SetLockMode(lockMouse ? CursorLockMode.Locked : CursorLockMode.Confined);
	}

	public async void Die() {
		lockMovement = true;
		lockCamera = true;
		fadeOut.gameObject.SetActive(true);
		fadeOut.fadeOut = true;
		await UniTask.Delay((int)(fadeOut.fadeTime * 1000) + 100);
		SceneManager.LoadScene("data/scenes/menu");
	}
}
