using UnityEngine;
using System.Collections;

public class asteroidLookUp : MonoBehaviour {
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 lookDirection = transform.position;
		Vector3 upDirection = transform.localPosition;

		lookDirection.y += 20.0f;

		transform.LookAt (lookDirection, transform.up);
	}
}
