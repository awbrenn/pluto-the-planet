using UnityEngine;
using System.Collections;

public class bossFoodReturn : MonoBehaviour {
	public float timeBeforeReturn = 10f;
	public float speed = 1f;

	private float spawnTime;
	private Vector3 targetPoint;


	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
		targetPoint = new Vector3(0,0,0); //where the boss's food is headed
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time > (spawnTime + timeBeforeReturn)) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, targetPoint, step);
		}
	}
}
