using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Moves card on hover to provide feedback on selectable card
public class CommandHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	RectTransform _thisRectTransform;
	float _yOffsetAmount = 30f;
	bool _isDragging = false;
	Vector2 _originPos;

	void Start(){
		_thisRectTransform = GetComponent<RectTransform> ();
	}

	public void OnPointerEnter(PointerEventData eventData){
		if (!_isDragging) {
			_originPos = _thisRectTransform.anchoredPosition;
			Vector2 tempAnchoredPos = _originPos;
			tempAnchoredPos.y += _yOffsetAmount;
			_thisRectTransform.anchoredPosition = tempAnchoredPos;
		}
	}

	public void OnPointerExit(PointerEventData eventData){
		_thisRectTransform.anchoredPosition = _originPos;
	}

	public void IsDragging(bool isDragging){
		_isDragging = isDragging;
	}
}