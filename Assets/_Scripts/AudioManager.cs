using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager _audioManagerInstance;
	//Since there arent that many clips, keep a separate audiosource for each audio clip
	[SerializeField] AudioSource _ambientAudio, _buttonClickAudio, _deathAudio, _turnAroundAudio, _drawCardAudio, _placeCardAudio, _punchAudio, _glitchAudio, _walkAudio;

	void Awake(){
		if (_audioManagerInstance == null) {
			_audioManagerInstance = this;
		} else if (_audioManagerInstance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}

	void Start(){
		_ambientAudio.loop = true;
		_ambientAudio.Play ();
	}

	public void PlayButtonClick(){
		_buttonClickAudio.Play ();
	}

	public void PlayDeath(){
		_deathAudio.Play ();
	}

	public void PlayTurnAround(){
		_turnAroundAudio.Play ();
	}

	public void PlayDrawCard(){
		if (!_drawCardAudio.isPlaying) {
			_drawCardAudio.Play ();
		}
	}

	public void PlayPlaceCard(){
		if (!_placeCardAudio.isPlaying) {
			_placeCardAudio.Play ();
		}
	}

	public void PlayGlitch(){
		_glitchAudio.Play ();
	}


	public void PlayPunch(){
		_punchAudio.Play ();
	}

	public void PlayWalk(){
		if (!_walkAudio.isPlaying) {
			_walkAudio.Play ();
		}
	}
}
