
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

		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Boss") {
			sizeScale contactorSS = other.gameObject.GetComponent<sizeScale> ();
			float selfSize = contacteeSS.getSize ();
			float otherSize = contactorSS.getSize ();
			float sizeDifference = otherSize / selfSize;
			if (sizeDifference >= 3.5) {
				Destroy (gameObject);
				contactorSS.addSize (selfSize);
			} 
			else {
				float impactDamage = (otherSize + selfSize) * .1f;
				print ("impact damage:  " + impactDamage);
				contacteeSS.addSize (-(impactDamage/2));
				contactorSS.addSize (-(impactDamage/2));

				print (contactorSS.getSize ());
				print (contacteeSS.getSize ());
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

					Vector3 startPoint = other.contacts [0].point;
					Vector3 testPoint = startPoint;
					Vector3 spawnPosition = startPoint;

					GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
					food.GetComponent<sizeScale> ().setSize (foodSize);

					while (startPoint == spawnPosition) {
						Vector2 randGen = Random.insideUnitCircle * (foodSize * .01f);
						Vector3 randPoint = new Vector3 (randGen.x, 0, randGen.y);
						testPoint += randPoint;
						bool test = testNewObjectPosition (food, testPoint, foodSize/100);  //assuming base size is 100 here

						if (test){
							spawnPosition = testPoint;
						}
					}

					Quaternion spawnRotation = Quaternion.identity;

					GameObject newFood = (GameObject) Instantiate (food, spawnPosition, spawnRotation);

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