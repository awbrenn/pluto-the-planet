using UnityEngine;
using System.Collections;

public class bossBehavior : MonoBehaviour {
	public GameObject attackType;
	public GameObject hazardType;

	private Animator bossAnimator;

	// Use this for initialization
	void Start () {
		GameObject bossAnimHolder = GameObject.FindGameObjectWithTag ("BossAnimatorGroup");
		bossAnimator = bossAnimHolder.GetComponent<Animator>();

	}

	public Animator findBossAnimator() {
		return bossAnimator;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
