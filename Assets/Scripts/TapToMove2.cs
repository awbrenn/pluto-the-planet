using UnityEngine;
using System.Collections;


public class TapToMove2 : MonoBehaviour {

	// public member variables
	public float acceleration = 1.0f;

	// private member variables
	private Camera main_camera;

	// Use this for initialization
	void Start () {
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
	}


	Vector3 getNormalizedTrajectory() {
		Vector3 tap_location_screen_coord = new Vector3(Input.mousePosition.x, Input.mousePosition.y, main_camera.nearClipPlane);

		Vector3 tap_location_world_coord = main_camera.ScreenToWorldPoint(tap_location_screen_coord);
		tap_location_world_coord.y = 0.0f;

		Vector3 pluto_location = transform.position;

		Vector3 trajectory = (tap_location_world_coord - pluto_location).normalized;

		return trajectory;
	}


	void acceleratePlutoInTrajectoryDirection (Vector3 trajectory) {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity += trajectory*acceleration;
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Ray ray = main_camera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit_info;
			bool hit;

			hit = Physics.Raycast (ray, out hit_info);

			// if you didn't hit anything or you didn't hit pluto
			if (!hit || hit_info.transform.name != "Pluto") {
				Vector3 trajectory = getNormalizedTrajectory ();

				acceleratePlutoInTrajectoryDirection (trajectory);
			}
		}
	}
}
