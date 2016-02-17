
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {
	public GameObject[] foodTypes;


	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter (Collision other)
	{
		sizeScale contacteeSS = gameObject.GetComponent<sizeScale>();
/*		sizeScale contactorSS = other.gameObject.GetComponent<sizeScale> ();

		if (contactorSS == null) {
			return;
		}

		float selfSize = contacteeSS.getSize ();
		float otherSize = contactorSS.getSize ();*/

		if (other.gameObject.tag == "Player") {
			sizeScale contactorSS = other.gameObject.GetComponent<sizeScale> ();
			float selfSize = contacteeSS.getSize ();
			float otherSize = contactorSS.getSize ();
			float sizeDifference = otherSize / selfSize;
			if (sizeDifference >= 3.5) {
				Destroy (gameObject);
				contactorSS.addSize (selfSize);
			}
		}
		if (other.gameObject.tag == "projectile"){
			projectileDamage hit = other.gameObject.GetComponent<projectileDamage> ();
			float selfSize = contacteeSS.getSize ();
			float hitDamage = hit.getDamage ();

			if (selfSize > hitDamage) {
				contacteeSS.addSize (-hitDamage);
				Destroy(other.gameObject);
				int chunks = (int)(hitDamage/10);

			}
			if (selfSize <= hitDamage) {
				Destroy (gameObject);
				Destroy(other.gameObject);
			}

		}
	}
}	