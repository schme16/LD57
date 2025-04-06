using System;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public RectTransform gameTitle;
	public RectTransform StartButton;
	public RectTransform OptionsButton;


	public RectTransform optionsTitle;
	public Slider audioSlider;
	public RectTransform audioSliderRect;
	public Slider musicSlider;
	public RectTransform musicSliderRect;
	public RectTransform BackToMainMenuButton;
	public StudioEventEmitter ambienceEmitter;
	public ScreenFade screenFader;

	public bool muteState;

	private Bus masterBus;
	private Bus MusicBus;
	private Bus SFXBus;
	public float currentSFXVolume;
	public float currentMusicVolume;
	public EasingFunction.Ease easeIn;
	public EasingFunction.Ease easeOut;


	private void Awake() {
		DontDestroyOnLoad(this);
	}

	private async void Start() {

		easeIn = EasingFunction.Ease.EaseInOutBack;
		easeOut = EasingFunction.Ease.EaseInOutBack;


		currentSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
		currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
		audioSlider.SetValueWithoutNotify(currentSFXVolume);
		musicSlider.SetValueWithoutNotify(currentMusicVolume);

		await UniTask.Delay(250);

		
		masterBus = RuntimeManager.GetBus("bus:/");

		MusicBus = RuntimeManager.GetBus("bus:/Music");
		MusicBus.setVolume(currentSFXVolume);

		SFXBus = RuntimeManager.GetBus("bus:/SFX");
		SFXBus.setVolume(currentSFXVolume);

		ambienceEmitter.Play();
		
		SetSFXVolume(currentSFXVolume);
		SetMusicVolume(currentMusicVolume);


		optionsTitle.anchoredPosition = new Vector3(0, 50, 0);
		audioSliderRect.anchoredPosition = new Vector3(0, -438.8226f, 0);
		musicSliderRect.anchoredPosition = new Vector3(0, -588.8226f, 0);
		BackToMainMenuButton.anchoredPosition = new Vector3(0, -215, 0);


		gameTitle.anchoredPosition = new Vector3(0, 50, 0);
		StartButton.anchoredPosition = new Vector3(0, -115, 0);
		OptionsButton.anchoredPosition = new Vector3(0, -215, 0);



		ShowMainMenu();

		/*if (muteState) {
			muteIcon.SetActive(true);
			unMuteIcon.SetActive(false);
		}
		else {
			muteIcon.SetActive(false);
			unMuteIcon.SetActive(true);
		}*/
	}

	// Update is called once per frame
	void Update() {

	}

	public void SetSFXVolume(float volume) {

		currentSFXVolume = volume;

		PlayerPrefs.SetFloat("SFXVolume", volume);
		PlayerPrefs.Save();

		//Set the volume on the master bus
		SFXBus.setVolume(currentSFXVolume);
	}

	public void SetMusicVolume(float volume) {

		currentMusicVolume = volume;

		//Set the volume on the master bus
		MusicBus.setVolume(currentMusicVolume);

		PlayerPrefs.SetFloat("MusicVolume", volume);
		PlayerPrefs.Save();
	}

	/*public void ToggleMute() {

		muteState = !muteState;

		if (muteState) {
			muteIcon.SetActive(true);
			unMuteIcon.SetActive(false);
		}
		else {
			muteIcon.SetActive(false);
			unMuteIcon.SetActive(true);
		}
	}*/

	public async void StartGame() {
		screenFader.gameObject.SetActive(true);
		await UniTask.Delay((int)(screenFader.fadeTime * 1000) + 250);
		await SceneManager.LoadSceneAsync("data/scenes/main");
	}

	public async void ShowMainMenu() {
		_.Translate(optionsTitle, new Vector3(0, 50, 0), 1, easeOut);
		_.Translate(audioSliderRect, new Vector3(0, -438.8226f, 0), 1, easeOut);
		_.Translate(musicSliderRect, new Vector3(0, -588.8226f, 0), 1, easeOut);
		_.Translate(BackToMainMenuButton, new Vector3(0, -212, 0), 1, easeOut);

		_.Translate(gameTitle, new Vector3(0, -22, 0), 1, easeIn);
		_.Translate(StartButton, new Vector3(0, 150, 0), 1, easeIn);
		_.Translate(OptionsButton, new Vector3(0, 50, 0), 1, easeIn);
	}

	public async void ShowOptions() {
		_.Translate(gameTitle, new Vector3(0, 50, 0), 1, easeOut);
		_.Translate(StartButton, new Vector3(0, -115, 0), 1, easeOut);
		_.Translate(OptionsButton, new Vector3(0, -215, 0), 1, easeOut);


		_.Translate(optionsTitle, new Vector3(0, -22, 0), 1, easeIn);
		_.Translate(audioSliderRect, new Vector3(0, 0, 0), 1, easeIn);
		_.Translate(musicSliderRect, new Vector3(0, -150, 0), 1, easeIn);
		_.Translate(BackToMainMenuButton, new Vector3(0, 50, 0), 1, easeIn);

	}
}
