using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public GameObject transitionVolume;
	public GameObject boss;
	public float speed = 1f;
	public float bossSpeed = 20f;
	public float smoothTime = .15f;
	public float camHeight = 10f;
	public float bossHoldTime = 5f;
	public float bossLookHeightPadding = 12f;
	public float sceneHoldTime = 10f;
	public float extraIntroHeight = 2f;
	public float sizeChangeStep= 30f;
	public float bossCamHeight = 10f;
	public float eatingRotationMultiplyier = 100f;
	public float bossCamBasePosDivisor = 6f;
	public float bossCamLookPosDivisor = 1f;
	public float bossRotSpeed = 1f;

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
	private bool mainGamePlay = false;
	private bool inTransitionVolume = false;
	private bool inBossVolume = false;

	private Quaternion outerLookDir;

	// Use this for initialization
	void Start () {
		basePlayerSize = player.transform.localScale.x /2;

		outerLookDir = Quaternion.Euler(90, 0, 0);
		spd = speed;
		cameraHeight = camHeight;
		float bossCamHeight = (boss.transform.localScale.y / 2f) + bossLookHeightPadding;
		camLoc = new Vector3 (boss.transform.position.x, bossCamHeight, boss.transform.position.z);
		transform.position = camLoc;

		GameObject safeVolume = GameObject.FindGameObjectWithTag ("Safe Volume");
		introSceneHeight = safeVolume.transform.localScale.y / 2 + extraIntroHeight;
		sceneCamLoc = camLoc;
		sceneCamLoc.y = introSceneHeight;

		player.transform.Rotate (90,0,0);
		boss.transform.LookAt (transform.localPosition, new Vector3 (0,0,1));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (introBossLook){
			player.GetComponent<tapToMovePluto> ().setCanMove (false);
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

		if (mainGamePlay){
			float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
			cameraHeight = ((playerHealth/100) + camHeight);
//			inTransitionVolume = transitionVolume.GetComponent<SphereCollider> ().bounds.Contains (player.transform.position);

			if (inBossVolume) {
//				Debug.Log ("inBossVolume");
//				float newCameraHeight = cameraHeight + bossCamHeight;
				float step = bossSpeed * Time.deltaTime;

				Vector3 pLoc = player.transform.position;

				Vector3 current = Vector3.zero - pLoc;
				float max = transitionVolume.transform.localScale.x / 2f;
				float distance = max - current.magnitude;
				float distancePercent = distance / max;
				float newCameraHeight = bossCamHeight * (1f - distancePercent) + (playerHealth/200);
//				float newCameraPos = bossCamBasePosDivisor * (1f - distancePercent);

				Vector3 bLoc = boss.transform.position;
				Vector3 lookPos = ((bLoc - pLoc / bossCamLookPosDivisor) + pLoc);
				Vector3 camBasePos = (-(bLoc - pLoc / bossCamBasePosDivisor) + pLoc);

				camLoc = new Vector3((camBasePos.x + 1), newCameraHeight, (camBasePos.z + 1));
//				print (lookPos);

				transform.position = Vector3.MoveTowards (transform.position, camLoc, step);
				transform.LookAt (lookPos);
				spd = speed;
				Vector3 upChange = new Vector3 (0.0f,1.0f,0.0f);
				player.transform.LookAt (boss.transform.localPosition, upChange);
//				boss.transform.LookAt (player.transform.localPosition, upChange);
				Quaternion newBossRot = Quaternion.LookRotation(player.transform.position);
				float bossRotStep = bossRotSpeed * Time.deltaTime;
				boss.transform.rotation = Quaternion.Lerp (boss.transform.rotation, newBossRot, bossRotStep);

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

				float rotStep = spd * eatingRotationMultiplyier * Time.deltaTime;

				Vector3 point = GetComponent<Camera> ().WorldToViewportPoint(player.transform.position);
				Vector3 delta = player.transform.position - GetComponent<Camera> ().ViewportToWorldPoint (new Vector3(.5f, .5f, point.z));
				Vector3 destination = transform.position + delta;
				destination.y = cameraHeight;

//				camLoc = new Vector3 (player.transform.position.x, cameraHeight, player.transform.position.z);
				transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, smoothTime);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, outerLookDir, rotStep);

				Vector3 lookDirection = new Vector3 (player.transform.position.x, player.transform.position.y + 1000.0f, player.transform.position.z);
				Vector3 upDirection = new Vector3 (0.0f, 0.0f, 1.0f);

				if (inTransitionVolume) {
					Vector3 upChange = new Vector3 (0.0f,1.0f,0.0f);
					player.transform.LookAt (boss.transform.localPosition, upChange);
				} 
				else {
					player.transform.LookAt (lookDirection, upDirection);
				}
				Quaternion newBossRot = Quaternion.LookRotation(player.transform.position);
				float bossRotStep = bossRotSpeed * Time.deltaTime;
				boss.transform.rotation = Quaternion.Lerp (boss.transform.rotation, newBossRot, bossRotStep);
			}
		}
	}

	public void setTransitionCamera (bool inOrOut){
		inTransitionVolume = inOrOut;
	}

	public void setBossLook (bool inOrOut){
		inBossVolume = inOrOut;
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
//		plutoZoomLook = true;
		player.GetComponent<tapToMovePluto> ().setCanMove (true);
		mainGamePlay = true;
	}
}
