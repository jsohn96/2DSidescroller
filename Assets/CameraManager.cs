using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	[SerializeField] Vector3 _strategyPhaseCameraPos;
	[SerializeField] Vector3 _actionPhaseCameraPos;
	Camera _thisCamera;

	float _strategyCamSize = 5f;
	float _actionCamSize = 3.7f;

	bool _isStrategyPhase = true;

	void Start(){
		_thisCamera = GetComponent<Camera> ();
	}
		
	public void SwitchToActionPhase(){
		if (_isStrategyPhase) {
			StartCoroutine (LerpCameraPos (_strategyPhaseCameraPos, _actionPhaseCameraPos, _strategyCamSize, _actionCamSize));
			_isStrategyPhase = false;
		}
	}

	public void SwitchToStrategyPhase(){
		if (!_isStrategyPhase) {
			StartCoroutine (LerpCameraPos (_actionPhaseCameraPos, _strategyPhaseCameraPos, _actionCamSize, _strategyCamSize));
			_isStrategyPhase = true;
		}
	}

	// Lerp Camera Between the two phase position
	IEnumerator LerpCameraPos(Vector3 origin, Vector3 goal, float originSize, float goalSize){
		float timer = 0f;
		float duration = 1f;
		while (timer < duration) {
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (origin, goal, timer / duration);
			_thisCamera.orthographicSize = Mathf.Lerp (originSize, goalSize, timer / duration);
			yield return null;
		}
		transform.position = goal;
		_thisCamera.orthographicSize = goalSize;
		yield return null;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.T)) {
			SwitchToActionPhase ();
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			SwitchToStrategyPhase ();	
		}
	}
}
