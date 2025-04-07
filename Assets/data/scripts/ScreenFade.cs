using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

	public float fadeTime;
	public bool fadeOut;
	public Image image;


	private async void Start() {

		if (fadeOut) {
			var col = image.color;
			col.a = 0;

			image.color = col;
			await _.FadeOut(image, fadeTime);
		}
		else {
			var col = image.color;
			col.a = 1;
			image.color = col;
			await _.FadeIn(image, fadeTime);
		}
	}
}


