using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class _ : MonoBehaviour {

	//Set the mouse lock to "confine", which means you can move the mouse but only inside the window, used for UI interactions
	public static void ConfineMouse() {
		Cursor.lockState = CursorLockMode.Confined;
	}

	//Set the mouse lock to "locked", which means you cannot move the mouse cursor visibly, and will "locked" to mthe screen center
	public static void LockMouse() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	public static void SetLockMode(CursorLockMode mode) {
		Cursor.lockState = mode;
	}

	public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, float volume = 0.5f, float pitch = 1f, bool playOnAwake = true) {
		var go = new GameObject("OneShotAudioClip");
		go.transform.position = pos;

		var source = go.AddComponent<AudioSource>();
		source.loop = false;
		source.volume = volume;
		source.pitch = pitch;
		source.clip = clip;
		source.playOnAwake = playOnAwake;
		if (playOnAwake) {
			source.Play();
		}

		Destroy(go, clip.length);
		return source;
	}


	//Scales a transform to a specified Vector3, can be awaited
	public async static UniTask FadeOut(SpriteRenderer sprite, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = sprite.color;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			sprite.color = Color.Lerp(startValue, new Color(startValue.r, startValue.g, startValue.b, 0), easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		sprite.color = new Color(startValue.r, startValue.g, startValue.b, 0);
	}




	//Scales a transform to a specified Vector3, can be awaited
	public async static UniTask FadeIn(SpriteRenderer sprite, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = sprite.color;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			sprite.color = Color.Lerp(startValue, new Color(startValue.r, startValue.g, startValue.b, 1), easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		sprite.color = new Color(startValue.r, startValue.g, startValue.b, 1);
	}




	//Scales a transform to a specified Vector3, can be awaited
	public async static UniTask FadeOut(Image sprite, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = sprite.color;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			sprite.color = Color.Lerp(startValue, new Color(startValue.r, startValue.g, startValue.b, 1), easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		sprite.color = new Color(startValue.r, startValue.g, startValue.b, 1);
	}




	//Scales a transform to a specified Vector3, can be awaited
	public async static UniTask FadeIn(Image sprite, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = sprite.color;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			sprite.color = Color.Lerp(startValue, new Color(startValue.r, startValue.g, startValue.b, 0), easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		sprite.color = new Color(startValue.r, startValue.g, startValue.b, 0);
	}

	//Rotates a transform to a specified euler, can be awaited
	public async static UniTask Rotate(Transform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = trans.eulerAngles;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.eulerAngles = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}
		trans.eulerAngles = destination;
	}

	//Rotates a transform to a specified euler, can be awaited
	public async static UniTask RotateLocal(Transform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = trans.localEulerAngles;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.localEulerAngles = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}
		trans.localEulerAngles = destination;
	}

	//Scales a transform to a specified Vector3, can be awaited
	public async static UniTask Scale(Transform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {

		float t = 0f;
		var startValue = trans.localScale;
		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.localScale = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		trans.localScale = destination;
	}

	//Translates a transform to a specified Vector3, can be awaited
	public async static UniTask Translate(Transform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad, CancellationToken cancellationToken = default) {


		float t = 0f;
		var startValue = trans.position;

		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.position = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
		}

		trans.position = destination;
	}

	//Translates a RectTransform to a specified Vector3, can be awaited
	public async static UniTask Translate(RectTransform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad) {


		float t = 0f;
		var startValue = trans.anchoredPosition;

		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.anchoredPosition = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}

		trans.anchoredPosition = destination;
	}


	//Translates a transform to a specified Vector3, can be awaited
	public async static UniTask TranslateLocal(Transform trans, Vector3 destination, float speed = 1.05f, EasingFunction.Ease easingFunction = EasingFunction.Ease.EaseInQuad, CancellationToken cancellationToken = default) {


		float t = 0f;
		var startValue = trans.localPosition;

		var easeing = new EasingFunction().GetEasingFunction(easingFunction);

		while (t < 1) {

			//Set the angle
			trans.localPosition = Vector3.Lerp(startValue, destination, easeing(0, 1, t));

			//Update the time value
			t = Mathf.Clamp(t + (Time.deltaTime * speed), 0, 1);
			await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
		}

		trans.localPosition = destination;
	}

	//Maps a given value, from one range to another
	public static float Map(float aValue, float aLow, float aHigh, float bLow, float bHigh) {
		var normal = Mathf.InverseLerp(aLow, aHigh, aValue);
		return Mathf.Lerp(bLow, bHigh, normal);
	}

	//Get's a random point inside nearly any given collider
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

	//Get's a random point inside a box collider
	private static Vector3 GetRandomPointInBoxCollider(BoxCollider boxCollider) {
		Vector3 extents = boxCollider.size * 0.5f;
		Vector3 randomPoint = new Vector3(
			Random.Range(-extents.x, extents.x),
			Random.Range(-extents.y, extents.y),
			Random.Range(-extents.z, extents.z)
		);
		return boxCollider.transform.TransformPoint(boxCollider.center + randomPoint);
	}

	//Get's a random point inside a sphere collider
	private static Vector3 GetRandomPointInSphereCollider(SphereCollider sphereCollider) {
		Vector3 randomDirection = Random.onUnitSphere * Random.Range(0f, 1f);
		return sphereCollider.transform.TransformPoint(sphereCollider.center + randomDirection * sphereCollider.radius);
	}

	//Get's a random point inside a capsule collider
	private static Vector3 GetRandomPointInCapsuleCollider(CapsuleCollider capsuleCollider) {
		float height = Mathf.Max(0, capsuleCollider.height * 0.5f - capsuleCollider.radius);
		Vector3 randomDirection = Random.insideUnitSphere * capsuleCollider.radius;
		float randomHeight = Random.Range(-height, height);
		Vector3 randomPoint = new Vector3(randomDirection.x, randomHeight, randomDirection.z);
		return capsuleCollider.transform.TransformPoint(capsuleCollider.center + randomPoint);
	}

	//Get's a random point inside a mesh collider
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

	//Get's a random point inside a triangle (really only useful for the GetRandomPointInMeshCollider func)
	private static Vector3 GetRandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c) {
		float r1 = Mathf.Sqrt(Random.value);
		float r2 = Random.value;
		return (1 - r1) * a + (r1 * (1 - r2)) * b + (r1 * r2) * c;
	}




	public async static UniTask PlayAudio(StudioEventEmitter audio, GameObject objectToPlayAt = null, CancellationToken cancellationToken = default) {

		if (objectToPlayAt is null) {
			objectToPlayAt = PlayerScript.player.gameObject;
		}
		
		try {

			/*// Create the event instance
			EventInstance eventInstance = RuntimeManager.CreateInstance(audio);*/

			// Start the event
			audio.EventInstance.start();
			audio.Play();

			// Create a completion source that will be triggered when the event finishes
			var completionSource = new UniTaskCompletionSource();

			// Poll the playback state until it's no longer playing
			// Also continuously update the 3D position
			await UniTask.Create(async () => {

				audio.EventDescription.getLength(out var totalLengthInMilliseconds);
				audio.EventInstance.getTimelinePosition(out var currentPosition);

				while (currentPosition <= totalLengthInMilliseconds) {
					audio.EventInstance.getTimelinePosition(out currentPosition);

					// Update the 3D attributes to the current player position
					audio.EventInstance.set3DAttributes(objectToPlayAt.To3DAttributes());

					completionSource.TrySetResult();
					await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellationToken); // Update position more frequently (50 times per second)
				}

				// Clean up the event instance
				audio.EventInstance.release();

				var triggerAfterPlay = audio.GetComponent<TriggerAfterPlay>();
				if (triggerAfterPlay) {
					triggerAfterPlay.OnPlayed.Invoke();
				}


			});

			await completionSource.Task;
		}
		catch (Exception ex) {
			Debug.LogError($"Error playing FMOD audio: {ex.Message}");
		}
	}



}
