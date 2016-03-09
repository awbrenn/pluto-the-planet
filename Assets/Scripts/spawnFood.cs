using UnityEngine;
using System.Collections;

public class spawnFood : MonoBehaviour {
	private float foodInitialAngularVelocity;
	private GameObject outerVolume;
	private GameObject innerVolume;
	private SphereCollider iV;
	private SphereCollider oV;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void spawnFoodPod (int numberToSpawn, float spawnCircleRadius, GameObject innerV, GameObject outerV, int minHealth, int maxHealth, float foodAngularVelocity, GameObject[] foodTypes){
		outerVolume = outerV;
		innerVolume = innerV;
		iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		float max = oV.radius * outerVolume.transform.localScale.x;

		foodInitialAngularVelocity = foodAngularVelocity;

		Vector3 spawnPodCenter = findRandomPointInFoodBelt ();

		for (int i = 0; i < numberToSpawn; i++){
			Vector3 spawnPosition = new Vector3 (0,0,0);
			Vector3 centerPosition = new Vector3 (0,0,0);

			int foodHealth = (int)Random.Range (minHealth, maxHealth);

			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

			while (spawnPosition == centerPosition) {
//				Debug.Log ("loop is active");
				Vector2 randGen = Random.insideUnitCircle * spawnCircleRadius;
				Vector3 randPoint = new Vector3 (randGen.x, 0, randGen.y);
				Vector3 testPoint = randPoint + spawnPodCenter;

				bool test1 = iV.bounds.Contains (testPoint);
				bool test2 = oV.bounds.Contains (testPoint);
				bool test3 = testNewObjectPosition (food, testPoint, foodHealth/food.GetComponent<objectHealth> ().baseHealth); //assuming base size is 100 here

				if (!test1){
					if (!test2){
//						Debug.Log ("setting Spawn to Test");
						if (test3){
							spawnPosition = new Vector3 (testPoint.x, 0, testPoint.z);
						}
					}
				}
			}

			Quaternion spawnRotation = Random.rotation;
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

	Vector3 findRandomPointInFoodBelt (){
		float max = oV.radius * outerVolume.transform.localScale.x;

		Vector3 randPosition = new Vector3 (0,0,0);
		Vector3 centerPosition = new Vector3 (0,0,0);

		while (randPosition == centerPosition) {
			Vector2 randGen = Random.insideUnitCircle * max;
			Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
			bool test = iV.bounds.Contains (testPoint);
			if (!test) {
				randPosition = testPoint;
			}
		}

		return randPosition;
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
