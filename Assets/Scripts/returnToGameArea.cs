using UnityEngine;
using System.Collections;

public class returnToGameArea : MonoBehaviour {
	public float acceleration = 1.0f;
	public GameObject innerBoarderVolumePrefab;
	public GameObject outerBoarderVolumePrefab;

	private float outerBoarderVolumeRadius;
	private float innerBoarderVolumeRadius;
	private Rigidbody rigidBody;
	private GameObject outerBoarderVolume;
	private GameObject innerBoarderVolume;
	// Use this for initialization
	void Start () {
		// get the radius of the growth volume
		if (outerBoarderVolumePrefab != null) {
			outerBoarderVolume = GameObject.FindGameObjectWithTag(outerBoarderVolumePrefab.tag);
			outerBoarderVolumeRadius = outerBoarderVolume.transform.localScale.x / 2.0f;
		}
			
		if (innerBoarderVolumePrefab != null) {
			innerBoarderVolume = GameObject.FindGameObjectWithTag(innerBoarderVolumePrefab.tag);
			innerBoarderVolumeRadius = innerBoarderVolume.transform.localScale.x / 2.0f;
		}

		rigidBody = GetComponent<Rigidbody> () as Rigidbody;
	}

	void FixedUpdate () {
		float distanceFromCenter = transform.position.magnitude;
		if (distanceFromCenter > outerBoarderVolumeRadius && outerBoarderVolumePrefab != null) {
			Vector3 directionToCenter = (-1.0f) * transform.position.normalized;
			Vector3 velocity = rigidBody.velocity.normalized;
			float accellerationInfluence = 1.0f - Vector3.Dot(directionToCenter, velocity);
			if (accellerationInfluence <= 1.0f) {
				accellerationInfluence = 0.0f;
			}

			rigidBody.velocity += directionToCenter * acceleration * accellerationInfluence;
		}

		if (distanceFromCenter < innerBoarderVolumeRadius && innerBoarderVolumePrefab != null) {
			Vector3 directionToCenter = transform.position.normalized;
			Vector3 velocity = rigidBody.velocity.normalized;
			float accellerationInfluence = 1.0f - Vector3.Dot(directionToCenter, velocity);
			if (accellerationInfluence <= 1.0f) {
				accellerationInfluence = 0.0f;
			}

			rigidBody.velocity += directionToCenter * acceleration * accellerationInfluence;
		}
	}
}
