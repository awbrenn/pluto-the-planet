using UnityEngine;
using System.Collections;


public class tapToMovePluto : MonoBehaviour {

	// public member variables
	public float acceleration = 0.3f;
	public float start_max_speed = 20.0f;
	public float speed_increase_decay_rate = -15.0f;


	// private member variables
	private Camera main_camera;
	private plutoBasicAttack pluto_basic_attack;
	private Rigidbody pluto_rigid_body;
	private float max_speed_scaled;
	private float acceleration_scaled;

	// Use this for initialization
	void Start () {
		pluto_basic_attack = this.GetComponent<plutoBasicAttack>();
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
	
		// kill rigid body rotations.
		pluto_rigid_body = GetComponent<Rigidbody> ();
		pluto_rigid_body.freezeRotation = true;
	}


	Vector3 getNormalizedTrajectory() {
		float distanceFromCamera = Mathf.Abs((transform.position - main_camera.transform.position).magnitude);
		Vector3 tap_location_screen_coord = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);

		Vector3 tap_location_world_coord = main_camera.ScreenToWorldPoint(tap_location_screen_coord);
		tap_location_world_coord.y = 0.0f;

		Vector3 pluto_location = transform.position;

		Vector3 trajectory = (tap_location_world_coord - pluto_location).normalized;

		return trajectory;
	}

	// update pluto's speed by his scale
	void updateSpeed()
	{
		max_speed_scaled = start_max_speed * Mathf.Exp(-gameObject.transform.localScale.x / speed_increase_decay_rate);
		acceleration_scaled = acceleration * Mathf.Exp(-gameObject.transform.localScale.x / speed_increase_decay_rate);
	}

	void acceleratePlutoInTrajectoryDirection (Vector3 trajectory) 
	{		
		updateSpeed ();

		// clamp Pluto's velocity to pluto max speed
		if ((pluto_rigid_body.velocity + trajectory*acceleration_scaled).magnitude <= max_speed_scaled)
			pluto_rigid_body.velocity += trajectory*acceleration_scaled;
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && !pluto_basic_attack.plutoPressed()) {
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
