using UnityEngine;
using System.Collections;

public class scarfCloudPlacement : MonoBehaviour {

	private GameObject venus;
	private Vector3 startScale;
	// Use this for initialization
	void Start () {
		venus = GameObject.FindGameObjectWithTag ("Boss");

		startScale = transform.localScale;

		transform.localScale = Vector3.Scale(startScale, venus.transform.localScale);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localScale = Vector3.Scale(startScale, venus.transform.localScale);
	}
}
