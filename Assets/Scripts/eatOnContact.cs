
using UnityEngine;
using System.Collections;

public class eatOnContact : MonoBehaviour {
	public GameObject[] foodTypes;
	public float healthTransferMultiplier = 1f;
	public float impactDamageMultiplier = 1f;

	public AudioClip hitSound;
	public AudioClip eatSound;
	public AudioClip takeDamageSound;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
	}

	void playerEatsProjectile (objectHealth pluto, GameObject projectile){
		objectHealth projectileHealth = projectile.GetComponent<objectHealth> ();
		audioSource.PlayOneShot (eatSound, 1f);
		Destroy (projectile);
		pluto.adjustHealth (projectileHealth.getHealth ());
	}

	void playerOrBossEat (objectHealth eaterHealth, GameObject toBeEaten){
		objectHealth toBeEatenHealth = toBeEaten.GetComponent<objectHealth> ();
		int selfHealth = eaterHealth.getHealth ();
		int otherHealth = toBeEatenHealth.getHealth ();

		float healthDifference = (float)(selfHealth) / (float)(otherHealth);

		if (healthDifference <= 0.35f || ((float)(selfHealth)) <= 10f) {
			if (audioSource != null){
				audioSource.PlayOneShot (eatSound, 1f);
				toBeEaten.GetComponent<ParticleSystem> ().Play ();
			}
			Destroy (gameObject);
			toBeEatenHealth.adjustHealth ((int)((float)selfHealth * healthTransferMultiplier));

			if (toBeEaten.tag == "Player") {
				toBeEaten.GetComponent<Animator> ().SetTrigger ("triggerChomp");
			}
		}
		else {
			audioSource.PlayOneShot (hitSound, 1f);
			float impactDamage = (otherHealth + selfHealth) * impactDamageMultiplier;

			if (toBeEaten.name == "Pluto" && otherHealth - impactDamage <= 0) { 
				UnityEngine.SceneManagement.SceneManager.LoadScene ("youLost");
			}

			eaterHealth.adjustHealth ((int)- (impactDamage/2));
			toBeEatenHealth.adjustHealth ((int)- (impactDamage/2));

//			print (eaterHealth.getHealth ());
//			print (toBeEatenHealth.getHealth ());
		}
	}

	void projectileDoesDamage (objectHealth target, GameObject projectile, Vector3 impactPoint, Vector3 impactNormal, Vector3 impactRelVelocity){
//		Debug.Log ("projectile made contact with " + gameObject.tag);

		projectileDamage hit = projectile.GetComponent<projectileDamage> ();
		int selfSize = target.getHealth ();
		int hitDamage = (int)(hit.getDamage ());

//		Debug.Log ("projectile damage eoc:  " + hitDamage);

		if (selfSize > hitDamage) {
			int foodHealth;
//			Debug.Log ("hit object health1:  " + contacteeHealth.getHealth() + " hit damage: " + hitDamage);
			int newHealth = target.getHealth () - hitDamage;

			target.instantiateHealth (newHealth);
//			Debug.Log ("hit object health2:  " + contacteeHealth.getHealth() + "new Health var:  " + newHealth);

			Destroy (projectile);
			int chunks = (int)(hitDamage / 5);
			// making boss not emit any food
			// making boss ANIMATE taking hit!!
			if (name == "Boss") {
				chunks = 0;
				Animator [] animArray = target.GetComponentsInChildren<Animator> ();
				Animator animActivator = animArray [0];
				animActivator.SetTrigger ("takeDamage"); 

				audioSource = target.GetComponent<AudioSource> ();
				audioSource.PlayOneShot (hitSound, 1f);
			}


			if (target.CompareTag ("food")) {
//				Debug.Log ("Projectile is trying to look shocked");
				Animator [] animArray = target.GetComponentsInChildren<Animator> ();
				Animator animActivator = animArray [0];
				animActivator.SetTrigger ("takeDamage"); 

				audioSource = target.GetComponent<AudioSource> ();
				audioSource.PlayOneShot (takeDamageSound, .7f);
			}  //make food animate

			for (int i = 0; i < chunks; i++) {
				foodHealth = (int)(hitDamage / chunks) + 5; // adding fudge 5 points to spawn foods health

//				Vector3 startPoint = other.contacts [0].point;
				Vector3 testPoint = impactPoint;
				Vector3 spawnPosition = impactPoint;

				GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];

				while (impactPoint == spawnPosition) {
					Vector2 randGen = Random.insideUnitCircle * 2f;
					Vector3 randPoint = new Vector3 (randGen.x, 0, randGen.y);
					testPoint += randPoint;
//					Debug.Log (testPoint);
					bool test = testNewObjectPosition (food, testPoint, (food.transform.localScale.x / 2f));  //assuming base size is 100 here

					if (test) {
						spawnPosition = testPoint;
					}
				}

				Quaternion spawnRotation = Random.rotation;

				GameObject newFood = (GameObject)Instantiate (food, spawnPosition, spawnRotation);
				newFood.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

//				Vector3 direction = other.contacts [0].normal;
				Vector3 dirRandomizor = Random.insideUnitSphere * 2f;
				Vector3 velocityRB = impactNormal + dirRandomizor + impactRelVelocity;

				Rigidbody foodRB = newFood.GetComponent<Rigidbody> ();
				foodRB.AddForce (-velocityRB * 8f);
			}

		}
		if (selfSize <= hitDamage) {
			Destroy (gameObject);
			Destroy (projectile);
		}
	}

	void OnCollisionEnter (Collision other)
	{
		objectHealth contacteeHealth = gameObject.GetComponent<objectHealth>();
		audioSource = other.gameObject.GetComponent<AudioSource> ();

		if (gameObject.tag == "Player" && other.gameObject.tag == "projectile") {
			playerEatsProjectile (contacteeHealth, other.gameObject);
		} 

		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Boss") {
			if (other.gameObject.tag == "Player") {
				maximumSize maxSize = other.gameObject.GetComponent<maximumSize> ();
				objectHealth plutoHealth = other.gameObject.GetComponent<objectHealth>();
				int currentMax = maxSize.getMaxHealth ();
				int currentHealth = plutoHealth.getHealth ();
//				Debug.Log ("pluto size: " + currentHealth);
				if (currentHealth < currentMax) {
					playerOrBossEat (contacteeHealth, other.gameObject);
					//other.gameObject.GetComponent<Animator> ().SetTrigger ("triggerHungry");
					other.gameObject.GetComponent<Animator> ().SetBool ("isHungry", true);
				} 
				else {
					other.gameObject.GetComponent<Animator> ().SetBool ("isHungry", false);
				}
			} 
			else {
//				Debug.Log ("boss is trying to eat");
				playerOrBossEat (contacteeHealth, other.gameObject);
				//other.gameObject.GetComponent<Animator> ().SetTrigger ("triggerFull");
			}
		}
		if (other.gameObject.tag == "projectile" && gameObject.tag != "Player"){
			projectileDoesDamage (contacteeHealth, other.gameObject, other.contacts [0].point, other.contacts [0].normal, other.relativeVelocity);
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