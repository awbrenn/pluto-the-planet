using UnityEngine;
using System.Collections;

public class badEffects : MonoBehaviour {
	private GameObject player;

	private bool loveArrowIsStopping = false;
	private float loveArrowStopLength;
	private float loveArrowStartTime;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if (loveArrowIsStopping) {
			tapToMovePluto tapCtrl = player.GetComponent<tapToMovePluto>();
			tapCtrl.setCanMove(false);
			Vector3 currentVelocity = gameObject.GetComponent<Rigidbody> ().velocity;
			if ((currentVelocity.x > 0) || (currentVelocity.z > 0)) {
				gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
			}
			if ((Time.time) > (loveArrowStopLength + loveArrowStartTime)) {
				loveArrowIsStopping = false;
				tapCtrl.setCanMove(true);
			}
		}
	}

	public void loveArrowActivate (float lifeLength, float startTime){
		loveArrowStopLength = lifeLength;
		Debug.Log ("stop length:  " + loveArrowStopLength);

		loveArrowStartTime = startTime;
		loveArrowIsStopping = true;
	}
}