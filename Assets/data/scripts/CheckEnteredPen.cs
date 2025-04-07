using UnityEngine;

public class CheckEnteredPen : MonoBehaviour {

	public string penTag;
	public float detectionRadius;
	public LayerMask detectionMask;
	public bool showDebugVisuals;


	public GameObject Pen {
		get;
		set;
	}

	public GameObject UpdateCheck() {
		// Perform sphere check
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

		if (colliders.Length == 1 && colliders[0].CompareTag(penTag)) {
			Pen = colliders[0].gameObject;
		}
		else {
			Pen = null;
		}
		return Pen;
	}

	private void OnDrawGizmos() {
		if (!showDebugVisuals || this.enabled == false) return;

		Gizmos.color = Pen ? Color.green : Color.yellow;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);

	}
}
