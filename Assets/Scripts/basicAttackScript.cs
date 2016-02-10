using UnityEngine;
using System.Collections;

public class basicAttackScript : MonoBehaviour {
	public Camera main_camera;
	public float max_speed = (float) 10.0;
	private float speed;
	private bool pluto_pressed = false;
	private Vector3 start_position;
	private Vector3 end_position;
	private GameObject projectile_prefab;
	private SphereCollider pluto_collider;
	private float time_pluto_was_pressed;

	// Use this for initialization
	void Start () {
		projectile_prefab = Resources.Load ("Prefabs/projectile") as GameObject;
		pluto_collider = transform.GetComponent<SphereCollider> () as SphereCollider;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = main_camera.ScreenPointToRay(Input.mousePosition);
			//Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);
			if (Physics.Raycast (ray) && !pluto_pressed) {
				start_position = Input.mousePosition;
				pluto_pressed = true;
				time_pluto_was_pressed = Time.time;
			}
		}

		if (Input.GetMouseButtonUp (0) && pluto_pressed) {
			Vector3 projectile_initial_position;
			SphereCollider projectile_collider;
			float delta_time = Time.time - time_pluto_was_pressed;
			float speed_scale = 100.0f;

			end_position = Input.mousePosition;
			if (end_position == start_position) {
				return;
			}

			Vector3 projectile_trajectory = (end_position - start_position).normalized;
			projectile_trajectory.z = projectile_trajectory.y;
			projectile_trajectory.y = 0.0f;
			speed = (float)((end_position - start_position).magnitude/speed_scale) / delta_time;

			// clamp spped to max speed
			if (speed >= max_speed) {
				speed = max_speed;
			}

			GameObject projectile = Instantiate (projectile_prefab) as GameObject;
			projectile_collider = projectile.transform.GetComponent<SphereCollider> () as SphereCollider;

			projectile_initial_position = projectile_trajectory * (pluto_collider.radius + projectile_collider.radius);
			projectile.transform.position = projectile_initial_position;

			Rigidbody rb = projectile.GetComponent<Rigidbody> ();
			rb.velocity = projectile_trajectory * speed;

			pluto_pressed = false;
		}
	}
}
