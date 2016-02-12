using UnityEngine;
using System.Collections;


public class TapToMove2 : MonoBehaviour {

	// public member variables
	public float acceleration = 1.0f;
	public float pluto_max_speed = 20.0f;

	// private member variables
	private Camera main_camera;
	private basicAttackScript basic_attack_script;
	private Rigidbody pluto_rigid_body;

	// Use this for initialization
	void Start () {
		basic_attack_script = this.GetComponent<basicAttackScript>();
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
	
		// kill rigid body rotations.
		pluto_rigid_body = GetComponent<Rigidbody> ();
		pluto_rigid_body.freezeRotation = true;
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
		// clamp Pluto's velocity to pluto max speed
		if ((pluto_rigid_body.velocity + trajectory*acceleration).magnitude <= pluto_max_speed)
			pluto_rigid_body.velocity += trajectory*acceleration;
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && !basic_attack_script.plutoPressed()) {
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
