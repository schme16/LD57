using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Target with Line Of Sight Detector", story: "Check [PotentialTarget] with [LineOfSightDetector]", category: "Conditions", id: "065d9342e4f81e57d64afe56741552e7")]
public partial class CheckTargetWithLineOfSightDetectorCondition : Condition {
	[SerializeReference] public BlackboardVariable<GameObject> PotentialTarget;
	[SerializeReference] public BlackboardVariable<LineOfSightDetector> LineOfSightDetector;

	public override bool IsTrue() {
		if (PotentialTarget == null) {
			return false;
		}
		else {
			return LineOfSightDetector.Value.PerformDetection(PotentialTarget.Value) != null;
		}
	}
}
