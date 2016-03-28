using UnityEngine;
using System.Collections;

public class boundaryFXVisibility : MonoBehaviour {

	public GameObject[] hideFX;

	void OnTriggerEnter(Collider player) {
		if (player.gameObject.tag == "Player") {
			for (int i = 0; i < hideFX.Length; ++i) {
				hideFX [i].GetComponent<Renderer> ().enabled = false;
			}
		}
	}

	void OnTriggerExit(Collider player) {
		if (player.gameObject.tag == "Player") {
			for (int i = 0; i < hideFX.Length; ++i) {
				hideFX [i].GetComponent<Renderer> ().enabled = true;
			}
		}
	}
}
