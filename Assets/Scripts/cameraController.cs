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

	// Use this for initialization
	void Start () {
		cameraHeight = 2f;
		playerLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
		transform.position = playerLoc;
	}
	
	// Update is called once per frame
	void Update () {
//		float step = speed * Time.deltaTime;
		playerSize = player.transform.localScale.x;
		float playerHealth = (float)player.GetComponent<objectHealth> ().getHealth();
		cameraHeight = ((playerHealth/100) + 5f);

//		cameraHeight = (playerSize + 5f);
		camLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
//		transform.position = Vector3.Lerp(transform.position, camLoc, step);

		transform.position = camLoc;
	}
}
