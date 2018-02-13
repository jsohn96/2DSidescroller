using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	PlayerMoveSet _whichMoveSet = PlayerMoveSet.none;
	public PlayerMoveSet WhichMoveSet {
		get { return _whichMoveSet; }
	}

	bool _removedFromPlay = true;
	public bool RemovedFromPlay {
		get { return _removedFromPlay; }
	}

	[SerializeField] Transform _thisCanvas;
	[SerializeField] CommandHover _commandHover;
	[SerializeField] Image _image;

	RectTransform _thisRectTransform;
	CanvasGroup _thisCanvasGroup = null;

	Transform _originParent = null;
	public Transform OriginParent {
		get { return _originParent;}
		set { if(value is Transform) _originParent = value; }
	}
	Transform _goalParent = null;
	public Transform GoalParent {
		get { return _goalParent;}
		set { if(value is Transform) _goalParent = value; }
	}
		
	GameObject _placeholder = null;

	float _screenWidth, _screenHeight;
	float _canvasScalarWidth = 1920f, _canvasScalarHeight = 1080f;

	void Start(){
		_thisRectTransform = GetComponent<RectTransform> ();
		_screenWidth = Screen.width;
		_screenHeight = Screen.height;
	}

	public void OnBeginDrag(PointerEventData data){
		_originParent = transform.parent;
		transform.SetParent (_thisCanvas);
		if (_thisCanvasGroup == null) {
			_thisCanvasGroup = GetComponent<CanvasGroup> ();
		}
		_thisCanvasGroup.blocksRaycasts = false;
		_commandHover.IsDragging (true);
	}

	public void OnDrag(PointerEventData data){
		Vector2 tempPointerPos = data.position;
		// convert screenspace to canvas space (default anchor at Top, Left)
		tempPointerPos.x = (tempPointerPos.x / _screenWidth) * _canvasScalarWidth;
		tempPointerPos.y = (tempPointerPos.y / _screenHeight) * _canvasScalarHeight - _canvasScalarHeight;

		_thisRectTransform.anchoredPosition = tempPointerPos;
	}

	public void OnEndDrag(PointerEventData data){
		transform.SetParent (_originParent);
		_thisCanvasGroup.blocksRaycasts = true;
		_commandHover.IsDragging (false);
	}

	public void RemoveFromPlay(){
		_image.enabled = false;
		_commandHover.enabled = false;
		if (_thisCanvasGroup == null) {
			_thisCanvasGroup = GetComponent<CanvasGroup> ();
		}
		_thisCanvasGroup.interactable = false;
		_removedFromPlay = true;
	}

}


