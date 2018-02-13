using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandManager : MonoBehaviour {
	
	[SerializeField] PlayerRobotController _playerRobotController;

	[SerializeField] PlayerMoveSet[] _playerMoveSetArray;

	int _totalMoveCnt;

	int _currentMoveCnt = 0;

	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.F)) {
			RelayCommand ();
		}
	}

	public void FeedInCommands(PlayerMoveSet[] playerMoves){
		_playerMoveSetArray = playerMoves;
	}
		

	//Iterate Over the player commands during the players turn
	void RelayCommand(){
		float waitForNextCommandDuration;

		PlayerMoveSet tempPlayerMoveSet = _playerMoveSetArray [_currentMoveCnt];
		waitForNextCommandDuration = _playerRobotController.HandlePlayerCommand (tempPlayerMoveSet);
		_currentMoveCnt++;

		StartCoroutine (WaitForNextCommand (waitForNextCommandDuration));
	}

	//Wait for current command to finish before calling next command
	IEnumerator WaitForNextCommand(float waitDuration){
		yield return new WaitForSeconds (waitDuration);
		if (_currentMoveCnt < _totalMoveCnt) {
			RelayCommand ();
		} else {
			EndPlayerTurn ();
		}
	}

	public void BeginPlayerCommand() {
		RelayCommand ();
	}

	void EndPlayerTurn(){
		_currentMoveCnt = 0;
		GameManager._gameManagerInstance.MoveToNextState ();
	}
}
