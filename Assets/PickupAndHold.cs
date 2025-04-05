using UnityEngine;

public class PickupAndHold : MonoBehaviour {

	public bool canPickup;
	public string validText = "Pickup ([keycode])";
	public string invalidText;
	private InteractableScript interact;


	void Start() {
		interact = GetComponent<InteractableScript>();

		interact.validInteractionText = validText;

		interact.invalidInteractionText = invalidText;

		interact.IsValid = () => {
			return canPickup && PlayerScript.player.interactScript == interact && PlayerScript.player.CanHold();
		};

		interact.OnInteract.AddListener(() => {
			//PlayerScript.player.
		});
	}
}
