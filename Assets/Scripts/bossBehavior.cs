using UnityEngine;
using System.Collections;

public class bossBehavior : MonoBehaviour {
	public GameObject attackType;
	public GameObject hazardType;

	private Animator bossAnimator;


	public Animator findBossAnimator() {
		GameObject bossAnimHolder = GameObject.FindGameObjectWithTag ("BossAnimatorGroup");
		bossAnimator = bossAnimHolder.GetComponent<Animator>();
		return bossAnimator;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
