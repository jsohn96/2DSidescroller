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

	[SerializeField] Transform _handArea;
		
	GameObject _placeholder = null;

	float _screenWidth, _screenHeight;
	float _canvasScalarWidth = 1920f, _canvasScalarHeight = 1080f;

	//Variables for Possible Unity 2017, non adjusting
	HorizontalLayoutGroup _horizontalLayoutGroup;

	Color _fullColor;
	Color _emptyColor;

	void Start(){
		_thisRectTransform = GetComponent<RectTransform> ();
		_screenWidth = Screen.width;
		_screenHeight = Screen.height;
		_image = GetComponent<Image> ();

		_fullColor = Color.white;
		_emptyColor = _fullColor;
		_emptyColor.a = 0.0f;
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
		_image.raycastTarget = false;
		_commandHover.enabled = false;
		if (_thisCanvasGroup == null) {
			_thisCanvasGroup = GetComponent<CanvasGroup> ();
		}
		_thisCanvasGroup.interactable = false;
		_removedFromPlay = true;
		StartCoroutine (FadeCommandAway ());
	}

	public void Activate(Sprite sprite, PlayerMoveSet playerMoveSet){
		_image.sprite = sprite;
		_whichMoveSet = playerMoveSet;
		_commandHover.enabled = true;
		if (_thisCanvasGroup == null) {
			_thisCanvasGroup = GetComponent<CanvasGroup> ();
		}
		_thisCanvasGroup.interactable = true;
		_image.raycastTarget = true;
		_image.color = _fullColor;
		transform.SetParent (_handArea);
		_removedFromPlay = false;
	}

	public void ReturnToHand(){
		transform.SetParent (_handArea);
		StartCoroutine (UnityBugWorkAround ());
	}

	IEnumerator UnityBugWorkAround(){
		yield return null;
		//TODO: Temporaray solution to Unity 2017 dynamic Layout Adjustment Bug
		_horizontalLayoutGroup = _handArea.GetComponent<HorizontalLayoutGroup> ();
		_horizontalLayoutGroup.enabled = false;
		_horizontalLayoutGroup.enabled = true;
		// End of Temporary solution
	}


	IEnumerator FadeCommandAway(){
		float timer = 0f;
		float duration = 1f;
		while (timer < duration) {
			timer += Time.deltaTime;
			_image.color = Color.Lerp (_fullColor, _emptyColor, timer / duration);
			yield return null;
		}
		_image.color = _emptyColor;
		yield return null;
	}
}


