using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDeck : MonoBehaviour {
	int _commandCardsInPlay = 0;
	public int CommandCardsInPlay {
		get{ return _commandCardsInPlay; }
		set { _commandCardsInPlay = value; 
			_numberText.text = _leftOverCardString [7 - _commandCardsInPlay];}
	}
	int _totalCardCnt;

	[SerializeField] CommandDrag[] _commandCardPool;
	int _commandCardPoolIterator = 0;

	[SerializeField] Sprite[] _commandSprites = new Sprite[7];

	int _prevDrawnMove = (int)PlayerMoveSet.none;


	[SerializeField] Text _numberText;
	string[] _leftOverCardString = new string[8]{"0", "1","2","3","4","5","6","7"};

	void Start(){
		_totalCardCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn + 4;
	}

	//Draw a command card from the deck and place it into the hand
	public void DrawCommand (){
		if (_commandCardsInPlay < _totalCardCnt) {
			_prevDrawnMove = SelectRandomMoveset ();
			IterateCardPool ();
		}
	}

	//Find an available Command Object from pool and activate it in the hand
	void IterateCardPool(){
		if (_commandCardPool [_commandCardPoolIterator].RemovedFromPlay) {
			_commandCardPool [_commandCardPoolIterator].Activate (
				_commandSprites [_prevDrawnMove], 
				(PlayerMoveSet)_prevDrawnMove);
			_commandCardsInPlay++;
			_numberText.text = _leftOverCardString [7 - _commandCardsInPlay];
		} else {
			if (_commandCardPoolIterator >= 6) {
				_commandCardPoolIterator = 0;
			} else {
				_commandCardPoolIterator++;
			}
			IterateCardPool ();
		}
	}


	//Randomly drawn card with some weighted value
	// Use recursion to prevent drawing the same commands in a row
	//not repeating cards 0, 2, 5, 6
	int SelectRandomMoveset(){
		int drawnCard;
		int randomNumber = Random.Range (0, 100);
		if (randomNumber < 10) {
			drawnCard = 0;
		} else if (randomNumber < 25) {
			drawnCard = 1;
		} else if (randomNumber < 35) {
			drawnCard = 2;
		} else if (randomNumber < 69) {
			drawnCard = 3;
		} else if (randomNumber < 90) {
			drawnCard = 4;
		} else if (randomNumber < 98) {
			drawnCard = 5;
		} else {
			drawnCard = 6;
		}

		if (drawnCard == _prevDrawnMove) {
			if (drawnCard == 0 || drawnCard == 2 || drawnCard == 5 || drawnCard == 6) {
				return SelectRandomMoveset ();
			} else {
				return drawnCard;
			}
		} else {
			return drawnCard;
		}
	}
}
