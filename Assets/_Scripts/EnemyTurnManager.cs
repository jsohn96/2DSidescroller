using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the enemy turn phase by delegating enemy movements
public class EnemyTurnManager : MonoBehaviour {
	public delegate void EnemyMove();
	public static event EnemyMove OnEnemyMove;

	int _totalMoveCnt;
	int _currentMoveCnt = 0;

	//preset overestimate of duration to wait 
	//account for all enemy movement durations
	float _longestWaitDuration = 2.2f;

	void Start(){
		_totalMoveCnt = GameManager._gameManagerInstance.HowManyMovesPerTurn;
	}

	//Send Event to all enemies to make next move
	void MakeEnemyMove(){
		// Send Event
		OnEnemyMove ();

		//Increment Move Index
		_currentMoveCnt++;
		StartCoroutine (WaitForNextMove ());
	}

	// Delay between each movement
	IEnumerator WaitForNextMove(){
		yield return new WaitForSeconds (_longestWaitDuration);
		if (_currentMoveCnt < _totalMoveCnt) {
			MakeEnemyMove ();
		} else {
			EndEnemyTurn ();
		}
	}

	void EndEnemyTurn(){
		_currentMoveCnt = 0;
		GameManager._gameManagerInstance.MoveToNextState ();
	}

	public void BeginEnemyTurn(){
		MakeEnemyMove();
	}
}
