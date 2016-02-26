using UnityEngine;
using System.Collections;

public class returnToGameArea : MonoBehaviour {
	public float accelerationTowardsCenter = 1.0f;
	public GameObject boarderVolumePrefab;

	private float boarderVolumeRadius;
	private Rigidbody rigidBody;
	private GameObject boarderVolume;
	// Use this for initialization
	void Start () {
		// get the radius of the growth volume
		boarderVolume = GameObject.FindGameObjectWithTag(boarderVolumePrefab.tag);
		boarderVolumeRadius = boarderVolume.transform.localScale.x / 2.0f;
		rigidBody = GetComponent<Rigidbody> () as Rigidbody;
	}

	// Update is called once per frame
	void Update () {
		float distanceFromCenter = transform.position.magnitude;
		if (distanceFromCenter > boarderVolumeRadius) {
			Vector3 directionToCenter = (-1.0f) * transform.position.normalized;
			rigidBody.velocity += directionToCenter * accelerationTowardsCenter;
		}
	}
}
