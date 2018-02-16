using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobotController : RobotController {

	// determines if attack is valid before processing effects
	public override float Attack (bool isLeft) {
		if (isLeft) {
			if (_isFacingRight) {
				StartCoroutine (TurnAround ());
			}
			if (_currentHorizontalIndex > 0) {
				HandleAttack (_currentHorizontalIndex - 1, isLeft);
			}
		} else {
			if (!_isFacingRight) {
				StartCoroutine (TurnAround ());
			}
			if (_currentHorizontalIndex < _tileGridData.horizontalTileCnt) {
				HandleAttack (_currentHorizontalIndex, isLeft);
			}
		}
		_robotAnim.SetTrigger (_attackAnimHash);
		return _attackDuration;
	}

	// Handles the effects of attack move
	void HandleAttack(int punchTargetIndex, bool isLeft){
		if (_currentHorizontalIndex < _tileGridData.horizontalTileCnt) {
			Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (punchTargetIndex, _currentVerticalIndex);
			//check if destructible wall is within attack
			if (tile.isWall) {
				StartCoroutine (LevelGenerator._levelGeneratorInstance.BreakWall (punchTargetIndex, _currentVerticalIndex));
				tile.isWall = false;
				LevelGenerator._levelGeneratorInstance.SetTile (punchTargetIndex, _currentVerticalIndex, tile);
			} else if (isLeft) {
				// determine if enemy is within attack
				if (tile.isOccupied) {
					// call to find the correct enemy to destroy
					LevelGenerator._levelGeneratorInstance.DestroyEnemy (punchTargetIndex, _currentVerticalIndex);
					// increment enemy kill count
					GameStateUI._gameStatsInstance.EnemyKills++;
					// remove the enemy from level
					tile.isOccupied = false;
					LevelGenerator._levelGeneratorInstance.SetTile (punchTargetIndex, _currentVerticalIndex, tile);
				}
			} else {
				if (punchTargetIndex + 1 < _tileGridData.horizontalTileCnt) {
					Tile rightTile = LevelGenerator._levelGeneratorInstance.GetTile (punchTargetIndex + 1, _currentVerticalIndex);
					// determine if enemy is within attack
					if (rightTile.isOccupied) {
						LevelGenerator._levelGeneratorInstance.DestroyEnemy (punchTargetIndex + 1, _currentVerticalIndex);
						// increment enemy kill count
						GameStateUI._gameStatsInstance.EnemyKills++;
						// remove the enemy from level
						rightTile.isOccupied = false;
						LevelGenerator._levelGeneratorInstance.SetTile (punchTargetIndex + 1, _currentVerticalIndex, rightTile);
					}
				}
			}
		}
	}

	// determine if the jump is valid
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

	// if the jump is valid, process the effects of the jump
	void HandleJump(int yIndex, bool isUp){
		Tile tile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, yIndex);
		Tile tempCurrentTile = LevelGenerator._levelGeneratorInstance.GetTile (_currentHorizontalIndex, _currentVerticalIndex);
		bool isJump;
		if (!isUp) {
			isJump = tempCurrentTile.isJump;
		} else {
			isJump = tile.isJump;
		}
		if (isJump) {
			Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);
			if (tile.isOccupied) {
				// if enemy is in destination, take damage
				_playerRobotMaterialHandler.TakeDamage ();
				// increment damage taken
				GameStateUI._gameStatsInstance.TimesHit++;
				// Animate player character being pushed back to original position
				StartCoroutine (LerpFailedMovement(transform.position, goalPos, _moveDuration));
			} else {
				// lerp player to goal position
				StartCoroutine (LerpMove (transform.position, goalPos, _jumpDuration, false, isUp));

				//Set the previous tile to reflect player absence
				tempCurrentTile.isPlayer = false;
				LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);
				_priorVerticalIndex = _currentVerticalIndex;
				_priorHorizontalIndex = _currentHorizontalIndex;
				//set the current tile to reflect player presence
				_currentVerticalIndex = yIndex;
				tile.isPlayer = true;
				LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);
			}
			CheckWinCondition ();
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
		_robotAnim.SetTrigger (_walkAnimHash);
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
		if (!isWall) {
			Vector3 goalPos = new Vector3 (tile.x, tile.y + _tileGridData.robotFootingOffset, -1f);
			if (tile.isOccupied) {
				// if goal position has enemy, take damage
				_playerRobotMaterialHandler.TakeDamage ();
				// incrememnt damage counter
				GameStateUI._gameStatsInstance.TimesHit++;
				// animate player push back
				StartCoroutine (LerpFailedMovement(transform.position, goalPos, _moveDuration));
			} else {
				StartCoroutine (LerpMove (transform.position, goalPos, _moveDuration, true));

				//Set the previous tile to reflect player absence
				tempCurrentTile.isPlayer = false;
				LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tempCurrentTile);
				_priorVerticalIndex = _currentVerticalIndex;
				_priorHorizontalIndex = _currentHorizontalIndex;
				//set the current tile to reflect player presence
				_currentHorizontalIndex = xIndex;
				tile.isPlayer = true;
				LevelGenerator._levelGeneratorInstance.SetTile (_currentHorizontalIndex, _currentVerticalIndex, tile);
			}
			CheckWinCondition ();
		}
	}


	public override float Rest() {
		_toggleRest.BeginContemplatingExistence ();
		return _restDuration;
	}

	TileGridData _tileGridData;
	[SerializeField] ToggleRest _toggleRest;
	[SerializeField] PlayerRobotMaterialHandler _playerRobotMaterialHandler;

	int _priorHorizontalIndex = 0;
	int _priorVerticalIndex = 0;
		
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


	// Handle enemy entering into player position and determining resulting player placement
	// Move the player back to their previous position
	public void AttackedByEnemy(){
		_playerRobotMaterialHandler.TakeDamage ();
		GameStateUI._gameStatsInstance.TimesHit++;
		// reference to previous position
		Tile priorTile = LevelGenerator._levelGeneratorInstance.GetTile (_priorHorizontalIndex, _priorVerticalIndex);
		Vector3 goalPos = new Vector3 (priorTile.x, priorTile.y + _tileGridData.robotFootingOffset, -1f);
		StartCoroutine (LerpMove (transform.position, goalPos, _moveDuration, true));

		// update player relocation
		priorTile.isPlayer = true;

		LevelGenerator._levelGeneratorInstance.SetTile (_priorHorizontalIndex, _priorVerticalIndex, priorTile);

		// swap the current and previous position of the character
		int tempH;
		int tempV;
		tempH = _priorHorizontalIndex;
		tempV = _priorVerticalIndex;
		_priorHorizontalIndex = _currentHorizontalIndex;
		_priorVerticalIndex = _currentVerticalIndex;
		_currentHorizontalIndex = tempH;
		_currentVerticalIndex = tempV;
	}

	//Lerp the player robot to simulate being thwarted by an enemy robot
	IEnumerator LerpFailedMovement(Vector3 startPos, Vector3 goalPos, float duration){
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (startPos, goalPos, _failedMovement.Evaluate(timer / duration));
			yield return null;
		}
		transform.position = startPos;
		yield return null;
	}

	// check if player is in the goal spot to determine game end state
	void CheckWinCondition(){
		if (_currentHorizontalIndex == _tileGridData.horizontalTileCnt - 1) {
			if (_currentVerticalIndex == _tileGridData.verticalTileCnt - 1) {
				GameManager._gameManagerInstance.GameCompleted();

			}
		}
	}
}
