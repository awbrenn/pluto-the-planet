using UnityEngine;
using System.Collections;

public class teleporterActivate : MonoBehaviour {
	public int targetSize = 152;

	objectHealth playerHealthScript;
	private int playerSize;
	teleportToNewLevel teleActivate;


	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		teleActivate = gameObject.GetComponent<teleportToNewLevel> ();
		playerHealthScript = player.GetComponent<objectHealth> ();
		playerSize = playerHealthScript.getHealth ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!teleActivate.checkTeleporterStatus()){
			GameObject[] aliveFood = GameObject.FindGameObjectsWithTag ("food");
			playerSize = playerHealthScript.getHealth ();
			if (playerSize >= targetSize || aliveFood.Length == 0) {
				activateTransporter ();
			}
		}
	}

	void activateTransporter(){
		teleActivate.changeTeleporterStatus (true);
	}
}
