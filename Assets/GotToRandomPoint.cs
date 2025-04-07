using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GotToRandomPoint : MonoBehaviour {

	public bool readyForNextPoint = true;
	public Vector3 point;
	public List<Transform> points;


	void Start() {

	}

	private async void Update() {
		if (readyForNextPoint) {
			readyForNextPoint = false;
			point = points[Random.Range(0, points.Count)].position;
			point.y = transform.position.y;

			transform.LookAt(point);
			
			await _.Translate(transform, point, 0.2f, EasingFunction.Ease.EaseInOutSine);
			await UniTask.Delay(Random.Range(500, 6500));
			await _.Rotate(transform, new Vector3(transform.eulerAngles.x, Vector3.Angle(transform.position, point), transform.eulerAngles.z), 0.2f, EasingFunction.Ease.EaseInOutSine);
			readyForNextPoint = true;
		}
	}

}
