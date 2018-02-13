using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobotController : RobotController {

	int _walkAnimHash = Animator.StringToHash("Walk");
	int _jumpAnimHash = Animator.StringToHash("Jump");
	int _attackAnimHash = Animator.StringToHash("Punch");

	public override float Attack (bool isLeft) {
		//TODO: Process the bool value for Left Right
		base._robotAnim.SetTrigger (_attackAnimHash);
		return base._attackDuration;
	}

	public override float Jump(bool isUp) {
		//TODO: Process the bool value for Up Down
		base._robotAnim.SetTrigger (_jumpAnimHash);
		return base._jumpDuration;
	}

	public override float Move(bool isLeft) {
		base._isMoving = isLeft;
		//TODO: Process the bool value for Left Right and remove above line
		base._robotAnim.SetBool (_walkAnimHash, isLeft);
		return base._moveDuration;
	}

	public override float Rest() {
		return base._restDuration;
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
		return waitDuration;
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
	}
}
