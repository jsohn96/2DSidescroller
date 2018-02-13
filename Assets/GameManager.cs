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
	Rest = 6
}

public class GameManager : MonoBehaviour {

	enum PlayState {
		Init = 0,
		Strategize = 1,
		PlayerTurn = 2,
		EnemyTurn = 3

	}

	PlayState _currentPlayState = PlayState.Init;
			
	public static GameManager _gameManagerInstance = null;

	public int HowManyMovesPerTurn {
		get {return _howManyMovesPerTurn; }
	}
	[SerializeField] int _howManyMovesPerTurn = 3;

	[Header("Manager References")]
	[SerializeField] PlayerStrategyManager _playerStrategyManager;
	[SerializeField] PlayerCommandManager _playerCommandManager;
	[SerializeField] EnemyTurnManager _enemyTurnManager;

	void Awake(){
		if (_gameManagerInstance == null) {
			_gameManagerInstance = this;
		} else if (_gameManagerInstance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}

	public void MoveToNextState(){
		switch (_currentPlayState) {
		case PlayState.Strategize:
			_currentPlayState = PlayState.PlayerTurn;
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

	}
}
