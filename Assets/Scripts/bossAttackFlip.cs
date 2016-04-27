using UnityEngine;
using System.Collections;

public class bossAttackFlip : MonoBehaviour {
	public GameObject[] greetings;

	private gameController gameCont;
	private cameraController camControl;

	private bool oneGreeting = false;
	private chatter chatterBox;
	private objectHealth objectHealth;

	// Use this for initialization
	void Start () {
		GameObject gCHolder = GameObject.FindGameObjectWithTag("GameController");
		if (gCHolder != null) gameCont = gCHolder.GetComponent<gameController> () as gameController;
		GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		camControl = camera.GetComponent<cameraController> () as cameraController;

		chatterBox = GameObject.FindGameObjectWithTag ("chatterBox").GetComponent<chatter> ();
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		objectHealth = player.GetComponent <objectHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider player){
//		Debug.Log (player.gameObject.tag + " entered Boss V");
		if (player.gameObject.tag == "Player"){
			int pHealth = objectHealth.getHealth ();
			if (!oneGreeting && pHealth >= 450) {
				oneGreeting = true;
				if (greetings != null) {
					foreach (GameObject message in greetings) {
						chatterBox.queueTextMessage (message);
					}
				}
			}

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
