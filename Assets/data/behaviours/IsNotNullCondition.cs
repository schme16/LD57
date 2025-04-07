using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsNotNull", story: "Check [Variable] is Not NULL", category: "Conditions", id: "ca7e1cf52928091598a90c0a75e311c5")]
public partial class IsNotNullCondition : Condition {
	[SerializeReference] public BlackboardVariable<GameObject> Variable;

	public override bool IsTrue() {
		return !ReferenceEquals(Variable.Value, null);
	}
}
