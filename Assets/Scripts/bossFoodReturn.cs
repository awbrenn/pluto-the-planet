using UnityEngine;
using System.Collections;

public class bossFoodReturn : MonoBehaviour {
	public float timeBeforeReturn = 10f;
	public float speed = 1f;

	private float spawnTime;
	private Vector3 targetPoint;
//	private Rigidbody myRB;


	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
		targetPoint = new Vector3(0,0,0); //where the boss's food is headed
//		myRB = gameObject.GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time > (spawnTime + timeBeforeReturn)) {
			float step = speed * Time.deltaTime;
//			myRB.velocity.Set (0,0,0);
			transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
		}
	}
}
