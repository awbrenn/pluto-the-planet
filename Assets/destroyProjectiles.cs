using UnityEngine;
using System.Collections;

public class destroyProjectiles : MonoBehaviour {

	private float plutoDistanceFromBoss;
	void Start() {
		plutoDistanceFromBoss = GameObject.FindGameObjectWithTag ("Player").transform.position.magnitude;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "projectile" && plutoDistanceFromBoss > transform.localScale.x / 2.0f ) {
			Destroy (other.gameObject);
		}
	}
		
}
