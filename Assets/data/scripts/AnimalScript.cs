using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;

public class AnimalScript : MonoBehaviour {

	private NavMeshAgent agent;
	public bool wantsPath;
	public float maxWalkDistance;
	public Transform test;
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}

	void FixedUpdate() {

		//Agent has no path, but wants a path
		if (/*!agent.hasPath && */wantsPath) {
			
			//wantsPath = false;
			
			//Roll dice for distance
			var distance = Range(0f, maxWalkDistance);
			
			//Roll dice for direction
			var angle = (test.position - transform.position);
			//var angle = (test.position - transform.position);

			/*Debug.Log(angle);*/
			agent.SetDestination(test.position);

		}
		else {
			
		}
		
	}
}
