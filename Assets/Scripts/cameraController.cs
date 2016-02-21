using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public GameObject bossVolume;
	public GameObject boss;
	public float speed = 1f;

	private Vector3 playerLoc;
	private Vector3 camLoc;
	private float cameraHeight;
	private float playerSize;
	private float spd;

	private bool mainGamePlay = true;
	private bool inBossVolume = false;

	private Quaternion outerLookDir;

	// Use this for initialization
	void Start () {
		outerLookDir = Quaternion.Euler(90, 0, 0);
		spd = speed;
		cameraHeight = 2f;
		playerLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
		transform.position = playerLoc;
	}
	
	// Update is called once per frame
	void Update () {
/*		Vector3 camLocTest = transform.position;
		camLocTest.y = 0f;
		Vector3 playerLocTest = player.transform.position;
		float camPlayerDistance = (camLocTest - playerLocTest).magnitude;
		if (camPlayerDistance > 2) {
			spd += camPlayerDistance;
		}
		else {
			spd = speed;
		}*/
		if (mainGamePlay){
			float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
			cameraHeight = ((playerHealth/100) + 5f);

			inBossVolume = bossVolume.GetComponent<SphereCollider> ().bounds.Contains (player.transform.position);

			if (inBossVolume) {
//				Debug.Log ("inBossVolume");
				spd += 0;

				float newCameraHeight = cameraHeight + 5f;

				float step = spd * Time.deltaTime;

				Vector3 pLoc = player.transform.position;
				Vector3 bLoc = boss.transform.position;
				Vector3 lookPos = ((bLoc - pLoc / 3) + pLoc);
//				float playerBossDistance = Vector3.Distance(bLoc, pLoc);

//				Ray playerToBoss = new Ray (pLoc, bLoc);
//				Vector3 newCLoc = playerToBoss.GetPoint (playerBossDistance);
				camLoc = new Vector3(player.transform.position.x, newCameraHeight, player.transform.position.z);
//				camLoc.y = newCameraHeight;


				print (lookPos);
//				print ("playerLocation" + pLoc);
//				print ("camLocation: "+newCLoc);

				transform.position = Vector3.Lerp (transform.position, camLoc, step);
				transform.LookAt (lookPos);
				spd = speed;

//				Vector3 adjustedCamPoint = Ray playerToBoss ();
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
}
