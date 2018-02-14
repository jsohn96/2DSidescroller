using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotController : RobotController {

	//preassigned Enemy Behavior
	[SerializeField] PlayerMoveSet[] _enemyMoveSetArray;

	[SerializeField] Material _material;

	int _thisEnemyMovesetLength = 0;
	int _currentMoveIndex = 0;

	int _outlineColor;
	int _outlineWidth;
	int _outlineNormalExtrusion;
	float _outlineWidthValue = 1.033f;
	float _outlineNormalExtrusionValue = 0.038f;

	void Start(){
		_outlineColor = Shader.PropertyToID ("_OutlineColor");
		_outlineWidth = Shader.PropertyToID ("_OutlineWidth");
		_outlineNormalExtrusion = Shader.PropertyToID ("_OutlineNormalExtrusion");
	}

	public void InitializeEnemy(PlayerMoveSet[] parsedCommands){
		_enemyMoveSetArray = parsedCommands;
		_thisEnemyMovesetLength = _enemyMoveSetArray.Length;
	}

	public override float Attack (bool isLeft) {
		//TODO: Process the bool value for Left Right
		_robotAnim.SetTrigger (_attackAnimHash);
		return _attackDuration;
	}

	public override float Jump(bool isUp) {
		//TODO: Process the bool value for Up Down
		_robotAnim.SetTrigger (_jumpAnimHash);
		return _jumpDuration;
	}

	public override float Move(bool isLeft) {
		base._isMoving = isLeft;
		//TODO: Process the bool value for Left Right and remove above line
		_robotAnim.SetBool (_walkAnimHash, isLeft);
		return _moveDuration;
	}

	public override float Rest() {
		return _restDuration;
	}

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


	void OnMouseEnter(){
		_material.SetFloat (_outlineWidth, _outlineWidthValue);
		_material.SetFloat (_outlineNormalExtrusion, _outlineNormalExtrusionValue);
	}

	void OnMouseDown(){
	
	}

	void OnMouseExit(){
		_material.SetFloat (_outlineWidth, 0.0f);
		_material.SetFloat (_outlineNormalExtrusion, 0.0f);
	}
}
