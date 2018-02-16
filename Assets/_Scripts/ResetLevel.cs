using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour {

	[SerializeField] GameObject _resetConfirmation;

	//Toggle the Reset Confirmation window On
	public void Reset(){
		_resetConfirmation.SetActive (true);
	}

	// Reset Level
	public void ResetConfirm(){
		StartCoroutine (ChangeLevel ());
	}

	// Close Reset Confirmation Window
	public void CloseResetConfirm(){
		_resetConfirmation.SetActive (false);
	}

	IEnumerator ChangeLevel(){
		yield return new WaitForSeconds (0.5f);
		float fadeTime = GameObject.Find("Fade").GetComponent<Fading>().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (1);
	}

}
