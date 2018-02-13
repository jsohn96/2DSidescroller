using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

	public void OnDrop(PointerEventData eventData) {
		CommandDrag commandDrag = eventData.pointerDrag.GetComponent<CommandDrag>();
		if(commandDrag != null) {
			commandDrag.OriginParent = this.transform;
			_moveSetInSlot = commandDrag.WhichMoveSet;
		}

	}
}
