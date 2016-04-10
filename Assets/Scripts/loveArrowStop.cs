using UnityEngine;
using System.Collections;

public class loveArrowStop : MonoBehaviour {
	public float stopTime = 1f;
	public float lifeLength = 8f;
	public float speed = 15f;

	private GameObject target;
	private Vector3 targetLoc;
	private Vector3 initialLoc;

	// Use this for initialization
	void Start () {
		GameObject kissStart = GameObject.FindGameObjectWithTag ("BossMouthLoc");
		target = GameObject.FindGameObjectWithTag ("Player");
		targetLoc = target.transform.position;
		initialLoc = kissStart.transform.position;

		gameObject.transform.position = initialLoc; // for when we want this to be spawning at the mouths loc
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		targetLoc = target.transform.position;
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetLoc, step);
		transform.LookAt (targetLoc);
		StartCoroutine (lifeLengthTimer());
	}

	IEnumerator lifeLengthTimer(){
		yield return new WaitForSeconds (lifeLength);
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag != "Boss" && other.gameObject.tag != "BossScarf") {
			Destroy (gameObject);
			other.attachedRigidbody.velocity = new Vector3 (0,0,0);
			if (other.gameObject.tag == "projectile") {
				Destroy (other.gameObject);
			} else if (other.gameObject.tag == "Player") {
				badEffects stop = other.gameObject.GetComponent<badEffects> () as badEffects;
				stop.loveArrowActivate (stopTime, Time.time);
			}
		}
	}
}
