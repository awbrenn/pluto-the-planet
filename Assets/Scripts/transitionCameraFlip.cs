using UnityEngine;
using System.Collections;

public class transitionCameraFlip : MonoBehaviour {
	public GameObject[] messages;
	private cameraController camControl;

	private bool oneSizeWarning = false;
	private chatter chatterBox;
	private objectHealth objectHealth;

	// Use this for initialization
	void Start () {
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		camControl = camera.GetComponent<cameraController> () as cameraController;
		chatterBox = GameObject.FindGameObjectWithTag ("chatterBox").GetComponent<chatter> ();

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		objectHealth = player.GetComponent <objectHealth> ();
	}

	void OnTriggerEnter(Collider player){
//		Debug.Log (player.gameObject.tag + " entered Transition");
		if (player.gameObject.tag == "Player"){
			int pHealth = objectHealth.getHealth ();
			if (!oneSizeWarning && pHealth <= 450) {
				oneSizeWarning = true;
				if (messages != null) {
					foreach (GameObject message in messages) {
						chatterBox.queueTextMessage (message);
					}
				}
			}
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
