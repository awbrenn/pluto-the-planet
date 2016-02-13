using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject[] foodTypes;
	public float outerSpawnDiameter;
	public float innerSpawnDiameter;
	public int foodSpawnCount;

	private GameObject outerVolume;
	private GameObject innerVolume;
//	private Bounds outerbound;
	private Bounds innerbound;

	// Use this for initialization
	void Start () {
//		Debug.Log ("start function ");
		outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
		innerVolume = GameObject.FindGameObjectWithTag("Boss Volume");
		SphereCollider oV = outerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
		SphereCollider iV = innerVolume.transform.GetComponent<SphereCollider> () as SphereCollider;
//		outerbound = new Bounds ((outerVolume.GetComponent<SphereCollider>).bounds.center, oV.bounds.size);
//		innerbound = new Bounds (iV.bounds.center, iV.bounds.size);

//		outerbound = new Bounds (new Vector3 (0, 0, 0), new Vector3(outerSpawnDiameter, outerSpawnDiameter, outerSpawnDiameter));
		innerbound = new Bounds (new Vector3 (0, 0, 0), new Vector3 (innerSpawnDiameter, innerSpawnDiameter, innerSpawnDiameter));

		for (int i =0; i < foodSpawnCount; i++)
		{
			float max = oV.radius * outerVolume.transform.localScale.x;

			Vector3 spawnPosition = new Vector3 (0,0,0);
			Vector3 centerPosition = new Vector3 (0,0,0);

			float foodSize = Random.Range (10, 100);
			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<sizeScale> ().setSize (foodSize);

			while (spawnPosition == centerPosition) {
//				Debug.Log ("loop is active");
				Vector3 testPoint = Random.insideUnitSphere * max;
				bool test = innerbound.Contains (testPoint);
				bool test2 = iV.bounds.Contains (testPoint);

//				print (test);
//				print (test2);
				
				if (test2 != true && test == test2) 
				{
//					Debug.Log ("setting Spawn to Test");
					spawnPosition = new Vector3 (testPoint.x, 0, testPoint.z);
					print (spawnPosition);
				}
			}

			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (food, spawnPosition, spawnRotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
