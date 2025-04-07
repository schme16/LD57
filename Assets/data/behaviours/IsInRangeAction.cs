using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsInRange", story: "Update [RangeDetector] and Assign [PotentialTarget]", category: "Action", id: "86f42e2221695629997a1a5dfc37747f")]
public partial class IsInRangeAction : Action {
	[SerializeReference] public BlackboardVariable<RangeDetector> RangeDetector;
	[SerializeReference] public BlackboardVariable<GameObject> PotentialTarget;

	protected override Status OnUpdate() {
		PotentialTarget.Value = RangeDetector.Value.UpdateDetector();
		return PotentialTarget.Value == null ? Status.Failure : Status.Success;
	}

}
