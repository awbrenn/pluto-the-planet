using UnityEngine;
using System.Collections;

public class transitionCameraFlip : MonoBehaviour {
	private cameraController camControl;


	// Use this for initialization
	void Start () {
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		camControl = camera.GetComponent<cameraController> () as cameraController;
	}

	void OnTriggerEnter(Collider player){
//		Debug.Log (player.gameObject.tag + " entered Transition");
		if (player.gameObject.tag == "Player"){
			if (camControl != null) camControl.setTransitionCamera (true);
		}
	}

	void OnTriggerExit(Collider player){
//		Debug.Log (player.gameObject.tag + " exited Transition");
		if (player.gameObject.tag == "Player"){
			if (camControl != null) camControl.setTransitionCamera (false);
		}
	}
}
