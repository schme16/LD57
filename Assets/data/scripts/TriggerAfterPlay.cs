using UnityEngine;
using UnityEngine.Events;

public class TriggerAfterPlay : MonoBehaviour {

	public UnityEvent OnPlayed;
	public bool setPlayerValues;
	
	public bool lockMovement;
	public bool lockCamera;
	public bool lockMouse;
	public bool showCallAnsweredFromBoss;
	public bool showMobileModel;
	public bool hideMobileModel;
	public bool enableShovel;


	void Start() {
		//Just adding a default to fix any whinging invoke might have
		OnPlayed.AddListener(() => {
			if (setPlayerValues) {
				PlayerScript.player.lockMovement = lockMovement;
			    PlayerScript.player.lockCamera = lockCamera;
			    PlayerScript.player.lockMouse = lockMouse;
			}

			if (showCallAnsweredFromBoss) {
				PlayerScript.player.ShowCallAnsweredFromBoss();
			}

			if (hideMobileModel) {
				PlayerScript.player.HangupPhone();
			}

			if (enableShovel) {
				PlayerScript.player.shovelOut = true;
				Destroy(PlayerScript.player.shovelPickup);
			}
		});
	}

	// Update is called once per frame
	void Update() {

	}
}
