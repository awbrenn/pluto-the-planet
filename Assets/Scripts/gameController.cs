using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject[] foodTypes;
	public int foodSpawnCount;
	public float foodInitialAngularVelocity = 2.8f;

	private GameObject outerVolume;
	private GameObject innerVolume;

	private GameObject pluto;
	private GameObject max;
	private SphereCollider iV;

	private GameObject boss;
	private GameObject attack;
	private GameObject hazard;


	// Use this for initialization
	void Start () {
//		Debug.Log ("start function ");
		outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
		innerVolume = GameObject.FindGameObjectWithTag("Boss Volume");
		iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		pluto = GameObject.FindGameObjectWithTag ("Player");
		boss = GameObject.FindGameObjectWithTag ("Boss");

		attack = boss.GetComponent<bossBehavior> ().attackType;
		hazard = boss.GetComponent<bossBehavior> ().hazardType;

		SphereCollider oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		float max = oV.radius * outerVolume.transform.localScale.x;

		Vector3 startPoint = placePluto(max, iV);
		pluto.transform.position = startPoint;

		InvokeRepeating ("hazardTimer", 1f, hazard.GetComponent<attackBehavior>().coolDown);
		InvokeRepeating ("attackTimer", 1f, attack.GetComponent<attackBehavior>().coolDown);


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

			Quaternion spawnRotation = Quaternion.identity;
			GameObject actualFood= (GameObject) Instantiate (food, spawnPosition, spawnRotation);
			actualFood.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

			Rigidbody rb = actualFood.GetComponent<Rigidbody> () as Rigidbody;
			rb.velocity = calculateAngularVelocity (actualFood);
		}
	}

	Vector3 calculateAngularVelocity(GameObject food) {
		Vector3 directionToCenter = (-1.0f) * food.transform.position.normalized;
		Vector3 perpendicularVector = new Vector3 ((-1.0f) * directionToCenter.z, 0.0f, directionToCenter.x);

		return perpendicularVector * foodInitialAngularVelocity;
	}

	void Update () {
	
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

	public Vector3 placePluto (float max, SphereCollider iV){
		Vector3 spawnPosition = new Vector3 (0,0,0);
		Vector3 centerPosition = new Vector3 (0,0,0);

		while (spawnPosition == centerPosition) {
			Vector2 randGen = Random.insideUnitCircle * max;
			Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
			bool test = iV.bounds.Contains (testPoint);

			if (!test){
				spawnPosition = testPoint;
			}
		}

		return spawnPosition;
	}

	void hazardTimer (){
		Vector3 plutoLoc = pluto.transform.position;
		if (iV.bounds.Contains (plutoLoc)) {
			Vector3 startPoint = new Vector3 (0, 0, 0);
			Quaternion spawnRotation = Quaternion.identity;

			Instantiate (hazard, startPoint, spawnRotation);
		}
	}

	void attackTimer () {
		Vector3 plutoLoc = pluto.transform.position;
		if (iV.bounds.Contains (plutoLoc)) {
			Vector3 startPoint = new Vector3 (0, 0, 0);
			Quaternion spawnRotation = Quaternion.identity;

			Instantiate (attack, startPoint, spawnRotation);
		}
	}
}

