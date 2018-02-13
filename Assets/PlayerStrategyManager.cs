using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrategyManager : MonoBehaviour {

	int _totalMoveCnt;
	PlayerMoveSet[] _tentativePlayerMoveSet;

	[SerializeField] PlayerCommandManager _playerCommandManager;

	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
		_tentativePlayerMoveSet = new PlayerMoveSet[_totalMoveCnt];
	}


	public void ConfirmStrategy(){
		for (int i = 0; i < _totalMoveCnt; i++) {
			if (_tentativePlayerMoveSet [i] == null) {
				//TODO: Handle the failed Confirmation
				return;
			}
		}
		_playerCommandManager.FeedInCommands (_tentativePlayerMoveSet);
		GameManager._gameManagerInstance.MoveToNextState ();
	}
}
