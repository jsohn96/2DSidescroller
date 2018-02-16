using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Toggles Strategy UI elements on and off based on current playstate
public class TogglePlayerStrategyUI : MonoBehaviour {
	[SerializeField] float _uiTransitionDuration = 0.8f;

	[SerializeField] AnimationCurve _smoothTransitionCurve;

	[Header("Bottom UI Variables")]
	[SerializeField] RectTransform _bottomUIRectTransform;
	[SerializeField] Vector2 _bottomOnPos, _bottomOffPos;

	[Header("Right UI Variables")]
	[SerializeField] RectTransform _rightUIRectTransform;
	[SerializeField] Vector2 _rightOnPos, _rightOffPos;

	bool _uiToggledOn = false;
	IEnumerator _uiStrategyCoroutine;


	void Start(){
		if (_uiToggledOn) {
			_rightUIRectTransform.anchoredPosition = _rightOnPos;
			_bottomUIRectTransform.anchoredPosition = _bottomOnPos;
		} else {
			_rightUIRectTransform.anchoredPosition = _rightOffPos;
			_bottomUIRectTransform.anchoredPosition = _bottomOffPos;
		}
	}

	public void ToggleStrategyPhaseOn(bool on) {
		_uiToggledOn = on;
		if (_uiStrategyCoroutine != null) {
			StopCoroutine (_uiStrategyCoroutine);
		}
		_uiStrategyCoroutine = PhaseUIOn (_uiToggledOn);
		StartCoroutine (_uiStrategyCoroutine);
	}

	IEnumerator PhaseUIOn (bool on){
		Vector2 bottomOriginPos = _bottomUIRectTransform.anchoredPosition;
		Vector2 rightOriginPos = _rightUIRectTransform.anchoredPosition;
		Vector2 bottomGoalPos;
		Vector2 rightGoalPos;
		if (on) {
			bottomGoalPos = _bottomOnPos;
			rightGoalPos = _rightOnPos;
		} else {
			bottomGoalPos = _bottomOffPos;
			rightGoalPos = _rightOffPos;
		}

		float timer = 0f;
		while (timer < _uiTransitionDuration) {
			timer += Time.deltaTime;
			_bottomUIRectTransform.anchoredPosition = Vector2.Lerp (bottomOriginPos, bottomGoalPos, 
				_smoothTransitionCurve.Evaluate(timer / _uiTransitionDuration));
			_rightUIRectTransform.anchoredPosition = Vector2.Lerp (rightOriginPos, rightGoalPos, 
				_smoothTransitionCurve.Evaluate(timer / _uiTransitionDuration));
			yield return null;
		}
		_bottomUIRectTransform.anchoredPosition = bottomGoalPos;
		_rightUIRectTransform.anchoredPosition = rightGoalPos;
		yield return null;
	}
}
