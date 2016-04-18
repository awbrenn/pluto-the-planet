using UnityEngine;
using System.Collections;

public class bossControlSwitch : MonoBehaviour {
	public float plutoPauseTime = 1.0f;

	private float pausedStartTime;
	private tapToMovePluto tapToMove;

	// Use this for initialization
	void Start () {
		tapToMove = GameObject.FindGameObjectWithTag ("Player").GetComponent<tapToMovePluto> ();
	}

	void Update () {
		if (Time.time - pausedStartTime > plutoPauseTime) {
			tapToMove.pauseControl = false;
		}
	}

	void OnTriggerEnter(Collider player){
		if (Time.timeSinceLevelLoad > 1.0f) {
			tapToMove.pauseControl = true;
			tapToMove.rotateTrajectory = true;
			pausedStartTime = Time.time;
		}
	}

	void OnTriggerExit(Collider player){		
		tapToMove.pauseControl = true;
		tapToMove.rotateTrajectory = false;
		pausedStartTime = Time.time;
	}
}
