using UnityEngine;
using System.Collections;

public class directionPointer : MonoBehaviour {
	public GameObject attachedTo;

	private GameObject boss;
	private Vector3 dirPointerLoc;


	// Use this for initialization
	void Start () {
		boss = GameObject.FindGameObjectWithTag("Boss");
		dirPointerLoc = dirPointerCenter ();
		transform.position = attachedTo.transform.position + dirPointerLoc;
	}
	
	// Update is called once per frame
	void Update () {
		dirPointerLoc = dirPointerCenter ();
		transform.position = attachedTo.transform.position + dirPointerLoc;
	}

	Vector3 dirPointerCenter(){
		Vector3 center = (((boss.transform.position - attachedTo.transform.position).normalized) * ((attachedTo.transform.localScale.x/2f) + 1f));

		return center;
	}
}
