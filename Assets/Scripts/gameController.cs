using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject[] foodTypes;
	public float outerSpawnDiameter;
	public float innerSpawnDiameter;
	public int foodSpawnCount;
	public float foodInitialAngularVelocity = 2.0f;

	private GameObject outerVolume;
	private GameObject innerVolume;
//	private Bounds outerbound;
//	private Bounds innerbound;

	// Use this for initialization
	void Start () {
		outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
		innerVolume = GameObject.FindGameObjectWithTag("Boss Volume");
		SphereCollider oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		SphereCollider iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;

		for (int i =0; i < foodSpawnCount; i++)
		{
			float max = oV.radius * outerVolume.transform.localScale.x;

			Vector3 spawnPosition = new Vector3 (0,0,0);
			Vector3 centerPosition = new Vector3 (0,0,0);

			int foodHealth = (int)Random.Range (10, 300);

			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<objectHealth> ().instantiateHealth (foodHealth);

			while (spawnPosition == centerPosition) {
				Vector2 randGen = Random.insideUnitCircle * max;
				Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
				bool test = iV.bounds.Contains (testPoint);
				bool test2 = testNewObjectPosition (food, testPoint, foodHealth/food.GetComponent<objectHealth> ().baseHealth); //assuming base size is 100 here

				if (!test)
				{
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
	
	// Update is called once per frame
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
}

