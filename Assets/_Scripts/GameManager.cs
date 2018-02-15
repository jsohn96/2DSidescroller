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
	EnemyTurn = 3,
	Completed = 4
}

public class GameManager : MonoBehaviour {

	PlayState _currentPlayState = PlayState.Strategize;
			
	public static GameManager _gameManagerInstance = null;
	int _damageCnt = 0;
	public int DamageCnt {
		get { return _damageCnt; }
		set { _damageCnt = value; }
	}

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
	[SerializeField] CameraManager _cameraManager;

	bool _gameCompleted = false;

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

	public void GameCompleted(){
		_gameCompleted = true;
		_currentPlayState = PlayState.Completed;
		_gameStateUI.SwitchStateUI (_currentPlayState);
	}

	public void MoveToNextState(){
		if (!_gameCompleted) {
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
	}

	void HandleCurrentState(){
		if (!_gameCompleted) {
			switch (_currentPlayState) {
			case PlayState.Strategize:
				_gameStateUI.SwitchStateUI (_currentPlayState);
				_togglePlayerStrategyUI.ToggleStrategyPhaseOn (true);
				_playerStrategyManager.BeginPlayerStrategy ();
				_cameraManager.SwitchCameraPhase ();
				break;
			case PlayState.PlayerTurn:
				_gameStateUI.SwitchStateUI (_currentPlayState);
				_playerCommandManager.BeginPlayerCommand ();
				_cameraManager.SwitchCameraPhase ();
				break;
			case PlayState.EnemyTurn:
				_gameStateUI.SwitchStateUI (_currentPlayState);
				_enemyTurnManager.BeginEnemyTurn ();
				_cameraManager.SwitchCameraPhase ();

				break;
			default:
				break;
			}
		}
	}
}
