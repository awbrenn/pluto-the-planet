using UnityEngine;
using System.Collections;


public class tapToMovePluto : MonoBehaviour {

	// public member variables
	public float acceleration = 0.3f;
	public float reverse_acceleration = 4.0f;
	public float start_max_speed = 20.0f;
	public float speed_increase_decay_rate = -15.0f;
	public float sensitivity = 100.0f;


	// private member variables
	private Camera main_camera;
	private plutoBasicAttack pluto_basic_attack;
	private Rigidbody pluto_rigid_body;
	private float max_speed_scaled;
	private float acceleration_scaled;
	private Vector3 start_mouse_position;
	private Vector3 current_mouse_position;
	private float safe_volume_radius;
	private float boss_volume_radius;
	private bool canMove = true;

	// Use this for initialization
	void Start () {
		pluto_basic_attack = this.GetComponent<plutoBasicAttack>();
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
	
		// kill rigid body rotations.
		pluto_rigid_body = GetComponent<Rigidbody> ();
		pluto_rigid_body.freezeRotation = true;
		safe_volume_radius = GameObject.FindGameObjectWithTag ("Safe Volume").transform.localScale.x / 2.0f;
		boss_volume_radius = GameObject.FindGameObjectWithTag ("Boss Volume").transform.localScale.x / 2.0f;
	}


	Vector3 getTrajectory() {
//		float distanceFromCamera = Mathf.Abs((transform.position - main_camera.transform.position).magnitude);
//		Vector3 tap_location_screen_coord = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
//
//		Vector3 tap_location_world_coord = main_camera.ScreenToWorldPoint(tap_location_screen_coord);
//		tap_location_world_coord.y = 0.0f;
//
//		Vector3 pluto_location = transform.position;
//
//		Vector3 trajectory = (tap_location_world_coord - pluto_location).normalized;
//
//		return trajectory;
		Vector3 trajectory = (current_mouse_position - start_mouse_position) / sensitivity;
		Vector3 direction_towards_boss = -1.0f * transform.position.normalized;
		//Vector3 up_direction = new Vector3 (0.0f, 0.0f, 1.0f);

		trajectory.z = trajectory.y;
		trajectory.y = 0.0f;

		if (transform.position.magnitude <= boss_volume_radius) {
			trajectory = Quaternion.Euler(0.0f, main_camera.transform.localRotation.eulerAngles.y, 0.0f) * trajectory;
		}

		return trajectory;
	}

	// update pluto's speed by his scale
	void updateSpeed()
	{
		max_speed_scaled = start_max_speed * Mathf.Exp(-gameObject.transform.localScale.x / speed_increase_decay_rate);
	}

	void acceleratePlutoInTrajectoryDirection (Vector3 trajectory) 
	{
		updateSpeed ();

		Vector3 new_velocity;

		new_velocity = trajectory * max_speed_scaled;

		if (new_velocity.magnitude > max_speed_scaled)
			new_velocity = trajectory.normalized * max_speed_scaled;

		pluto_rigid_body.velocity = new_velocity;
	}

	public void setCanMove (bool newState){
		canMove = newState;
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (canMove) {
			foreach (Touch touch in Input.touches) {
				if (touch.position.x < Screen.width / 2) {
					Debug.Log(touch.position.x + Screen.width / 2);

					switch (touch.phase) {
					case TouchPhase.Began:
						start_mouse_position = touch.position;
						break;
					case TouchPhase.Moved:
						current_mouse_position = touch.position;

						Vector3 trajectory = getTrajectory ();

						//Debug.Log (start_mouse_position + "    " + current_mouse_position + "     " + trajectory);

						if (!pluto_basic_attack.shooting && transform.position.magnitude <= safe_volume_radius)
							acceleratePlutoInTrajectoryDirection (trajectory);
						break;
					}
				}
			}
		}
/*		else if (!canMove) {
			if (pluto_rigid_body.velocity.magnitude > 0) {
				pluto_rigid_body.velocity = Vector3.zero;
			}
		}*/


		//################### controls for PC ######################
		if (canMove && Input.GetMouseButton (0) && Input.touches.Length == 0) {

			// on tap
			if (Input.GetMouseButtonDown (0)) {
				start_mouse_position = Input.mousePosition;
			}

			// on holding down
			if (Input.GetMouseButton (0)) {
				current_mouse_position = Input.mousePosition;

				Vector3 trajectory = getTrajectory ();

				//Debug.Log (start_mouse_position + "    " + current_mouse_position + "     " + trajectory);

				if (!pluto_basic_attack.shooting && transform.position.magnitude <= safe_volume_radius)
					acceleratePlutoInTrajectoryDirection (trajectory);
			}

		}
		else if (!canMove) {
			if (pluto_rigid_body.velocity.magnitude > 0) {
				pluto_rigid_body.velocity = Vector3.zero;
			}
		}
		//########################################################## 
	}
}
