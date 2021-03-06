using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject[] foodTypes;
	public int foodSpawnCount= 100;
//	public int foodSpawnPodCount = 10;
//	public int foodPerPod = 10;
	public float foodInitialAngularVelocity = 2.8f;

	public int minFoodSize = 10;
	public int maxFoodSize = 300;

	public AudioClip attackSound;

	private GameObject outerVolume;
	private GameObject innerVolume;

	private GameObject pluto;
	private SphereCollider iV;

	private GameObject boss;
	private GameObject attack;
	private GameObject hazard;
	private Animator bossAnimator;
	private spawnFood spFood;
	private AudioSource bossSource;

	private bool inBossVolume;

	// Use this for initialization
	void Start () {
//		Debug.Log ("start function ");
		GameObject gC = GameObject.FindGameObjectWithTag("GameController");
		spFood = gC.GetComponent<spawnFood> () as spawnFood;

		outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
		innerVolume = GameObject.FindGameObjectWithTag("Transition Volume");
		iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		pluto = GameObject.FindGameObjectWithTag ("Player");
		boss = GameObject.FindGameObjectWithTag ("Boss");
		bossSource = boss.GetComponent<AudioSource>();

		attack = boss.GetComponent<bossBehavior> ().attackType;
		hazard = boss.GetComponent<bossBehavior> ().hazardType;
		bossAnimator = boss.GetComponent<bossBehavior> ().findBossAnimator ();

		SphereCollider oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		float max = oV.radius * outerVolume.transform.localScale.x;

		placePluto();

		InvokeRepeating ("hazardTimer", 3f, hazard.GetComponent<attackBehavior>().coolDown);
		InvokeRepeating ("attackTimer", 1f, attack.GetComponent<attackBehavior>().coolDown);

/*		for (int i = 0; i < foodSpawnPodCount; i++){
			spFood.spawnFoodPod (foodPerPod, 3f, innerVolume, outerVolume, minFoodSize, maxFoodSize, foodInitialAngularVelocity, foodTypes);
		}*/

		for (int i =0; i < foodSpawnCount; i++) {
			Vector3 spawnPosition = new Vector3 (0,0,0);
			Vector3 centerPosition = new Vector3 (0,0,0);

			int foodHealth = (int)Random.Range (10, 300);

			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

			while (spawnPosition == centerPosition) {
//				Debug.Log ("loop is active");
				Vector2 randGen = Random.insideUnitCircle * max;
				Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
				bool test = iV.bounds.Contains (testPoint);
				bool test2 = testNewObjectPosition (food, testPoint, foodHealth/food.GetComponent<objectHealth> ().baseHealth); //assuming base size is 100 here

				if (!test)
				{
//					Debug.Log ("setting Spawn to Test");
					if (test2){
						spawnPosition = new Vector3 (testPoint.x, 0, testPoint.z);
					}
				}
			}

			Quaternion spawnRotation = Random.rotation;
			GameObject actualFood= (GameObject) Instantiate (food, spawnPosition, spawnRotation);
			actualFood.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

			Rigidbody rb = actualFood.GetComponent<Rigidbody> () as Rigidbody;
			rb.velocity = calculateAngularVelocity (actualFood);
//			actualFood.GetComponent<Animator> ().SetTime (((double)(Random.Range (0.0f, 1.0f))));
		}
	}

	Vector3 calculateAngularVelocity(GameObject food) {
		Vector3 directionToCenter = (-1.0f) * food.transform.position.normalized;
		Vector3 perpendicularVector = new Vector3 ((-1.0f) * directionToCenter.z, 0.0f, directionToCenter.x);

		return perpendicularVector * foodInitialAngularVelocity;
	}

	void Update () {
//		float plutoHealth = pluto.GetComponent<objectHealth> ().getHealth ();
//
//		if (plutoHealth <= 0.0f) {
//			UnityEngine.SceneManagement.SceneManager.LoadScene ("youLost");
//		} else 
		if (GameObject.FindWithTag("Boss") == null) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("youWon");
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

	void placePluto (){
//		Debug.Log ("pluto is placed");
		Vector3 spawnPosition = new Vector3 (0,0,0);
		Vector3 centerPosition = new Vector3 (0,0,0);
		SphereCollider growthVolumeCollider = GameObject.Find ("GrowthVolume").GetComponent<SphereCollider> () as SphereCollider;
		float max_radius = GameObject.Find ("SafeVolume").transform.localScale.x / 2.0f;


		while (spawnPosition == centerPosition) {
			Vector2 randGen = Random.insideUnitCircle * max_radius;
			Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
			bool test = growthVolumeCollider.bounds.Contains (testPoint);

			if (!test){
				spawnPosition = testPoint;
			}
		}

		pluto.transform.position = spawnPosition;
	}

	void hazardTimer (){
		Vector3 plutoLoc = pluto.transform.position;
		if (inBossVolume){
			Vector3 startPoint = new Vector3 (0, 0, 0);
			Quaternion spawnRotation = Quaternion.identity;
			bossAnimator.SetTrigger ("hazard");
			int numberOfClouds = Random.Range (4, 8);
			for (int i = 0; i < numberOfClouds; i ++){
				Instantiate (hazard, startPoint, spawnRotation);
			}
		}
	}

	void attackTimer () {
		Vector3 plutoLoc = pluto.transform.position;
		if (inBossVolume){
			Vector3 startPoint = new Vector3 (0, 0, 0);
			Quaternion spawnRotation = Quaternion.identity;
			bossAnimator.SetTrigger ("attack");
			Instantiate (attack, startPoint, spawnRotation);
			bossSource.PlayOneShot (attackSound, .7f);
		}
	}

	public void activateBoss(bool inOrOut){
		inBossVolume = inOrOut;
	}
}

