using UnityEngine;
using System.Collections;

public class bossAttackFlip : MonoBehaviour {
	private gameController gameCont;

	// Use this for initialization
	void Start () {
		GameObject gCHolder = GameObject.FindGameObjectWithTag("GameController");
		gameCont = gCHolder.GetComponent<gameController> () as gameController;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider player){
//		Debug.Log (player.gameObject.tag + " entered Boss V");
		if (player.gameObject.tag == "Player"){
			gameCont.activateBoss (true);
		}
	}

	void OnTriggerExit(Collider player){
//		Debug.Log (player.gameObject.tag + " exited Boss V");
		if (player.gameObject.tag == "Player"){
			gameCont.activateBoss (false);
		}
	}
}
