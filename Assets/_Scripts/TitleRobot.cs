﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unique Controller for the robot in start scene
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

	// Toggle outline glow on
	void OnMouseEnter() {
		_originalMaterials = _robotSkinnedMeshRenderer.materials;
		_robotSkinnedMeshRenderer.materials = _robotMaterials;
		_robotOriginalGreenMaterial = _robotMidGreenMeshRenderer.material;
		_robotMidGreenMeshRenderer.material = _robotMidGreenMaterials;
	}

	// Toggle outline glow off
	void OnMouseExit() {
		_robotSkinnedMeshRenderer.materials = _originalMaterials;
		_robotMidGreenMeshRenderer.material = _robotOriginalGreenMaterial;
	}

	// perform an animation
	void OnMouseDown(){
		int randomNumber = Random.Range (1, 4);
		switch (randomNumber) {
		case 1:
			Move (true);
			break;
		case 2:
			Jump (true);
			break;
		case 3:
			Attack (true);
			break;
		default:
			break;
		}
	}
}
