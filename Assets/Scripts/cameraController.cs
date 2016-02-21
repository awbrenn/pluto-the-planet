using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	public GameObject player;
	public GameObject bossVolume;
	public GameObject boss;

	private Vector3 playerLoc;
	private float cameraHeight;
	private float playerSize;

	// Use this for initialization
	void Start () {
		cameraHeight = 5f;
		playerLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
		gameObject.transform.position = playerLoc;
	}
	
	// Update is called once per frame
	void Update () {
		playerLoc = new Vector3(player.transform.position.x, cameraHeight, player.transform.position.z);
		gameObject.transform.position = playerLoc;
	}
}
