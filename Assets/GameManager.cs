using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	enum PlayState {
		Strategize = 0,
		PlayerTurn = 1,
		EnemyTurn = 2

	}
			
	public static GameManager _gameManagerInstance = null;

	public int HowManyMovesPerTurn {
		get {return _howManyMovesPerTurn; }
	}
	[SerializeField] int _howManyMovesPerTurn = 3;

	void Awake(){
		if (_gameManagerInstance == null) {
			_gameManagerInstance = this;
		} else if (_gameManagerInstance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}
}
