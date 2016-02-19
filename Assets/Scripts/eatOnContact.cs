
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {
	public GameObject[] foodTypes;


	// Use this for initialization
	void Start () {
	
	}

	void OnCollisionEnter (Collision other)
	{
		objectHealth contacteeHealth = gameObject.GetComponent<objectHealth>();

		if (other.gameObject.tag == "Player") {
			objectHealth contactorHealth = other.gameObject.GetComponent<objectHealth> ();
			int selfHealth = contacteeHealth.getHealth ();
			int otherHealth = contactorHealth.getHealth ();
			float healthDifference = otherHealth / selfHealth;
			if (healthDifference >= 0.35) {
				Destroy (gameObject);
				contactorHealth.adjustHealth (selfHealth);
			}
		}
		if (other.gameObject.tag == "projectile"){
			projectileDamage hit = other.gameObject.GetComponent<projectileDamage> ();
			int selfSize = contacteeHealth.getHealth ();
			int hitDamage = (int) (hit.getDamage ());

			if (selfSize > hitDamage) {
				contacteeHealth.adjustHealth (-hitDamage);
				Destroy(other.gameObject);
				int chunks = (int)(hitDamage/10);
				for (int i=0; i < chunks; i++){
					int foodHealth = (int) (hitDamage/chunks);
					Vector3 direction = other.contacts [0].normal;
					Vector3 startPoint = other.contacts [0].point;

					GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
					food.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

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