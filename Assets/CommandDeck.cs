using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDeck : MonoBehaviour {
	[SerializeField] Transform _handArea;

	int _commandCardsInPlay = 0;
	int _totalCardCnt;

	[SerializeField] CommandDrag[] _commandCardPool;

	int _prevDrawnMove = (int)PlayerMoveSet.none;

	void Start(){
		_totalCardCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn + 2;
		_commandCardPool = new CommandDrag[_totalCardCnt];
	}

	//Draw a command card from the deck and place it into the hand
	public void DrawCommand (){
		_prevDrawnMove = SelectRandomMoveset ();
		Debug.Log (_prevDrawnMove);
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
