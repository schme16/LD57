using System;
using UnityEngine;

public class ShovelScript : MonoBehaviour {

	public bool hitting;


	public void ScanForHits() {
		Debug.Log("Scanning for hits");
	}
	
	public void SetHitting(string state) {
		Debug.Log($"Setting hitting: {state == "true"}");
		hitting = state == "true";
	}

}
