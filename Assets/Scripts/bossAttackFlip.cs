using UnityEngine;
using System.Collections;

public class bossAttackFlip : MonoBehaviour {
	private gameController gameCont;
	private cameraController camControl;


	// Use this for initialization
	void Start () {
		GameObject gCHolder = GameObject.FindGameObjectWithTag("GameController");
		if (gCHolder != null) gameCont = gCHolder.GetComponent<gameController> () as gameController;
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		camControl = camera.GetComponent<cameraController> () as cameraController;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider player){
//		Debug.Log (player.gameObject.tag + " entered Boss V");
		if (player.gameObject.tag == "Player"){
			if (gameCont != null) {
				gameCont.activateBoss (true);
				camControl.setBossLook (true);
			}
		}
	}

	void OnTriggerExit(Collider player){
//		Debug.Log (player.gameObject.tag + " exited Boss V");
		if (player.gameObject.tag == "Player"){
			if (gameCont != null) {
				gameCont.activateBoss (false);
				camControl.setBossLook (false);
			}
		}
	}
}
