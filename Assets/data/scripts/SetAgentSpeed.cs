using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class SetAgentSpeed : MonoBehaviour {

	public NavMeshAgent agent;
	public BehaviorGraphAgent bAgent;
	public GameObject pen;
	public float speed;


	private void Start() {
		agent = GetComponent<NavMeshAgent>();
		bAgent = GetComponent<BehaviorGraphAgent>();

		agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
	}

	// Update is called once per frame
	void Update() {
		bAgent.SetVariableValue("Pen", pen);
		agent.speed = speed;
	}
}
