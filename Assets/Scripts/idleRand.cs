using UnityEngine;
using System.Collections;

public class idleRand : MonoBehaviour {
	private Animator animActivator;

	// Use this for initialization
	void Start () {
		Animator [] animArray = gameObject.GetComponentsInChildren<Animator> ();
		animActivator = animArray [0];
		StartCoroutine (idleRandTimer());
	}
	
	IEnumerator idleRandTimer(){
		yield return new WaitForSeconds (Random.Range(0, 5));
		animActivator.SetTrigger ("offsetTrigger");
	}
}
