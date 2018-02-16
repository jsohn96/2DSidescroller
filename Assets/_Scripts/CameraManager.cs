using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the camera position and zoom based on current Play State
public class CameraManager : MonoBehaviour {
	[SerializeField] Vector3 _strategyPhaseCameraPos;
	[SerializeField] Vector3 _enemyActionPhaseCameraPos;
	Camera _thisCamera;

	[SerializeField] Transform _playerRobot;
	Vector3 _tempCamPos;

	float _strategyCamSize = 5f;
	float _enemyActionCamSize = 3.7f;
	float _playerActionCamSize = 2f;

	// 0 - strategy Phase, 1 - PlayerAction Phase, 2 - EnemyAction Phase
	int _playPhase = 0;
	bool _transitioning = false;
	bool _initialSizing = false;

	void Start(){
		_thisCamera = GetComponent<Camera> ();
		_tempCamPos = _thisCamera.transform.position;
	}

	void Update(){
		if (_playPhase == 1 && !_transitioning) {
			_thisCamera.transform.position = CalculatePlayerActionCamPosition ();
		}
	}

	// Calculate the goal position for player robot follow cams
	Vector3 CalculatePlayerActionCamPosition(){
		_tempCamPos.x = _playerRobot.position.x;
		_tempCamPos.y = _playerRobot.position.y + 0.8f;
		return _tempCamPos;
	}
		
	public void SwitchCameraPhase(){
		// Prevent the Camera Lerp on first scene load
		if (!_initialSizing) {
			_initialSizing = true;
		} else {
			if (!_transitioning) {
				StartCoroutine (LerpCameraPos ());
			}
		}
	}

	// Lerp Camera Between the three phase position
	IEnumerator LerpCameraPos(){
		_transitioning = true;
		float timer = 0f;
		float duration = 1.2f;
		while (timer < duration) {
			timer += Time.deltaTime;
			if (_playPhase == 0) {
				transform.position = Vector3.Lerp (_strategyPhaseCameraPos, CalculatePlayerActionCamPosition(), timer / duration);
				_thisCamera.orthographicSize = Mathf.Lerp (_strategyCamSize, _playerActionCamSize, timer / duration);
			} else if (_playPhase == 1) {
				transform.position = Vector3.Lerp (_tempCamPos, _enemyActionPhaseCameraPos, timer / duration);
				_thisCamera.orthographicSize = Mathf.Lerp (_playerActionCamSize, _enemyActionCamSize, timer / duration);
			} else {
				transform.position = Vector3.Lerp (_enemyActionPhaseCameraPos, _strategyPhaseCameraPos, timer / duration);
				_thisCamera.orthographicSize = Mathf.Lerp (_enemyActionCamSize, _strategyCamSize, timer / duration);
			}
			yield return null;
		}

		if (_playPhase == 0) {
			transform.position = CalculatePlayerActionCamPosition();
			_thisCamera.orthographicSize = _playerActionCamSize;
		} else if (_playPhase == 1) {
			transform.position = _enemyActionPhaseCameraPos;
			_thisCamera.orthographicSize = _enemyActionCamSize;
		} else {
			transform.position = _strategyPhaseCameraPos;
			_thisCamera.orthographicSize = _strategyCamSize;
		}
		yield return null;
		if (_playPhase == 2) {
			_playPhase = 0;
		} else {
			_playPhase++;
		}
		_transitioning = false;
	}
}
