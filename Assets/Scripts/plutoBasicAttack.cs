using UnityEngine;
using System.Collections;

public class plutoBasicAttack : MonoBehaviour {
	public float projectile_max_speed = 10.0f;
	public float percentageOfPlutosHealth = 0.03f;

	private bool pluto_pressed = false;
	private float speed;
	private Vector3 start_position;
	private Vector3 end_position;
	private GameObject projectile_prefab;
	private float time_pluto_was_pressed;
	private Camera main_camera;

	// Use this for initialization
	void Start () {
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
		projectile_prefab = Resources.Load ("Prefabs/defaultProjectile") as GameObject;
	}


	void spawnProjectile (Vector3 projectile_trajectory, float speed) {
		Vector3 projectile_initial_position;


		// clamp speed to max speed
		if (speed >= projectile_max_speed) {
			speed = projectile_max_speed;
		}

		GameObject projectile = Instantiate (projectile_prefab) as GameObject;

		// setting the projectiles health to percentageOfPlutosHealth that of plutos
		float plutosHealth = GetComponent<objectHealth> ().getHealth ();
		projectile.GetComponent<objectHealth> ().instantiateHealth((int)(plutosHealth * percentageOfPlutosHealth));

		// using scale of Pluto to determine the initial position of projectile 
		projectile_initial_position = projectile_trajectory * (transform.localScale.x/2.0f + projectile.transform.localScale.x/2.0f) + transform.position;
		projectile.transform.position = projectile_initial_position;

		//GameObject newProjectile = (GameObject) Instantiate (projectile);


		Rigidbody projectile_rigid_body = projectile.GetComponent<Rigidbody> ();
		Vector3 pluto_velocity = (GetComponent<Rigidbody> ()).velocity;
		projectile_rigid_body.velocity = (projectile_trajectory * speed) + pluto_velocity;
	
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray camera_ray = main_camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Debug.DrawRay (camera_ray.origin, camera_ray.direction * 10, Color.yellow);

			// Check to see of the user pressed pluto
			if (Physics.Raycast (camera_ray, out hit, Mathf.Infinity, 7) && !pluto_pressed) {
				if (hit.transform.name == "Pluto") {
					start_position = Input.mousePosition;
					pluto_pressed = true;
					time_pluto_was_pressed = Time.time;
				}
			}
		}

		if (Input.GetMouseButtonUp (0) && pluto_pressed) {
			float delta_time = Time.time - time_pluto_was_pressed;
			float speed_scale = 100.0f;

			// if the player didn't swipe, we don't spawn a projectile
			end_position = Input.mousePosition;
			if (end_position == start_position) {
				pluto_pressed = false;
				return;
			}

			// calculate the normalized projectile trajectory
			Vector3 projectile_trajectory = (end_position - start_position).normalized;

			// transform the trajectory from the x-y plane to the x-z plane
			projectile_trajectory.z = projectile_trajectory.y;
			projectile_trajectory.y = 0.0f;

			// get the speed (*note speed_scale is a fudge factor, may need to refactor)
			speed = (float)((end_position - start_position).magnitude/speed_scale) / delta_time;

			spawnProjectile (projectile_trajectory, speed);

			pluto_pressed = false;
		}
	}

	// getters
	public bool plutoPressed() {
		return pluto_pressed;
	}
}
