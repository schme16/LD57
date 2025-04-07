using System;
using System.Xml.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEnd : MonoBehaviour {


	private async void OnTriggerEnter(Collider other) {
		PlayerScript.player.lockMovement = true;
		//PlayerScript.player.firstPersonController.enabled = false;
		PlayerScript.player.controller.enabled = false;
		
		await UniTask.Delay(500);
		await _.Translate(PlayerScript.player.transform, PlayerScript.player.transform.position + (PlayerScript.player.transform.forward * 2));
		await UniTask.Delay(1500);
		await _.RotateLocal(PlayerScript.player.theDoorFull, PlayerScript.player.theDoorFullOpen);
		await UniTask.Delay(1000);
		await _.Translate(PlayerScript.player.transform, PlayerScript.player.theDoor.position);
		PlayerScript.player.fadeOutWhite.gameObject.SetActive(true);
		await UniTask.Delay((int)(PlayerScript.player.fadeOutWhite.fadeTime * 1000));
		PlayerScript.player.Die();
	}

}
