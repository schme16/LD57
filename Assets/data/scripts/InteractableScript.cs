using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class InteractableScript : MonoBehaviour {

	public string text;
	public string validInteractionText;
	private string lastValidInteractionText;
	public string invalidInteractionText;
	private string lastInvalidInteractionText;
	public UnityEvent OnInteract;
	public Func<bool> IsValid = () => false;
	public KeyCode interactionKey;
	public bool valid;
	private bool lastValid;
	private bool lastInView;
	private bool inView;
	public bool showOutline;
	private bool lastShowOutline;
	public Color outlineColour = new Color(255, 127, 28, 0f);
	private Color lastOutlineColour;
	private float outlineAlpha;
	private float lastOutlineAlpha;

	private Outline outline;

	private void Start() {

		//Fetch the outline
		outline = GetComponent<Outline>();
		var colour = outlineColour;
		colour.a = 0;
		outlineColour = colour;
		outline.enabled = false;


		//Get the interaction key
		//TODO: make this respect changes on the fly?
		if (interactionKey == KeyCode.None) {
			interactionKey = GameController._keyInteract;
		}

		UpdateOutline();
	}

	private void Update() {

		UpdateOutline();

		inView = PlayerScript.player.interactScript == this;

		//Get the validator value
		valid = IsValid();

		if (valid) {
			text = validInteractionText;
		}
		else {

			text = invalidInteractionText;
		}

		//Are we in range?
		if (inView || inView != lastInView) {


			//Only update the text state on changes
			if (lastInView != inView || valid != lastValid || validInteractionText != lastValidInteractionText || invalidInteractionText != lastInvalidInteractionText) {

				//Update the last in range value
				lastInView = inView;

				//Update the last valid value
				lastValid = valid;

				//Update the last valid text value
				lastValidInteractionText = validInteractionText;

				//Update the last invalid text value
				lastInvalidInteractionText = invalidInteractionText;
			}
		}

		//If both in range, and valid
		if (inView && valid) {

			//Was the interact key pressed?
			if (Input.GetKeyDown(GameController._keyInteract)) {

				//Was an interaction function set?
				if (OnInteract is not null) {


					//Run the func
					OnInteract.Invoke();
				}
			}
		}


	}

	private void OnDisable() {
		inView = false;
		UpdateOutline();
	}

	private void UpdateOutline() {


		//Update the outline alpha value
		if (showOutline && inView) {
			if (showOutline && !outline.enabled) {
				outline.enabled = true;
				var colour = outlineColour;
				colour.a = 0;
				outlineColour = colour;
			}
			outlineAlpha += (Time.deltaTime * 10);
		}
		else if (outlineAlpha > 0) {
			outlineAlpha -= (Time.deltaTime * 10);
			if (outlineAlpha <= 0) {
				outline.enabled = false;
			}
		}

		/*//Was the show outline value changed?
		if (lastShowOutline != showOutline) {

			lastShowOutline = showOutline;
			if (showOutline) {
				outline.enabled = showOutline;

				var colour = outlineColour;
				colour.a = 0;
				outlineColour = colour;

			}
		}*/

		//Was the outline colour changed?
		if (outlineColour != lastOutlineColour) {
			lastOutlineColour = outlineColour;
			outline.OutlineColor = outlineColour;
		}


		//Clamp it to the alpha bounds
		outlineAlpha = Mathf.Clamp(outlineAlpha, 0, 1);

		//Was this alpha value different from last frame?
		if (lastOutlineAlpha != outlineAlpha) {

			//Sync the vals
			lastOutlineAlpha = outlineAlpha;

			//Update the alpha
			var colour = outlineColour;
			colour.a = outlineAlpha;

			outlineColour = colour;
		}


	}


}
