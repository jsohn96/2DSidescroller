using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStrategyManager : MonoBehaviour {

	int _totalMoveCnt;
	PlayerMoveSet[] _tentativePlayerMoveSet;
	[SerializeField] CommandHand[] _slots = new CommandHand[3];

	[SerializeField] PlayerCommandManager _playerCommandManager;

	[SerializeField] CommandDeck _commandDeck;

	[SerializeField] Button _confirmButton;

	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
		_tentativePlayerMoveSet = new PlayerMoveSet[_totalMoveCnt];
	}


	public void ConfirmStrategy(){
		AudioManager._audioManagerInstance.PlayButtonClick ();
		int slotCnt = _slots.Length;
		for (int i = 0; i < slotCnt; i++) {
			PlayerMoveSet tempMoveSet = _slots [i].CheckOccupancy ();
			if (tempMoveSet != PlayerMoveSet.none) {
				_tentativePlayerMoveSet [i] = tempMoveSet;
			} else {
				Debug.Log (tempMoveSet);
				ReturnAllCardsToHand (slotCnt);
				return;
			}
		}
		_playerCommandManager.FeedInCommands (_tentativePlayerMoveSet);
		GameManager._gameManagerInstance.MoveToNextState ();
		RemoveConfirmedCardsFromPlay (slotCnt);
		_confirmButton.interactable = false;
	}

	void ReturnAllCardsToHand(int slotCnt){
		for (int i = 0; i < slotCnt; i++) {
			_slots [i].ReturnOccupant ();
		}
	}

	void RemoveConfirmedCardsFromPlay(int slotCnt){
		for (int i = 0; i < slotCnt; i++) {
			_slots [i].RemoveOccupant ();
			_commandDeck.CommandCardsInPlay--;
		}

	}

	public void BeginPlayerStrategy(){
		_confirmButton.interactable = true;
	}
}
