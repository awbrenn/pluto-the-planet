using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public GameObject bossVolume;
	public GameObject boss;
	public float speed = 1f;
	public float camHeight = 10f;
	public float bossHoldTime = 5f;
	public float sceneHoldTime = 10f;
	public float extraIntroHeight = 2f;

	private Vector3 playerLoc;
	private Vector3 camLoc;
	private Vector3 sceneCamLoc;

	private float introSceneHeight;
	private float cameraHeight;
	private float playerSize;
	private float spd;

	private bool introBossLook = true;
	private bool introSceneLook = false;
	private bool mainGamePlay = false;
	private bool inBossVolume = false;

	private Quaternion outerLookDir;

	// Use this for initialization
	void Start () {
		outerLookDir = Quaternion.Euler(90, 0, 0);
		spd = speed;
		cameraHeight = camHeight;
		camLoc = new Vector3 (boss.transform.position.x, 5f, boss.transform.position.z);
		playerLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
		transform.position = camLoc;

		GameObject growthVolume = GameObject.FindGameObjectWithTag ("Growth Volume");
		introSceneHeight = growthVolume.transform.localScale.y / 2 + extraIntroHeight;
		sceneCamLoc = camLoc;
		sceneCamLoc.y = introSceneHeight;
	}
	
	// Update is called once per frame
	void Update () {
		if (introBossLook){
			StartCoroutine (introLookTimer (bossHoldTime));
			introBossLook = false;
			Debug.Log ("end intro boss look");

		}
		if (introSceneLook){
			Debug.Log ("in introSceneLook");
			float step = (spd * 4) * Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, sceneCamLoc, step);
			if (transform.position.y >= (sceneCamLoc.y - 1f)){
				Debug.Log ("begin countdown to main game play");
				StartCoroutine (sceneLookTimer (sceneHoldTime));
				introSceneLook = false;
			}
		}

		if (mainGamePlay){
			float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
			cameraHeight = ((playerHealth/100) + camHeight);
			inBossVolume = bossVolume.GetComponent<SphereCollider> ().bounds.Contains (player.transform.position);

			if (inBossVolume) {
//				Debug.Log ("inBossVolume");
				spd += 0;

				float newCameraHeight = cameraHeight + 5f;

				float step = spd * Time.deltaTime;

				Vector3 pLoc = player.transform.position;
				Vector3 bLoc = boss.transform.position;
				Vector3 lookPos = ((bLoc - pLoc / 3) + pLoc);

				camLoc = new Vector3(player.transform.position.x, newCameraHeight, player.transform.position.z);

//				print (lookPos);

				transform.position = Vector3.Lerp (transform.position, camLoc, step);
				transform.LookAt (lookPos);
				spd = speed;
			}
			else {
//				Debug.Log ("not in boss volume");
				float step = (spd * 4) * Time.deltaTime;
				float rotStep = (spd * 100) * Time.deltaTime;

				camLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
				transform.position = Vector3.Lerp(transform.position, camLoc, step);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, outerLookDir, rotStep);
				spd = speed;
			}
		}
	}

	IEnumerator introLookTimer (float lookTime){
		Debug.Log ("lookTimer started at" + Time.time);

		yield return new WaitForSeconds (lookTime);

		Debug.Log ("lookTimer ended at" + Time.time);

		introSceneLook = true;

	}

	IEnumerator sceneLookTimer (float lookTime){
		Debug.Log ("lookTimer started at" + Time.time);

		yield return new WaitForSeconds (lookTime);

		Debug.Log ("lookTimer ended at" + Time.time);

		mainGamePlay = true;

	}

/*	Vector3 lookCenter(){
		Vector3 center = (((boss.transform.position - attachedTo.transform.position).normalized) * ((attachedTo.transform.localScale.x/2f) + 1f));

		return center;
	}*/
}
