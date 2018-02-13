using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {
	protected float _range = 3f; //range of attack
	protected float _movementDistance = 3f; //per turn
	protected bool _isMoving = false;

	[SerializeField] protected Animator _robotAnim;

	[SerializeField] protected float _attackDuration, _jumpDuration, _moveDuration, _restDuration;

	public virtual float Attack(bool isLeft) {
		return _attackDuration;
	}

	public virtual float Jump(bool isUp) {
		return _jumpDuration;
	}

	public virtual float Move(bool isLeft) {
		return _moveDuration;
	}

	public virtual float Rest() {
		return _restDuration;
	}
}
