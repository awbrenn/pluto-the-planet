using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	public GameObject[] foodTypes;
	public float outerSpawnDiameter;
	public float innerSpawnDiameter;
	public int foodSpawnCount;

	private GameObject outerVolume = GameObject.FindGameObjectWithTag("Growth Volume");
	private GameObject innerVolume = GameObject.FindGameObjectWithTag("Boss Volume");
//	private SphereCollider outerBoundary = outerVolume.GetComponent("Sphere Collider");

//	private Bounds outerbound = outerBoundary.bounds; 



	// Use this for initialization
	void Start () {
		for (int i =0; i < foodSpawnCount; i++)
		{
			Vector3 spawnPosition = (Vector3.zero);
			Vector3 centerPosition = (Vector3.zero);
			Bounds outerbound = new Bounds (Vector3.zero, new Vector3(outerSpawnDiameter, outerSpawnDiameter, outerSpawnDiameter));

			float foodSize = Random.Range (10, 100);
			GameObject food = foodTypes [Random.Range (0, foodTypes.Length)];
			food.GetComponent<sizeScale> ().setSize (foodSize);

			while (spawnPosition == centerPosition) 
			{
				Vector3 testSpawnPosition = new Vector3 
					(Random.Range (-outerSpawnDiameter, outerSpawnDiameter), 0, Random.Range (-outerSpawnDiameter, outerSpawnDiameter));
				if (outerbound.Contains (testSpawnPosition)) {
					spawnPosition = testSpawnPosition;
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
