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

public class PlayerCommandManager : MonoBehaviour {
	
	[SerializeField] PlayerRobotController _playerRobotController;

	[SerializeField] PlayerMoveSet[] _playerMoveSetArray;

	int _totalMoveCnt;

	int _currentTurnCnt = 0;

	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
		_playerMoveSetArray = new PlayerMoveSet[_totalMoveCnt];
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.F)) {
			RelayCommand ();
		}
	}
		

	//Iterate Over the player commands during the players turn
	void RelayCommand(){
		float waitForNextCommandDuration;

		PlayerMoveSet tempPlayerMoveSet = _playerMoveSetArray [_currentTurnCnt];
		waitForNextCommandDuration = _playerRobotController.HandlePlayerCommand (tempPlayerMoveSet);
		_currentTurnCnt++;

		StartCoroutine (WaitForNextCommand (waitForNextCommandDuration));
	}

	//Wait for current command to finish before calling next command
	IEnumerator WaitForNextCommand(float waitDuration){
		yield return new WaitForSeconds (waitDuration);
		if (_currentTurnCnt < _totalMoveCnt) {
			RelayCommand ();
		} else {
			EndPlayerTurn ();
		}
	}

	void EndPlayerTurn(){
		_currentTurnCnt = 0;
		GameManager._gameManagerInstance.MoveToNextState ();
	}
}
