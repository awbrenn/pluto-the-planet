using UnityEngine;
using System.Collections;

public class plutoBasicAttack : MonoBehaviour {
	public float projectile_max_speed = 10.0f;
	public float projectileMinSpeed = 2f;
	public float percentageOfPlutosHealthForHealth = 0.03f;
	public float percentageOfPlutosHealthForDamage = .1f;
	public int minDamage = 10;
	public AudioClip shootSound;
	public bool shooting = false;
	public float sensitivity = 500.0f;

	private AudioSource source;
	private float speed;
	private Vector3 start_position;
	private Vector3 end_position;
	private GameObject projectile_prefab;
	private float time_of_touch;
	private float bossVolumeRadius;
	private float start_time;
	private bool check_for_swipe = true;

	// Use this for initialization
	void Start () {
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
		projectile_initial_position = projectile_trajectory * ((transform.localScale.x/2.0f + projectile.transform.localScale.x/2.0f) * 2.0f) + transform.position;
		projectile.transform.position = projectile_initial_position;


		Rigidbody projectile_rigid_body = projectile.GetComponent<Rigidbody> () as Rigidbody;
		Vector3 pluto_velocity = (GetComponent<Rigidbody> ()).velocity;
		if (addPlutoVelocity) {
			projectile_rigid_body.velocity = (projectile_trajectory * speed + pluto_velocity);
		} else {
			projectile_rigid_body.velocity = (projectile_trajectory * speed);
		}
			
		gameObject.GetComponent<objectHealth> ().adjustHealth ((int)((-1.0f) * plutosHealth * percentageOfPlutosHealthForDamage));

		source.PlayOneShot (shootSound, 1f);
	}

	void shootProjectile() {
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
			if (end_position == start_position) {
				shooting = false;
				return;
			}

			// calculate the normalized projectile trajectory
			projectile_trajectory = (end_position - start_position).normalized;

			// transform the trajectory from the x-y plane to the x-z plane
			projectile_trajectory.z = projectile_trajectory.y;
			projectile_trajectory.y = 0.0f;

			speed = projectile_max_speed;
			spawnProjectile (projectile_trajectory, speed, true);
		}
	}


	bool checkForSwipe (float delta_time) {
		bool check = false;

		check = (Input.mousePosition - start_position).magnitude / delta_time > sensitivity;

		return check;
	}


	// Update is called once per frame
	void FixedUpdate () {

		foreach (Touch touch in Input.touches) {
			if (touch.position.x > Screen.width / 2) {
				Debug.Log("shoot " + touch.position.x + " " + Screen.width / 2);

				switch (touch.phase) {
				case TouchPhase.Began:
					start_position = touch.position;
					break;
				case TouchPhase.Ended:
					end_position = touch.position;
					shootProjectile ();
					break;
				}
			}
		}

		//################### controls for PC ######################
		if (Input.GetMouseButtonDown (0) && Input.touches.Length == 0) {
			// is shooting
			if (!shooting) {
				start_position = Input.mousePosition;
				shooting = true;
				time_of_touch = Time.time;
				check_for_swipe = true;
			}
		}

		if (shooting == true && check_for_swipe && Input.touches.Length == 0) {
			if (Time.time - time_of_touch > 0.05f) {
				shooting = checkForSwipe (Time.time - time_of_touch);
				check_for_swipe = false;
			}
		}

		if (Input.GetMouseButtonUp (0) && shooting && Input.touches.Length == 0) {
			end_position = Input.mousePosition;

			if (shooting) {
				shootProjectile ();
			}

			shooting = false;
		}
		//###########################################################
	}

	// getters
	public bool plutoPressed() {
		return shooting;
	}
}
