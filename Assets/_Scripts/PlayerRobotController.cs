using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobotController : RobotController {

	public override float Attack (bool isLeft) {
		//TODO: Process the bool value for Left Right
		_robotAnim.SetTrigger (_attackAnimHash);
		return _attackDuration;
	}

	public override float Jump(bool isUp) {
		int goalYIndex;
		if (isUp) {
			goalYIndex = _currentVerticalIndex + 1;
			if (goalYIndex < _tileGridData.verticalTileCnt) {
				HandleJump (goalYIndex, isUp);
			}
		} else {
			goalYIndex = _currentVerticalIndex - 1;
			if (goalYIndex >= 0) {
				HandleJump (goalYIndex, isUp);
			}
		}
		_robotAnim.SetTrigger (_jumpAnimHash);
		return _jumpDuration;
	}

	void HandleJump(int yIndex, bool isUp){
		Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, yIndex);
		Tile tempCurrentTile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, _currentVerticalIndex);
		bool isJump;
		if (!isUp) {
			isJump = tempCurrentTile.isJump;
		} else {
			isJump = tile.isJump;
		}
		if (!isJump) {
			//Handle Running into a wall
//			Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);
//			StartCoroutine (LerpMovementFail (transform.position, goalPos, _moveDuration, true));
			_robotAnim.SetTrigger (_jumpAnimHash);
		} else if (tile.isOccupied) {
			// show being attacked
			//  Move to place?
		} else {
			Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);
			StartCoroutine (LerpMove (transform.position, goalPos, _jumpDuration, false, isUp));
			_robotAnim.SetTrigger (_jumpAnimHash);

			//Set the previous tile to reflect player absence
			tempCurrentTile.isPlayer = false;
			LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);

			//set the current tile to reflect player presence
			_currentVerticalIndex = yIndex;
			tile.isPlayer = true;
			LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);
		}
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
		return _moveDuration;
	}

	void HandleMovement(int xIndex, bool isLeft){
		Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (xIndex, _currentVerticalIndex);
		Tile tempCurrentTile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, _currentVerticalIndex);
		bool isWall;
		if (!isLeft) {
			isWall = tempCurrentTile.isWall;
		} else {
			isWall = tile.isWall;
		}
		if (isWall) {
			//Handle Running into a wall
			_robotAnim.SetTrigger (_walkAnimHash);
		} else if (tile.isOccupied) {
			// show being attacked
			//  Move to place
		} else {
			Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);
			StartCoroutine (LerpMove (transform.position, goalPos, _moveDuration, true));
			_robotAnim.SetTrigger (_walkAnimHash);

			//Set the previous tile to reflect player absence
			tempCurrentTile.isPlayer = false;
			LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);

			//set the current tile to reflect player presence
			_currentHorizontalIndex = xIndex;
			tile.isPlayer = true;
			LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);
		}
	}


	public override float Rest() {
		return _restDuration;
	}

	TileGridData _tileGridData;
		
	void Start(){
		//Initialize Position to 0,0
		_currentHorizontalIndex = 0;
		_currentVerticalIndex = 0;

		// keep a reference to level spacing data
		_tileGridData = LevelGenerator._levelGeneratorInstance.GetTileGridData();
	}

	public float HandlePlayerCommand(PlayerMoveSet playerMoveSet){
		float waitDuration = 0f;

		switch (playerMoveSet) {
		case PlayerMoveSet.AttackLeft:
			waitDuration = Attack (true);
			break;
		case PlayerMoveSet.AttackRight:
			waitDuration = Attack (false);
			break;
		case PlayerMoveSet.MoveLeft:
			waitDuration = Move (true);
			break;
		case PlayerMoveSet.MoveRight:
			waitDuration = Move (false);
			break;
		case PlayerMoveSet.JumpUp:
			waitDuration = Jump (true);
			break;
		case PlayerMoveSet.JumpDown:
			waitDuration = Jump (false);
			break;
		case PlayerMoveSet.Rest:
			waitDuration = Rest ();
			break;
		default:
			break;
		}
		return waitDuration + 0.5f;
	}



	void Update(){
		if (Input.GetKeyDown (KeyCode.A)) {
			Move (true);
		} else if (Input.GetKeyUp(KeyCode.A)) {
			Move (false);
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			Jump (true);
		}
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			Attack (true);
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			StartCoroutine (TurnAround ());
		}
	}
}
