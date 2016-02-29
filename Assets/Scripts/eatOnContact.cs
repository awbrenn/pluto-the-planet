
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {
	public GameObject[] foodTypes;
	public float healthTransferMultiplier = 1f;
	public float impactDamageMultiplier = 1f;
	public AudioClip hitSound;
	public AudioClip eatSound;

	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
	}

	void OnCollisionEnter (Collision other)
	{
		objectHealth contacteeHealth = gameObject.GetComponent<objectHealth>();
		audioSource = other.gameObject.GetComponent<AudioSource> ();

		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Boss") {
			objectHealth contactorHealth = other.gameObject.GetComponent<objectHealth> ();
			int selfHealth = contacteeHealth.getHealth ();
			int otherHealth = contactorHealth.getHealth ();
			float healthDifference = (float)(selfHealth) / (float)(otherHealth);
			if (healthDifference <= 0.35f) {
				audioSource.PlayOneShot (eatSound, 1f);
				Destroy (gameObject);
				contactorHealth.adjustHealth ((int)((float)selfHealth * healthTransferMultiplier));
			}
			else {
				audioSource.PlayOneShot (hitSound, 1f);
				float impactDamage = (otherHealth + selfHealth) * impactDamageMultiplier;
				print ("impact damage:  " + impactDamage);
				contacteeHealth.adjustHealth ((int)- (impactDamage/2));
				contactorHealth.adjustHealth ((int)- (impactDamage/2));

				print (contactorHealth.getHealth ());
				print (contacteeHealth.getHealth ());
			}
		}
		if (other.gameObject.tag == "projectile"){
			projectileDamage hit = other.gameObject.GetComponent<projectileDamage> ();
			int selfSize = contacteeHealth.getHealth ();
			int hitDamage = (int) (hit.getDamage ());

//			Debug.Log ("projectile damage eoc:  " + hitDamage);

			if (selfSize > hitDamage) {
				int foodHealth;
//				Debug.Log ("hit object health1:  " + contacteeHealth.getHealth() + " hit damage: " + hitDamage);
				int newHealth = contacteeHealth.getHealth () - hitDamage;

				contacteeHealth.instantiateHealth (newHealth);
//				Debug.Log ("hit object health2:  " + contacteeHealth.getHealth() + "new Health var:  " + newHealth);

				Destroy(other.gameObject);
				int chunks = (int)(hitDamage/5);
				// temporary making boss not emit any food
				if (name == "Boss")
					chunks = 0;
				for (int i=0; i < chunks; i++){
					foodHealth = (int) (hitDamage/chunks) + 5; // adding fudge 5 points to spawn foods health

					Vector3 startPoint = other.contacts [0].point;
					Vector3 testPoint = startPoint;
					Vector3 spawnPosition = startPoint;

					GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];

					while (startPoint == spawnPosition) {
						float foodScale = food.transform.localScale.x;

						Vector2 randGen = Random.insideUnitCircle * .5f;
						Vector3 randPoint = new Vector3 (randGen.x, 0, randGen.y);
						testPoint += randPoint;
						bool test = testNewObjectPosition (food, testPoint, (food.transform.localScale.x/2f));  //assuming base size is 100 here

						if (test){
							spawnPosition = testPoint;
						}
					}

					Quaternion spawnRotation = Quaternion.identity;

					GameObject newFood = (GameObject) Instantiate (food, spawnPosition, spawnRotation);
					newFood.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

					Vector3 direction = other.contacts [0].normal;
					Vector3 dirRandomizor = Random.insideUnitSphere * 2f;
					Vector3 velocityRB = direction + dirRandomizor + other.relativeVelocity;

					Rigidbody foodRB = newFood.GetComponent<Rigidbody>();
					foodRB.AddForce (-velocityRB * 8f);
				}

			}
			if (selfSize <= hitDamage) {
				Destroy (gameObject);
				Destroy(other.gameObject);
			}
		}
	}

	public bool testNewObjectPosition(GameObject food, Vector3 checkPosition, float foodScale) {
		bool check = true;
		Collider[] hitColliders;

		hitColliders = Physics.OverlapSphere(checkPosition, foodScale, 7); // the int 7 masks out layer 8 +, which now only holds the boundary spheres

		if (hitColliders.Length > 0) {
			check = false;
		}

		return check;

	}
}	