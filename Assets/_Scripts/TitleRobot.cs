using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRobot : RobotController {
	[Space(10)]
	[Header("Robot Outline Material References")]
	[Space(5)]
	[SerializeField] SkinnedMeshRenderer _robotSkinnedMeshRenderer;
	Material[] _originalMaterials;
	[SerializeField] Material[] _robotMaterials;
	[SerializeField] SkinnedMeshRenderer _robotMidGreenMeshRenderer;
	Material _robotOriginalGreenMaterial;
	[SerializeField] Material _robotMidGreenMaterials;

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

	void OnMouseEnter() {
		_originalMaterials = _robotSkinnedMeshRenderer.materials;
		_robotSkinnedMeshRenderer.materials = _robotMaterials;
		_robotOriginalGreenMaterial = _robotMidGreenMeshRenderer.material;
		_robotMidGreenMeshRenderer.material = _robotMidGreenMaterials;
	}

	void OnMouseExit() {
		_robotSkinnedMeshRenderer.materials = _originalMaterials;
		_robotMidGreenMeshRenderer.material = _robotOriginalGreenMaterial;
	}

	void OnMouseDown(){
		int randomNumber = Random.Range (1, 5);
		switch (randomNumber) {
		case 1:
			Move (true);
			break;
		case 2:
			Move (false);
			break;
		case 3:
			Jump (true);
			break;
		case 4:
			Attack (true);
			break;
		default:
			break;
		}
	}
}
