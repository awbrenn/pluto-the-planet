using UnityEngine;
using System.Collections;

public class disableBFCollider : MonoBehaviour {
	public float colliderOffTime = .1f;

	private float spawnTime;
	private Collider myCollider;
	private Rigidbody myRigidBody;

	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
		myCollider = gameObject.GetComponent<SphereCollider>();
		myRigidBody = gameObject.GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
}
