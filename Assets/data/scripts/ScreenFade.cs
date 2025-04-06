using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

	public float fadeTime;
	public bool fadeOut;
	public Image image;


	private async void Start() {

		if (fadeOut) {
			image.color = new Color(0,0,0, 0);
			await _.FadeOut(image, fadeTime);
		}
		else {
			image.color = Color.black;
			await _.FadeIn(image, fadeTime);
		}
	}
}


