using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check If Destination Has Been Reached", story: "[CheckIfDestinationReached] by [Agent] and set new [WanderTarget] inside [Pen]", category: "Action", id: "3b0bb7973972e18685b6bfe386d98eb6")]
public partial class CheckIfDestinationHasBeenReachedAction : Action {
	[SerializeReference] public BlackboardVariable<CheckIfDestinationReached> CheckIfDestinationReached;
	[SerializeReference] public BlackboardVariable<Vector3> WanderTarget;
	[SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
	[SerializeReference] public BlackboardVariable<GameObject> Pen;
	public float maxTimeToDestination = 15;
	private float timeTaken;
	private bool hasReachedDestination;



	protected override Status OnUpdate() {

		if (CheckReachedDestination() || timeTaken > maxTimeToDestination || (WanderTarget.Value.x == -1)) {
			timeTaken = 0;
			WanderTarget.Value = GetRandomPointInCollider(Pen.Value.GetComponent<Collider>());
			return Status.Success;
		}
		else {
			timeTaken += Time.deltaTime;
			return Status.Failure;
		}
	}





	private bool CheckReachedDestination() {
		return !(Agent.Value.pathPending || Agent.Value.remainingDistance > Agent.Value.stoppingDistance);
	}





	public static Vector3 GetRandomPointInCollider(Collider collider) {
		if (collider is BoxCollider boxCollider) {
			return GetRandomPointInBoxCollider(boxCollider);
		}
		else if (collider is SphereCollider sphereCollider) {
			return GetRandomPointInSphereCollider(sphereCollider);
		}
		else if (collider is CapsuleCollider capsuleCollider) {
			return GetRandomPointInCapsuleCollider(capsuleCollider);
		}
		else if (collider is MeshCollider meshCollider && meshCollider.convex) {
			return GetRandomPointInMeshCollider(meshCollider);
		}
		else {
			Debug.LogWarning("Unsupported collider type or non-convex MeshCollider.");
			return collider.bounds.center;
		}
	}

	private static Vector3 GetRandomPointInBoxCollider(BoxCollider boxCollider) {
		Vector3 extents = boxCollider.size * 0.5f;
		Vector3 randomPoint = new Vector3(
			Random.Range(-extents.x, extents.x),
			Random.Range(-extents.y, extents.y),
			Random.Range(-extents.z, extents.z)
		);
		return boxCollider.transform.TransformPoint(boxCollider.center + randomPoint);
	}

	private static Vector3 GetRandomPointInSphereCollider(SphereCollider sphereCollider) {
		Vector3 randomDirection = Random.onUnitSphere * Random.Range(0f, 1f);
		return sphereCollider.transform.TransformPoint(sphereCollider.center + randomDirection * sphereCollider.radius);
	}

	private static Vector3 GetRandomPointInCapsuleCollider(CapsuleCollider capsuleCollider) {
		float height = Mathf.Max(0, capsuleCollider.height * 0.5f - capsuleCollider.radius);
		Vector3 randomDirection = Random.insideUnitSphere * capsuleCollider.radius;
		float randomHeight = Random.Range(-height, height);
		Vector3 randomPoint = new Vector3(randomDirection.x, randomHeight, randomDirection.z);
		return capsuleCollider.transform.TransformPoint(capsuleCollider.center + randomPoint);
	}

	private static Vector3 GetRandomPointInMeshCollider(MeshCollider meshCollider) {
		Mesh mesh = meshCollider.sharedMesh;
		if (mesh == null) {
			Debug.LogWarning("MeshCollider has no mesh.");
			return meshCollider.bounds.center;
		}

		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		int triIndex = Random.Range(0, triangles.Length / 3) * 3;

		Vector3 a = vertices[triangles[triIndex]];
		Vector3 b = vertices[triangles[triIndex + 1]];
		Vector3 c = vertices[triangles[triIndex + 2]];

		Vector3 randomPoint = GetRandomPointInTriangle(a, b, c);
		return meshCollider.transform.TransformPoint(randomPoint);
	}

	private static Vector3 GetRandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c) {
		float r1 = Mathf.Sqrt(Random.value);
		float r2 = Random.value;
		return (1 - r1) * a + (r1 * (1 - r2)) * b + (r1 * r2) * c;
	}
}
