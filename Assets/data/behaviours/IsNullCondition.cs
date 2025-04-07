using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Variable is NULL", story: "Check [Variable] is NULL", category: "Conditions", id: "987a075f647d156246abf054ed7daa38")]
public partial class IsNullCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Variable;


	public override bool IsTrue() {

		return Variable.Value == null || Variable.Value is null || ReferenceEquals(Variable.Value, null);
	}
}
