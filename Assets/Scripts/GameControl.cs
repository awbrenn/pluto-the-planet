using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	public GameObject[] foodTypes;
	public float outerSpawnDiameter;
	public float innerSpawnDiameter;
	public int foodSpawnCount;



	// Use this for initialization
	void Start () {
		for (int i =0; i < foodSpawnCount; i++)
		{
//			float foodSize = Random.range (10, 100);
//			GameObject food = foodTypes [Random.range (0, foodTypes.length)];
//			food.setSize (foodSize);
//			Vector3 spawnPosition = new Vector3 (Random.Range (-outerSpawnDiameter.x, outerSpawnDiameter.x), 0, Random.Range (-outerSpawnDiameter.z, outerSpawnDiameter.z));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
