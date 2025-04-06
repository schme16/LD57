using System;
using UnityEngine;

public class PickupAndHold : MonoBehaviour {

	public bool canPickup;
	public string validTextPickup = "Pickup ([keycode])";
	public string validTextDrop = "Drop ([keycode])";
	public string invalidText;
	public Vector3 HoldPosition = new Vector3(0, 0, 0);
	[NonSerialized]
	public Rigidbody rb;
	public Collider collider;
	public InteractableScript interact;
	public LayerMask rbMaskWhenHeld;
	[NonSerialized]
	public LayerMask rbInitialMask;
	[NonSerialized]
	public bool isInsidePlayer;
	[NonSerialized]
	public bool debounce;


	void Start() {
		rb = GetComponent<Rigidbody>();

		if (rb is not null) {
			rbInitialMask = rb.excludeLayers;
		}

		interact = GetComponent<InteractableScript>();

		

		interact.invalidInteractionText = invalidText;

		interact.IsValid = () => {
			
			//Can be picked up
			if (canPickup && PlayerScript.player.interactScript == interact && PlayerScript.player.CanHold()) {
				Debug.Log(1);
				interact.validInteractionText = validTextPickup;
				return true;
			}
			else if (PlayerScript.player.currentlyHeldObject is not null && PlayerScript.player.currentlyHeldObject == this) {

				interact.validInteractionText = validTextDrop;
				Debug.Log($"2 - {interact.validInteractionText}");
				return true;
			}
			else {
				Debug.Log(3);
				return false;
			}

		};

		interact.OnInteract.AddListener(() => {
			PlayerScript.player.HoldObject(this);

		});
	}

	private void OnTriggerEnter(Collider other) {

		//Ignore any events during debounce
		//used to prevent double fires on changing isKinematic
		if (debounce) {
			return;
		}

		if (other.CompareTag("Player") && PlayerScript.player.currentlyHeldObject == this) {
			isInsidePlayer = true;
		}
	}

	private void OnTriggerExit(Collider other) {

		//Ignore any events during debounce
		//used to prevent double fires on changing isKinematic
		if (debounce) {
			return;
		}

		if (other.CompareTag("Player")) {
			isInsidePlayer = false;

			if (collider is not null && PlayerScript.player.currentlyHeldObject != this) {
				collider.excludeLayers = rbInitialMask;
			}
		}
	}
}
