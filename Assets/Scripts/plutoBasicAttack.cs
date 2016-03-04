using UnityEngine;
using System.Collections;

public class plutoBasicAttack : MonoBehaviour {
	public float projectile_max_speed = 10.0f;
	public float projectileMinSpeed = 2f;
	public float percentageOfPlutosHealthForHealth = 0.03f;
	public float percentageOfPlutosHealthForDamage = .1f;
	public int minDamage = 10;
	public AudioClip shootSound;

	private AudioSource source;
	private float volLowRange = 0.5f;
	private float volHighRange = 1.0f;
	private bool pluto_pressed = false;
	private float speed;
	private Vector3 start_position;
	private Vector3 end_position;
	private GameObject projectile_prefab;
	private float time_pluto_was_pressed;
	private Camera main_camera;
	private float bossVolumeRadius;

	// Use this for initialization
	void Start () {
		main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;
		projectile_prefab = Resources.Load ("Prefabs/defaultProjectile") as GameObject;
		bossVolumeRadius = GameObject.Find ("BossVolume").transform.localScale.x/2.0f;

		source = GetComponent<AudioSource> ();
	}


	void spawnProjectile (Vector3 projectile_trajectory, float speed, bool addPlutoVelocity) {
		Vector3 projectile_initial_position;


		// clamp speed to max speed
		if (speed >= projectile_max_speed) {
			speed = projectile_max_speed;
		}

		else if (speed < projectileMinSpeed){
			speed = projectileMinSpeed;
		}

		GameObject projectile = Instantiate (projectile_prefab) as GameObject;

		// setting the projectiles health to percentageOfPlutosHealth that of plutos
		float plutosHealth = GetComponent<objectHealth> ().getHealth ();
		projectile.GetComponent<objectHealth> ().instantiateHealth((int)(plutosHealth * percentageOfPlutosHealthForHealth));
		float projectileDamage = plutosHealth * percentageOfPlutosHealthForDamage;

		if (projectileDamage < minDamage) {
			projectileDamage = minDamage;
		}
			

		projectile.GetComponent<projectileDamage> ().setDamage (projectileDamage);

//		Debug.Log ("projectile damage in pba:  " + projectile.GetComponent<projectileDamage> ().getDamage());

		// using scale of Pluto to determine the initial position of projectile 
		projectile_initial_position = projectile_trajectory * ((transform.localScale.x/2.0f + projectile.transform.localScale.x/2.0f) * 1.25f) + transform.position;
		projectile.transform.position = projectile_initial_position;


		Rigidbody projectile_rigid_body = projectile.GetComponent<Rigidbody> () as Rigidbody;
		Vector3 pluto_velocity = (GetComponent<Rigidbody> ()).velocity;
		if (addPlutoVelocity) {
			projectile_rigid_body.velocity = (projectile_trajectory * speed + pluto_velocity);
		} else {
			projectile_rigid_body.velocity = (projectile_trajectory * speed);
		}
			
		gameObject.GetComponent<objectHealth> ().adjustHealth ((int)((-1.0f) * plutosHealth * percentageOfPlutosHealthForDamage));
		Debug.Log ("pluto health loss on projectile spawn: " + (int)((-1.0f) * plutosHealth * percentageOfPlutosHealthForDamage));

		source.PlayOneShot (shootSound, 1f);
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray camera_ray = main_camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			//Debug.DrawRay (camera_ray.origin, camera_ray.direction * 10, Color.yellow);

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
			Vector3 bossPosition = GameObject.FindWithTag ("Boss").transform.position;
			Vector3 projectile_trajectory;
			Vector3 plutoVelocity = GetComponent<Rigidbody> ().velocity;

			if ((transform.position - bossPosition).magnitude < bossVolumeRadius) {
				projectile_trajectory = (bossPosition - transform.position).normalized;
				speed = projectile_max_speed;
				spawnProjectile (projectile_trajectory, speed, false);
			}
			else {
				// if the player didn't swipe, we don't spawn a projectile
				end_position = Input.mousePosition;
				if (end_position == start_position) {
					pluto_pressed = false;
					return;
				}

				// calculate the normalized projectile trajectory
				projectile_trajectory = (end_position - start_position).normalized;

				// transform the trajectory from the x-y plane to the x-z plane
				projectile_trajectory.z = projectile_trajectory.y;
				projectile_trajectory.y = 0.0f;

				// get the speed (*note speed_scale is a fudge factor, may need to refactor)
				speed = (float)((end_position - start_position).magnitude/speed_scale) / delta_time;
				spawnProjectile (projectile_trajectory, speed, true);
			}

			pluto_pressed = false;
		}
	}

	// getters
	public bool plutoPressed() {
		return pluto_pressed;
	}
}
