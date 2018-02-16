using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Toggles timescale to alter in game speed
public class ToggleSpeed : MonoBehaviour {
	Text _text;

	bool _isSpedUp = false;

	void Start(){
		_text = GetComponent<Text> ();
	}

	public void ToggleGameSpeed() {
		AudioManager._audioManagerInstance.PlayButtonClick ();
		if (_isSpedUp) {
			Time.timeScale = 1.0f;
			_text.text = "Speed x3";
		} else {
			Time.timeScale = 3.0f;
			_text.text = "Speed x1";
		}
		_isSpedUp = !_isSpedUp;
	}
}
