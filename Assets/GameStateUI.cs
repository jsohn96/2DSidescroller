using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour {
	[SerializeField] Text _strategy;
	[SerializeField] Text _playerAction;
	[SerializeField] Text _enemyAction;

	PlayState _prevPlayState = PlayState.Init;


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
