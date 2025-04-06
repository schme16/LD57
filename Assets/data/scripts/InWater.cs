using System;
using UnityEngine;

public class InWater : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log(1);
			PlayerScript.player.inWater = true;
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log(2);
			PlayerScript.player.inWater = true;
		}
	}



	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			PlayerScript.player.inWater = false;
		}
	}
}
