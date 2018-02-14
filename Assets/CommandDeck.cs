using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDeck : MonoBehaviour {
	int _commandCardsInPlay = 0;
	public int CommandCardsInPlay {
		get{ return _commandCardsInPlay; }
		set { _commandCardsInPlay = value; 
			_numberText.text = _leftOverCardString [5 - _commandCardsInPlay];}
	}
	int _totalCardCnt;

	[SerializeField] CommandDrag[] _commandCardPool;
	int _commandCardPoolIterator = 0;

	[SerializeField] Sprite[] _commandSprites = new Sprite[7];

	int _prevDrawnMove = (int)PlayerMoveSet.none;


	[SerializeField] Text _numberText;
	string[] _leftOverCardString = new string[6]{"0", "1","2","3","4","5"};

	void Start(){
		_totalCardCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn + 2;
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
			_numberText.text = _leftOverCardString [5 - _commandCardsInPlay];
		} else {
			if (_commandCardPoolIterator >= 4) {
				_commandCardPoolIterator = 0;
			} else {
				_commandCardPoolIterator++;
			}
			IterateCardPool ();
		}
	}



	// Use recursion to prevent drawing the same commands in a row
	// May need to have weighted values upon testing
	int SelectRandomMoveset(){
		int drawnMove = Random.Range (0, 7);
		if (drawnMove == _prevDrawnMove) {
			return SelectRandomMoveset ();
		} else {
			return drawnMove;
		}
	}
}
