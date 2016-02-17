
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter (Collision other)
	{
		sizeScale contacteeSS = gameObject.GetComponent<sizeScale>();
		sizeScale contactorSS = other.gameObject.GetComponent<sizeScale> ();

		if (contactorSS == null) {
			return;
		}

		float selfSize = contacteeSS.getSize ();
		float otherSize = contactorSS.getSize ();

		if (other.gameObject.tag == "Player") {
			float sizeDifference = otherSize / selfSize;
			if (sizeDifference >= 3.5) {
				Destroy (gameObject);
				contactorSS.addSize (selfSize);
			}
		}
	}
}	