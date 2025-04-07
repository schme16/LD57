using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdatePen", story: "Check [Triggers] and Update [Pen]", category: "Action", id: "54e4961315209bff532b790f6879fdc1")]
public partial class UpdatePenAction : Action {
	[SerializeReference] public BlackboardVariable<CheckEnteredPen> Triggers;
	[SerializeReference] public BlackboardVariable<GameObject> Pen;

	protected override Status OnUpdate() {
		Pen.Value = Triggers.Value.UpdateCheck();
		return Pen is not null ? Status.Success : Status.Failure;
	}


}
