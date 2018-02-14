using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandHand : MonoBehaviour, IDropHandler {
	public enum Slot {
		Hand = 0,
		Slot1 = 1,
		Slot2 = 2,
		Slot3 = 3
	}

	[SerializeField] Slot _whichSlot;
	PlayerMoveSet _moveSetInSlot = PlayerMoveSet.none;
	public PlayerMoveSet MoveSetInSlot{
		get { return _moveSetInSlot; }
	}

	CommandDrag _occupantCommand = null;

	//Handle drop of command card
	public void OnDrop(PointerEventData eventData) {
		CommandDrag commandDrag = eventData.pointerDrag.GetComponent<CommandDrag> ();
		if (commandDrag != null) {
			//Code specific to handling Slots
			if (_whichSlot != Slot.Hand) {
				if (transform.childCount > 0) {
					_occupantCommand.ReturnToHand ();
				}
				_moveSetInSlot = commandDrag.WhichMoveSet;
				_occupantCommand = commandDrag;
			}
			commandDrag.OriginParent = this.transform;
		}	
	}

	//Check if this slot has a valid command card
	public PlayerMoveSet CheckOccupancy(){
		if (transform.childCount <= 0) {
			_moveSetInSlot = PlayerMoveSet.none;
		}
		return _moveSetInSlot;
	}

	//Return previously placed command card back into hand
	public void ReturnOccupant(){
		if (_occupantCommand != null) {
			_occupantCommand.ReturnToHand ();
		}
	}

	//Remove the confirmed command cards from play
	public void RemoveOccupant(){
		if (_occupantCommand != null) {
			_occupantCommand.RemoveFromPlay ();
		}
	}
}
