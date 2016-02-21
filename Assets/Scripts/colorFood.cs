using UnityEngine;
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

//		// make sure theres no dividing by zero
//		if (plutoHealthValue <= 0.0)
//			plutoHealthValue = 0.001f; // make it some small value

		if (foodsHealth.getHealth () / plutoHealthValue <= percentageOfPlutosHealthForEating) {
			foodRenderer.material.color = Color.blue;
		} else {
			foodRenderer.material.color = Color.red;
		}
	}
}
