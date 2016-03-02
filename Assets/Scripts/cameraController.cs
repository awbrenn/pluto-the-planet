using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public GameObject transitionVolume;
	public GameObject boss;
	public float speed = 1f;
	public float camHeight = 10f;
	public float bossHoldTime = 5f;
	public float sceneHoldTime = 10f;
	public float plutoHoldTime = .5f;
	public float extraIntroHeight = 2f;
	public float sizeChangeStep= 30f;

	public float smoothTime = .15f;
	private Vector3 velocity = Vector3.zero;

	private Vector3 playerLoc;
	private Vector3 camLoc;
	private Vector3 sceneCamLoc;

	private float introSceneHeight;
	private float cameraHeight;
	private float basePlayerSize;
	private float spd;

	private bool introBossLook = true;
	private bool introSceneLook = false;
	private bool plutoZoomLook = false;
	private bool mainGamePlay = false;
	private bool inTransitionVolume = false;

	private Quaternion outerLookDir;

	// Use this for initialization
	void Start () {
		basePlayerSize = player.transform.localScale.x /2;

		outerLookDir = Quaternion.Euler(90, 0, 0);
		spd = speed;
		cameraHeight = camHeight;
		float bossCamHeight = (boss.transform.localScale.y / 2f) + 5f;
		camLoc = new Vector3 (boss.transform.position.x, bossCamHeight, boss.transform.position.z);
		transform.position = camLoc;

		GameObject safeVolume = GameObject.FindGameObjectWithTag ("Safe Volume");
		introSceneHeight = safeVolume.transform.localScale.y / 2 + extraIntroHeight;
		sceneCamLoc = camLoc;
		sceneCamLoc.y = introSceneHeight;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (introBossLook){
			StartCoroutine (introLookTimer (bossHoldTime));
			introBossLook = false;
//			Debug.Log ("end intro boss look");

		}
		if (introSceneLook){
//			Debug.Log ("in introSceneLook");
			float step = (spd * 4) * Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, sceneCamLoc, step);
			if (transform.position.y >= (sceneCamLoc.y - 1f)){
//				Debug.Log ("begin countdown to pluto zoom");
				StartCoroutine (sceneLookTimer (sceneHoldTime));
				introSceneLook = false;
			}
		}

		if (plutoZoomLook) {
			playerLoc = new Vector3 (player.transform.position.x, cameraHeight, player.transform.position.z);

//			Debug.Log ("begin zoom in to pluto");
			float step = (spd * 4) * Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, playerLoc, step);
			if (transform.position.y <= (playerLoc.y + 1)) {
//				Debug.Log ("begin countdown to main game play");
				StartCoroutine (plutoLookTimer (plutoHoldTime));
				plutoZoomLook = false;
			}
		}


		if (mainGamePlay){
			float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
			cameraHeight = ((playerHealth/100) + camHeight);
			inTransitionVolume = transitionVolume.GetComponent<SphereCollider> ().bounds.Contains (player.transform.position);

			if (inTransitionVolume) {
//				Debug.Log ("inBossVolume");
				spd += 0;

				float newCameraHeight = cameraHeight + 20f;

				float step = spd * Time.deltaTime;

				Vector3 pLoc = player.transform.position;
				Vector3 bLoc = boss.transform.position;
				Vector3 lookPos = ((bLoc - pLoc / 3) + pLoc);

				camLoc = new Vector3(player.transform.position.x, newCameraHeight, player.transform.position.z);

//				print (lookPos);

				transform.position = Vector3.MoveTowards (transform.position, camLoc, step);
				transform.LookAt (lookPos);
				spd = speed;
			}
			else {
				float currentPSize = transform.localScale.x / 2;
				float stepPSize = currentPSize;

//				Debug.Log ("not in boss volume");

				if (currentPSize >= basePlayerSize) {
					if (currentPSize >= stepPSize + sizeChangeStep) {
						cameraHeight += sizeChangeStep * 1f;
						stepPSize = currentPSize;
					}
				}
				

/*				if (spd < player.GetComponent<Rigidbody> ().velocity.magnitude) {
					spd = Mathf.Lerp (spd, player.GetComponent<Rigidbody> ().velocity.magnitude, (Time.deltaTime * .5f));
				}
				
				if (spd > player.GetComponent<Rigidbody> ().velocity.magnitude) {
					spd = Mathf.Lerp (player.GetComponent<Rigidbody> ().velocity.magnitude, spd, (Time.deltaTime * .5f));
				}*/
				
//				float step = (spd * 4) * Time.deltaTime;
				float rotStep = (spd * 100) * Time.deltaTime;

				Vector3 point = GetComponent<Camera> ().WorldToViewportPoint(player.transform.position);
				Vector3 delta = player.transform.position - GetComponent<Camera> ().ViewportToWorldPoint (new Vector3(.5f, .5f, point.z));
				Vector3 destination = transform.position + delta;
				destination.y = cameraHeight;

//				camLoc = new Vector3 (player.transform.position.x, cameraHeight, player.transform.position.z);
				transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, smoothTime);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, outerLookDir, rotStep);
//				spd = speed;
			}
		}
	}

	IEnumerator introLookTimer (float lookTime){
//		Debug.Log ("lookTimer started at" + Time.time);

		yield return new WaitForSeconds (lookTime);

//		Debug.Log ("lookTimer ended at" + Time.time);

		introSceneLook = true;

	}

	IEnumerator sceneLookTimer (float lookTime){
//		Debug.Log ("lookTimer started at" + Time.time);

		yield return new WaitForSeconds (lookTime);

//		Debug.Log ("lookTimer ended at" + Time.time);

		plutoZoomLook = true;

	}

	IEnumerator plutoLookTimer(float lookTime){
		yield return new WaitForSeconds (lookTime);
		mainGamePlay = true;
	}

/*	Vector3 lookCenter(){
		Vector3 center = (((boss.transform.position - attachedTo.transform.position).normalized) * ((attachedTo.transform.localScale.x/2f) + 1f));

		return center;
	}*/
}
