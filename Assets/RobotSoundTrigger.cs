using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSoundTrigger : MonoBehaviour {
	public void PlayPunch(){
		AudioManager._audioManagerInstance.PlayPunch ();
	}

	public void PlayStep(){
		AudioManager._audioManagerInstance.PlayWalk ();
	}
}
