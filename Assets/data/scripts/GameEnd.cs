using System;
using System.Xml.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEnd : MonoBehaviour {


	private async void OnTriggerEnter(Collider other) {
		PlayerScript.player.lockMovement = true;
		//PlayerScript.player.firstPersonController.enabled = false;
		PlayerScript.player.controller.enabled = false;
		//PlayerScript.player.cameraRoot
		_.RotateLocal(PlayerScript.player.theDoorFull, PlayerScript.player.theDoorFullOpen);
		_.Translate(PlayerScript.player.transform, PlayerScript.player.theDoor.position);
		await UniTask.Delay(350);
		PlayerScript.player.fadeOutWhite.gameObject.SetActive(true);
		await UniTask.Delay((int)(PlayerScript.player.fadeOutWhite.fadeTime * 1000));
		PlayerScript.player.Die();
	}

}
