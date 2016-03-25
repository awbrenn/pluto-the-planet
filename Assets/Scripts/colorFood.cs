﻿using UnityEngine;
using System.Collections;

public class colorFood : MonoBehaviour {
	// ratio of foodHealth/plutoHealth in order to be edible
	public float percentageOfPlutosHealthForEating = 0.35f;

	private objectHealth plutosHealth;
	private objectHealth foodsHealth;
	private Renderer foodRenderer;
	// Use this for initialization
	void Start () {
		// get plutos health script
		plutosHealth = GameObject.Find ("Pluto").GetComponent<objectHealth> () as objectHealth;
		foodsHealth = GetComponent<objectHealth> () as objectHealth;
		foodRenderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float plutoHealthValue = plutosHealth.getHealth ();

		if (((float) (foodsHealth.getHealth ()) / (float) (plutoHealthValue)) <= percentageOfPlutosHealthForEating) {
			foodRenderer.materials[2].color = Color.blue;
//			Debug.Log ("Pluto's health:  " + plutoHealthValue + "  Food's Health:  " + foodsHealth.getHealth());
		} else {
			foodRenderer.materials[2].color = Color.red;
		}
	}
}
