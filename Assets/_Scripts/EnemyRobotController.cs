using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotController : RobotController {

	//preassigned Enemy Behavior
	TileGridData _tileGridData;
	[SerializeField] PlayerMoveSet[] _enemyMoveSetArray;
	PlayerRobotController _playerRobotController;

	int _thisEnemyMovesetLength = 0;
	int _currentMoveIndex = 0;
	RobotEnemyMaterialHandler _robotEnemyMaterialHandler;

	void Start(){
		// keep a reference to level spacing data
		_tileGridData = LevelGenerator._levelGeneratorInstance.GetTileGridData();

		//Enemy Robots Default to Facing Left
		_isFacingRight = !_isFacingRight;

		_robotEnemyMaterialHandler = GetComponent<RobotEnemyMaterialHandler> ();

		_playerRobotController = GameObject.Find ("PlayerRobot").GetComponent <PlayerRobotController>();
	}

	//Called by Level Generator to Ininitialize the Enemy Robot
	public void InitializeEnemy(int posX, int posY, PlayerMoveSet[] parsedCommands){
		_currentHorizontalIndex = posX;
		_currentVerticalIndex = posY;
		_enemyMoveSetArray = parsedCommands;
		_thisEnemyMovesetLength = _enemyMoveSetArray.Length;
	}

	public override float Jump(bool isUp) {
		int goalYIndex;
		if (isUp) {
			goalYIndex = _currentVerticalIndex + 1;
			HandleJump (goalYIndex, isUp);
		} else {
			goalYIndex = _currentVerticalIndex - 1;
			HandleJump (goalYIndex, isUp);
		}
		_robotAnim.SetTrigger (_jumpAnimHash);
		return _jumpDuration;
	}

	// Called after determining index of position to jump to
	void HandleJump(int yIndex, bool isUp){
		Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, yIndex);
		Tile tempCurrentTile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, _currentVerticalIndex);
		Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);

		//If the player is in goal position, do damage to the player
		if (tile.isPlayer) {
			_playerRobotController.AttackedByEnemy ();
			tile.isPlayer = false;
		}

		//Movement of the character to goal position
		StartCoroutine (LerpMove (transform.position, goalPos, _jumpDuration, false, isUp));

		//Set the previous tile to reflect player absence
		tempCurrentTile.isOccupied = false;
		LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);

		//set the current tile to reflect player presence
		_currentVerticalIndex = yIndex;
		tile.isOccupied = true;
		LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);
	}

	public override float Move(bool isLeft) {
		int goalXIndex;
		if (isLeft) {
			if (_isFacingRight) {
				StartCoroutine (TurnAround ());
			}
			// Check if the left space exists (is within bounds)
			goalXIndex = _currentHorizontalIndex - 1;
			if (goalXIndex >= 0) {
				HandleMovement (goalXIndex, isLeft);

			}
		} else {
			if (!_isFacingRight) {
				StartCoroutine (TurnAround ());
			}
			// Check if the right space exists (is within bounds)
			goalXIndex = _currentHorizontalIndex + 1;
			if (goalXIndex < _tileGridData.horizontalTileCnt) {
				HandleMovement (goalXIndex, isLeft);
			}
		}
		_robotAnim.SetTrigger (_walkAnimHash);
		return _moveDuration;
	}

	// Called if movement destination is a valid position
	void HandleMovement(int xIndex, bool isLeft){
		Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (xIndex, _currentVerticalIndex);
		Tile tempCurrentTile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, _currentVerticalIndex);

		Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);

		// If the player is in the goal position, do damage
		if (tile.isPlayer) {
			_playerRobotController.AttackedByEnemy ();
			tile.isPlayer = false;
		}

		// Move the character to the goal position
		StartCoroutine (LerpMove (transform.position, goalPos, _moveDuration, true));

		//Set the previous tile to reflect player absence
		tempCurrentTile.isOccupied = false;
		LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);

		//set the current tile to reflect player presence
		_currentHorizontalIndex = xIndex;
		tile.isOccupied = true;
		LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);

	}

	// Called through level generator, by PlayerRobotController do animate the robot destruction
	public void DestroyEnemyRobot(){
		StartCoroutine(_robotEnemyMaterialHandler.DestroyEnemy ());
	}

	// Iteration of the enemy moveset to determine next action
	void HandleEnemyMove(){
		PlayerMoveSet tempMoveSet = _enemyMoveSetArray [_currentMoveIndex];

		switch (tempMoveSet) {
		case PlayerMoveSet.AttackLeft:
			Attack (true);
			break;
		case PlayerMoveSet.AttackRight:
			Attack (false);
			break;
		case PlayerMoveSet.MoveLeft:
			Move (true);
			break;
		case PlayerMoveSet.MoveRight:
			Move (false);
			break;
		case PlayerMoveSet.JumpUp:
			Jump (true);
			break;
		case PlayerMoveSet.JumpDown:
			Jump (false);
			break;
		case PlayerMoveSet.Rest:
			Rest ();
			break;
		default:
			break;
		}

		_currentMoveIndex++;
		if (_currentMoveIndex >= _thisEnemyMovesetLength) {
			_currentMoveIndex = 0;
		}
	}

	void OnEnable(){
		EnemyTurnManager.OnEnemyMove += HandleEnemyMove;
	}

	void OnDisable(){
		EnemyTurnManager.OnEnemyMove -= HandleEnemyMove;
	}
}
