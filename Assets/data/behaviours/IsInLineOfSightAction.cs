using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsInLineOfSight", story: "Update [LineOfSightDetector] to check if we can see [PotentialTarget] and Assign [Target]", category: "Action", id: "97c841c97b28d23a207e663fae9a34ce")]
public partial class IsInLineOfSightAction : Action {
	[SerializeReference] public BlackboardVariable<LineOfSightDetector> LineOfSightDetector;
	[SerializeReference] public BlackboardVariable<GameObject> PotentialTarget;
	[SerializeReference] public BlackboardVariable<GameObject> Target;


	protected override Status OnUpdate() {
		Target.Value = LineOfSightDetector.Value.PerformDetection(PotentialTarget);
		return Target.Value == null ? Status.Failure : Status.Success;
	}
	
}
