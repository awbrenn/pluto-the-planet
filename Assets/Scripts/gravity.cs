using UnityEngine;
using System.Collections;

public class gravity : MonoBehaviour {

	Rigidbody rigidBody;
	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> () as Rigidbody;
	}
	
	// Update is called once per frame
	void Update () {
		float radius;
		float angularVelocity;
		float acceleration;
		Vector3 directionToCenter = (-1.0f) * transform.position.normalized;
		Vector3 tangentVector = new Vector3 ((-1.0f) * directionToCenter.z, 0.0f, directionToCenter.x);
		Vector3 unitVelocityVector = rigidBody.velocity.normalized;
		angularVelocity = Vector3.Dot (tangentVector, unitVelocityVector);


		radius = transform.position.magnitude;
		acceleration = ((angularVelocity * angularVelocity) / radius) * 0.1f;

		rigidBody.velocity += directionToCenter * acceleration;
	}
}
