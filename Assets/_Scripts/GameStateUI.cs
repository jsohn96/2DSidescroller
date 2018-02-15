using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour {
	[SerializeField] Text _strategy;
	[SerializeField] Text _playerAction;
	[SerializeField] Text _enemyAction;
	[SerializeField] Text _completed;

	PlayState _prevPlayState = PlayState.Init;

	int _enemyKills = 0;
	public int EnemyKills {
		get { return _enemyKills; }
	}
	int _timesHit = 0;
	public int TimesHit {
		get { return _timesHit; }
	}
	int _numberOfRounds = 0;
	public int NumberOfRounds {
		get { return _numberOfRounds; }
	}

	GameStateUI _gameStatsInstance = null;

	void Awake(){
		if (_gameStatsInstance == null) {
			_gameStatsInstance = this;
		} else if (_gameStatsInstance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
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
}
