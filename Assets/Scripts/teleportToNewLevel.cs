using UnityEngine;
using System.Collections;

public class teleportToNewLevel : MonoBehaviour {
	public bool readyToGo = true;
	public int sceneToLoad = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update(){
		if (readyToGo) {
			gameObject.GetComponent<ParticleSystem> ().Play ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (readyToGo) {
			if (other.tag == "Player") {
				UnityEngine.SceneManagement.SceneManager.LoadScene (sceneToLoad);
			}
		}
	}

	public void changeTeleporterStatus (bool newStatus){
		readyToGo = newStatus;
	}

	public bool checkTeleporterStatus (){
		return readyToGo;
	}
}
