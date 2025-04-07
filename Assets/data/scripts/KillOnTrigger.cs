using System;
using UnityEngine;

public class KillOnTrigger : MonoBehaviour {


	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			PlayerScript.player.Die();
		}
	}
}
