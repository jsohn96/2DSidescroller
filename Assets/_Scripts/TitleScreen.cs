using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Manages processes in the title screen
public class TitleScreen : MonoBehaviour {
	Fading _fadeScript;
	AsyncOperation async;
	[SerializeField] Image _instructionImage;

	void Start(){
		_fadeScript = GameObject.Find ("Fade").GetComponent<Fading> ();

		StartCoroutine (LoadAsyncScene ());
	}

	public void ExitGame(){
		AudioManager._audioManagerInstance.PlayButtonClick ();
		Application.Quit ();
	}

	public void ShowControls() {
		AudioManager._audioManagerInstance.PlayButtonClick ();
		_instructionImage.enabled = !_instructionImage.enabled;
	}

	public void StartGame() {
		AudioManager._audioManagerInstance.PlayButtonClick ();
		StartCoroutine (ChangeLevel ());
	}


	IEnumerator LoadAsyncScene(){
		async = SceneManager.LoadSceneAsync (1);

		async.allowSceneActivation = false;
		yield return async;
	}

	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds (0.5f);
		float fadeTime = _fadeScript.BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		async.allowSceneActivation = true;
	}
}
