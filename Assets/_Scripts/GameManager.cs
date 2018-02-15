using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveSet {
	AttackLeft = 0,
	AttackRight = 1,
	MoveLeft = 2,
	MoveRight = 3,
	JumpUp = 4,
	JumpDown = 5,
	Rest = 6,
	none = 7
}

public enum PlayState {
	Init = 0,
	Strategize = 1,
	PlayerTurn = 2,
	EnemyTurn = 3

}

public class GameManager : MonoBehaviour {

	PlayState _currentPlayState = PlayState.Strategize;
			
	public static GameManager _gameManagerInstance = null;

	public int HowManyMovesPerTurn {
		get {return _howManyMovesPerTurn; }
	}
	[SerializeField] int _howManyMovesPerTurn = 3;

	[Header("Manager References")]
	[SerializeField] PlayerStrategyManager _playerStrategyManager;
	[SerializeField] PlayerCommandManager _playerCommandManager;
	[SerializeField] EnemyTurnManager _enemyTurnManager;

	[SerializeField] GameStateUI _gameStateUI;
	[SerializeField] TogglePlayerStrategyUI _togglePlayerStrategyUI;

	void Awake(){
//		if (_gameManagerInstance == null) {
			_gameManagerInstance = this;
//		}
//		else if (_gameManagerInstance != this) {
//			Destroy (gameObject);
//		}
//		DontDestroyOnLoad (this);
	}

	void Start(){
		HandleCurrentState ();
	}

	void Update(){
		//Debug Key to speed up time
		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			Time.timeScale = 3.0f;
		} else if (Input.GetKeyUp(KeyCode.LeftControl)) {
			Time.timeScale = 1.0f;
		}
	}

	public void MoveToNextState(){
		switch (_currentPlayState) {
		case PlayState.Strategize:
			_currentPlayState = PlayState.PlayerTurn;
			_togglePlayerStrategyUI.ToggleStrategyPhaseOn (false);
			break;
		case PlayState.PlayerTurn:
			//TODO: Check if this ends the game
			_currentPlayState = PlayState.EnemyTurn;
			break;
		case PlayState.EnemyTurn:
			//TODO: Check if this ends the game
			_currentPlayState = PlayState.Strategize;
			break;
		default:
			break;
		}
		HandleCurrentState ();
	}

	void HandleCurrentState(){
		switch (_currentPlayState) {
		case PlayState.Strategize:
			_gameStateUI.SwitchStateUI (_currentPlayState);
			_togglePlayerStrategyUI.ToggleStrategyPhaseOn (true);
			_playerStrategyManager.BeginPlayerStrategy ();
			break;
		case PlayState.PlayerTurn:
			_gameStateUI.SwitchStateUI (_currentPlayState);
			_playerCommandManager.BeginPlayerCommand ();
			break;
		case PlayState.EnemyTurn:
			_gameStateUI.SwitchStateUI (_currentPlayState);
			_enemyTurnManager.BeginEnemyTurn ();

			break;
		default:
			break;
		}
	}
}
