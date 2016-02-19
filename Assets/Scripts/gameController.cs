using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject[] foodTypes;
	public int foodSpawnCount;

	public GameObject[] hazardTypes;

	private GameObject outerVolume;
	private GameObject innerVolume;
	private float max;

//	public Canvas uI;

//	private GUIText gameOverText;
	private bool gameOver;
	private bool restart;

//	private class testLocation tl ();

	// Use this for initialization
	void Start () {
		outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
		innerVolume = GameObject.FindGameObjectWithTag("Boss Volume");
		SphereCollider oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		SphereCollider iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		max = oV.radius * outerVolume.transform.localScale.x;

		GameObject pluto = GameObject.FindGameObjectWithTag("Player");
		Vector3 plutoStartPoint = (growthAreaPoint(max, iV));

		pluto.transform.localPosition = plutoStartPoint;

		spawnFood (foodSpawnCount, max, oV, iV);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	public void spawnFood(int spawnCount, float maxSize, SphereCollider oV, SphereCollider iV){
		
		for (int i =0; i < spawnCount; i++)
		{
			Vector3 spawnPosition = new Vector3 (0,0,0);
			Vector3 centerPosition = new Vector3 (0,0,0);

			float foodSize = Random.Range (10, 100);
			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<sizeScale> ().setSize (foodSize);

			while (spawnPosition == centerPosition) {
				Vector3 testPoint = growthAreaPoint (maxSize, iV);
				bool test2 = testNewObjectPosition (food, testPoint, foodSize/100); //assuming base size is 100 here

				if (test2){
					spawnPosition = new Vector3 (testPoint.x, 0, testPoint.z);
					}
			}

			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (food, spawnPosition, spawnRotation);
		}

	}

	public Vector3 growthAreaPoint (float maxSize, SphereCollider iV){
		Vector3 newPoint = new Vector3 (0, 0, 0);
		Vector3 centerPosition = new Vector3 (0, 0, 0);

		while (newPoint == centerPosition) {
			Vector2 randGen = Random.insideUnitCircle * maxSize;
			Vector3 testPoint = new Vector3 (randGen.x, 0, randGen.y);
			bool test = iV.bounds.Contains (testPoint);
			if (!test){
				newPoint = testPoint;
			}
		}
		return newPoint;
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

