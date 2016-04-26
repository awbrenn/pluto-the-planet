using UnityEngine;
using System.Collections;

public class camControlTutorial : MonoBehaviour {
	public GameObject player;
	public float speed = 1f;
	public float smoothTime = .15f;
	public float camHeight = 10f;
	public float bossHoldTime = 5f;
	public float bossLookHeightPadding = 12f;
	public float sceneHoldTime = 10f;
	public float extraIntroHeight = 2f;
	public float sizeChangeStep= 30f;
	public float bossCamHeight = 10f;
	public float eatingRotationMultiplyier = 100f;
	public float bossCamBasePosDivisor = 20f;
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


	private bool mainGamePlay = true;


	private Quaternion outerLookDir;

	// Use this for initialization
	void Start () {
		basePlayerSize = player.transform.localScale.x /2;

		outerLookDir = Quaternion.Euler(90, 0, 0);
		spd = speed;
		cameraHeight = camHeight;
		float bossCamHeight = bossLookHeightPadding;
		camLoc = new Vector3 (player.transform.position.x, bossCamHeight, player.transform.position.z);
		transform.position = camLoc;

		GameObject safeVolume = GameObject.FindGameObjectWithTag ("Safe Volume");
		introSceneHeight = safeVolume.transform.localScale.y / 2 + extraIntroHeight;
		sceneCamLoc = camLoc;
		sceneCamLoc.y = introSceneHeight;

		player.transform.Rotate (90,0,0);
		player.transform.LookAt (transform.localPosition, new Vector3 (0,0,1));
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (mainGamePlay){
			float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
			cameraHeight = ((playerHealth/100) + camHeight);
			//			inTransitionVolume = transitionVolume.GetComponent<SphereCollider> ().bounds.Contains (player.transform.position);


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

			//Vector3 lookDirection = new Vector3 (player.transform.position.x, player.transform.position.y + 1000.0f, player.transform.position.z);
			//Vector3 upDirection = new Vector3 (0.0f, 0.0f, 1.0f);
		}
	}

}
