using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateUI : MonoBehaviour {
	[SerializeField] Text _strategy;
	[SerializeField] Text _playerAction;
	[SerializeField] Text _enemyAction;
	[SerializeField] Text _completed;

	PlayState _prevPlayState = PlayState.Init;

	[Header("Reference to Final Stat # Text")]
	[SerializeField] Text[] _finalCntText = new Text[3];
	[SerializeField] Text _finalComment;
	[SerializeField] GameObject _finalStats;
	string[] _finalCommentStrings = new string[3]{"You did well", "Eh, you could have done better", "Murderer!, How could you kill those poor robots"};

	[SerializeField]GraphicRaycaster _graphicRaycaster;

	int _enemyKills = 0;
	public int EnemyKills {
		get { return _enemyKills; }
		set { _enemyKills = value; }
	}
	int _timesHit = 0;
	public int TimesHit {
		get { return _timesHit; }
		set { _timesHit = value; }
	}
	int _numberOfRounds = 0;
	public int NumberOfRounds {
		get { return _numberOfRounds; }
		set { _numberOfRounds = value; }
	}

	public static GameStateUI _gameStatsInstance = null;
	Fading _fadeScript;
	bool _tookButtonInputOnce = false;


	void Awake(){
		if (_gameStatsInstance == null) {
			_gameStatsInstance = this;
		} else if (_gameStatsInstance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}

	void Start(){
		_fadeScript = GameObject.Find ("Fade").GetComponent<Fading> ();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		_completed.enabled = false;
		_tookButtonInputOnce = false;
		if (scene.buildIndex == 2) {
			DisplayStats ();
		} else {
			//Reset all stats when entering game scene
			if (scene.buildIndex == 1) {
				_enemyKills = 0;
				_timesHit = 0;
				_numberOfRounds = 0;
			}
			_graphicRaycaster.enabled = false;
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}


	public void SwitchStateUI(PlayState currentPlayState){
		DisablePreviousPlayState ();
		switch (currentPlayState) {
		case PlayState.Strategize:
			_strategy.enabled = true;
			break;
		case PlayState.PlayerTurn:
			_playerAction.enabled = true;
			break;
		case PlayState.EnemyTurn:
			_enemyAction.enabled = true;
			break;
		case PlayState.Completed:
			_completed.enabled = true;
			break;
		default:
			break;
		}
		_prevPlayState = currentPlayState;
	}

	void DisablePreviousPlayState(){
		switch (_prevPlayState) {
		case PlayState.Strategize:
			_strategy.enabled = false;
			break;
		case PlayState.PlayerTurn:
			_playerAction.enabled = false;
			break;
		case PlayState.EnemyTurn:
			_enemyAction.enabled = false;
			break;
		default:
			break;
		}
	}

	public void ReturnToTitle(){
		if (!_tookButtonInputOnce) {
			_tookButtonInputOnce = true;
			StartCoroutine (ChangeLevel (0, 1f));
		}
	}

	public void RestartGame(){
		if (!_tookButtonInputOnce) {
			_tookButtonInputOnce = true;
			StartCoroutine (ChangeLevel (1, 1f));
		}
	}

	public void ShowEndStats(){
		StartCoroutine (ChangeLevel (2, 2f));
	}

	void DisplayStats(){
		_graphicRaycaster.enabled = true;
		_finalStats.SetActive (true);
		_finalCntText [0].text = _enemyKills.ToString ();
		_finalCntText [1].text = _timesHit.ToString ();
		_finalCntText [2].text = _numberOfRounds.ToString ();
		if (_enemyKills > 0) {
			_finalComment.text = _finalCommentStrings [2];
			_finalComment.color = new Color (1f, 0f, 0f);
		} else {
			if (_timesHit == 0) {
				_finalComment.text = _finalCommentStrings [0];
				_finalComment.color = new Color (0.1f, 1f, 0.2f);
			} else {
				_finalComment.text = _finalCommentStrings [1];
				_finalComment.color = new Color (0.8f, 0.8f, 0f);
			}
		}
	}

	IEnumerator ChangeLevel(int sceneIndex, float fadeDelay){
		yield return new WaitForSeconds (fadeDelay);
		_finalStats.SetActive (false);
		float fadeTime = _fadeScript.BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (sceneIndex);
	}
}
