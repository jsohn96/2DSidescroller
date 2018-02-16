using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {
	protected float _range = 3f; //range of attack
	protected float _movementDistance = 3f; //per turn
	protected bool _isMoving = false;

	protected bool _isFacingRight = true;

	protected int _walkAnimHash = Animator.StringToHash("Walk");
	protected int _jumpAnimHash = Animator.StringToHash("Jump");
	protected int _attackAnimHash = Animator.StringToHash("Punch");

	protected int _currentHorizontalIndex;
	public int CurrentHorizontalIndex {
		get{ return _currentHorizontalIndex; }
	}
	protected int _currentVerticalIndex;
	public int CurrentVerticalIndex {
		get{ return _currentVerticalIndex; }
	}

	[SerializeField] protected Animator _robotAnim;

	[SerializeField] protected float _attackDuration, _jumpDuration, _moveDuration, _restDuration;

	[Header("Animation Curves for Lerping Movement")]
	[SerializeField] AnimationCurve _walkMovement;
	[SerializeField] AnimationCurve _jumpUpCurve;
	[SerializeField] AnimationCurve _jumpDownCurve;
	[SerializeField] protected AnimationCurve _failedMovement;

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


	//Call when robot is facing the incorrect direction
	public IEnumerator TurnAround(){
		AudioManager._audioManagerInstance.PlayTurnAround ();
		Vector3 currentEuler = transform.eulerAngles;
		Vector3 goalEuler = currentEuler;
		goalEuler.y -= 180f;
		float timer = 0f;
		float duration = 0.5f;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.eulerAngles = Vector3.Lerp (currentEuler, goalEuler, timer / duration);
			yield return null;
		}
		_isFacingRight = !_isFacingRight;
		transform.eulerAngles = goalEuler;
		yield return null;
	}

	public IEnumerator LerpMove(Vector3 startPos, Vector3 goalPos, float duration, bool isWalk, bool isUp = false){
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			if (isWalk) {
				transform.position = Vector3.Lerp (startPos, goalPos, _walkMovement.Evaluate(timer / duration));
			} else {
				if (isUp) {
					transform.position = Vector3.Lerp (startPos, goalPos, _jumpUpCurve.Evaluate (timer / duration));
				} else {
					transform.position = Vector3.Lerp (startPos, goalPos, _jumpDownCurve.Evaluate(timer / duration));
				}
			}
			yield return null;
		}
		transform.position = goalPos;
		yield return null;
	}
}
