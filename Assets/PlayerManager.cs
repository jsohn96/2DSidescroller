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

public class PlayerManager : MonoBehaviour {
	
	[SerializeField] PlayerRobotController _playerRobotController;

	PlayerMoveSet[] _playerMoveSetArray;

	int _totalMoveCnt;

	int _currentTurnCnt = 0;
	//float _waitForNextCommandDuration = 0f;


	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
		_playerMoveSetArray = new PlayerMoveSet[_totalMoveCnt];
	}
		

	//Handle the current Player turn Action
	void RelayCommand(){
		float waitForNextCommandDuration;

		PlayerMoveSet tempPlayerMoveSet = _playerMoveSetArray [_currentTurnCnt];

		waitForNextCommandDuration = _playerRobotController.HandlePlayerCommand (tempPlayerMoveSet);
		_currentTurnCnt++;

		StartCoroutine (WaitForNextCommand (waitForNextCommandDuration));
	}

	IEnumerator WaitForNextCommand(float waitDuration){
		yield return new WaitForSeconds (waitDuration);
		if (_currentTurnCnt < _totalMoveCnt) {
			RelayCommand ();
		}
	}
}
