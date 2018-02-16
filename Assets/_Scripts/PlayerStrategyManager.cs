using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Responsible for Strategy Play State of receiving and processing player input 
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

	//Triggered by button press 
	// Checks if all appropriate slots have been filled to either accept or reject player sequence input
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

	// Fail state for player input
	void ReturnAllCardsToHand(int slotCnt){
		for (int i = 0; i < slotCnt; i++) {
			_slots [i].ReturnOccupant ();
		}
	}

	// success state for player input
	void RemoveConfirmedCardsFromPlay(int slotCnt){
		for (int i = 0; i < slotCnt; i++) {
			_slots [i].RemoveOccupant ();
			_commandDeck.CommandCardsInPlay--;
		}

	}

	//Triggered when game manager enters strategy phase
	public void BeginPlayerStrategy(){
		_confirmButton.interactable = true;
	}
}
