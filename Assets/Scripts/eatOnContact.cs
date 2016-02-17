
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
				for (int i=0; i < chunks; i++){
					float foodSize = hitDamage/chunks;
					Vector3 direction = other.contacts [0].normal;
					Vector3 startPoint = other.contacts [0].point;

					GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
					food.GetComponent<sizeScale> ().setSize (foodSize);

					Quaternion spawnRotation = Quaternion.identity;

					Instantiate (food, startPoint, spawnRotation);
				}

			}
			if (selfSize <= hitDamage) {
				Destroy (gameObject);
				Destroy(other.gameObject);
			}

		}
	}
}	