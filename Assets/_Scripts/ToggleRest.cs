using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRest : MonoBehaviour {
	TextMesh _textMesh;
	string[] _existentialThoughts = new string[3]{
		"What is my purpose?",
		"There must be a reason",
		"Do I matter?"
	};

	Color _fullColor;
	Color _emptyColor;

	[SerializeField] Transform _followTransform;

	Vector3 _positionDifference;

	void Start(){
		_textMesh = GetComponent<TextMesh> ();
		_fullColor = Color.white;
		_emptyColor = new Color (1f, 1f, 1f, 0f);
		_textMesh.color = _emptyColor;

		//calculate the offset between this transform and follow transform
		_positionDifference = _followTransform.position - transform.position;
	}

	void Update(){
		// Follow the follow transforms position
		transform.position = _followTransform.position - _positionDifference;
	}

	//Toggle the rest text on
	public void BeginContemplatingExistence(){
		_textMesh.text = _existentialThoughts[Random.Range (0, 3)];
		StartCoroutine (RemoveTextInTime (1f));
	}

	//Fade the text out based on a timer
	IEnumerator RemoveTextInTime(float duration){
		_textMesh.color = _fullColor;
		yield return new WaitForSeconds (duration);
		float timer = 0f;
		float fadeDuration = 1f;
		while (timer < fadeDuration) {
			timer += Time.deltaTime;
			_textMesh.color = Color.Lerp (_fullColor, _emptyColor, timer / fadeDuration);
			yield return null;
		}
		_textMesh.color = _emptyColor;
		yield return null;
	}
}
