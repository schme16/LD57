using System;
using FMODUnity;
using UnityEngine;

public class ShovelScript : MonoBehaviour {

	public bool hitting;
	public StudioEventEmitter hitAudio;
	public Collider hitBox;
	public int damage = 1;
	public float distance;
	public LayerMask mask;
	public Vector3 scale = new Vector3(0.6f,0.6f,0.6f);

	private RaycastHit[] hits;
	private RaycastHit hit;
	private bool didHit;


	public void ScanForHits() {
		Debug.Log("Scanning for hits");
		hits = Physics.BoxCastAll(hitBox.bounds.center, scale * 0.5f, hitBox.transform.up, hitBox.transform.rotation, distance, mask);
		if (hits.Length > 0) {
			Debug.Log($"Hit! {hits.Length}");
			foreach (var raycastHit in hits) {
				var health = raycastHit.transform.GetComponent<Health>() ??  raycastHit.transform.GetComponentInParent<Health>() ?? raycastHit.transform.GetComponentInChildren<Health>();
				if (health) {
					Debug.Log(1111);
					health.TakeDamage(damage);
				}
			}
			
			_.PlayAudio(hitAudio);
			didHit = true;

		}
		else {
			didHit = false;
		}
	}

	public void SetHitting(string state) {
		hitting = state == "true";
	}

}
