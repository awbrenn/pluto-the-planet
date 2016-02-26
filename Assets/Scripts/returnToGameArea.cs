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

	// Update is called once per frame
	void Update () {
		float distanceFromCenter = transform.position.magnitude;
		if (distanceFromCenter > outerBoarderVolumeRadius && outerBoarderVolumePrefab != null) {
			Vector3 directionToCenter = (-1.0f) * transform.position.normalized;
			rigidBody.velocity += directionToCenter * acceleration;
		}

		if (distanceFromCenter < innerBoarderVolumeRadius && innerBoarderVolumePrefab != null) {
			Vector3 directionToCenter = transform.position.normalized;
			rigidBody.velocity += directionToCenter * acceleration;
		}
	}
}
