
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		sizeScale contactee = gameObject.GetComponent<sizeScale>();
		sizeScale contactor = other.gameObject.GetComponent<sizeScale> ();

		if (contactor == null) {
			return;
		}

		float selfSize = contactee.getSize ();
		float otherSize = contactor.getSize ();

		if (other.tag == "Player") {
			float sizeDifference = otherSize / selfSize;
			if (sizeDifference >= 3.5) {
				Destroy (gameObject);
				contactor.addSize (selfSize);
			}
		}
	}

}	