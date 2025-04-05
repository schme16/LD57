using System;
using UnityEngine;

public class PickupAndHold : MonoBehaviour {

	public bool canPickup;
	public string validText = "Pickup ([keycode])";
	public string invalidText;
	public Vector3 HoldPosition = new Vector3(0, 0, 0);
	[NonSerialized]
	public Rigidbody rb;
	public Collider collider;
	private InteractableScript interact;
	public LayerMask rbMaskWhenHeld;
	[NonSerialized]
	public LayerMask rbInitialMask;
	//[NonSerialized]
	public bool isInsidePlayer;
	//[NonSerialized]
	public bool debounce;


	void Start() {
		rb = GetComponent<Rigidbody>();

		if (rb is not null) {
			rbInitialMask = rb.excludeLayers;
		}

		interact = GetComponent<InteractableScript>();

		interact.validInteractionText = validText;

		interact.invalidInteractionText = invalidText;

		interact.IsValid = () => {
			return canPickup && PlayerScript.player.interactScript == interact && PlayerScript.player.CanHold();
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

		Debug.Log(1);
		if (other.CompareTag("Player") && PlayerScript.player.currentlyHeldObject == this) {
			Debug.Log(2);
			isInsidePlayer = true;
		}
	}

	private void OnTriggerExit(Collider other) {

		//Ignore any events during debounce
		//used to prevent double fires on changing isKinematic
		if (debounce) {
			return;
		}

		Debug.Log(3);
		if (other.CompareTag("Player")) {
			Debug.Log(4);
			isInsidePlayer = false;

			if (collider is not null && PlayerScript.player.currentlyHeldObject != this) {
				Debug.Log(7);
				collider.excludeLayers = rbInitialMask;
			}
		}
	}
}
