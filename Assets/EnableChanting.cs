using FMODUnity;
using UnityEngine;

public class EnableChanting : MonoBehaviour {

	public StudioEventEmitter chanting;
	
	void Start() {
		_.PlayAudio(chanting, chanting.gameObject);
	}

	// Update is called once per frame
	void Update() {

	}
}
