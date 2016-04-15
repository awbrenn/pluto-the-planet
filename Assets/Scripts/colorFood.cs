using UnityEngine;
using System.Collections;

public class colorFood : MonoBehaviour {
	// ratio of foodHealth/plutoHealth in order to be edible
	public float percentageOfPlutosHealthForEating = 0.35f;
	public int eyeColorIndex = 1;
	public Texture blueTexture;
	public Texture redTexture;

	private objectHealth plutosHealth;
	private objectHealth foodsHealth;
	private Renderer foodRenderer;
	// Use this for initialization
	void Start () {
		// get plutos health script
		plutosHealth = GameObject.Find ("Pluto").GetComponent<objectHealth> () as objectHealth;
		foodsHealth = GetComponentInParent<objectHealth> () as objectHealth;
		foodRenderer = GetComponent<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float plutoHealthValue = plutosHealth.getHealth ();
//		Debug.Log ("Pluto's health:  " + plutoHealthValue + "  Food's Health:  " + foodsHealth.getHealth() + "  Fraction of Size:  " + ((float) (foodsHealth.getHealth ()) / (float) (plutoHealthValue)));

		if (((float) (foodsHealth.getHealth ()) / (float) (plutoHealthValue)) <= percentageOfPlutosHealthForEating || ((float)foodsHealth.getHealth ()) <= 10f) {
			foodRenderer.materials[eyeColorIndex].SetTexture("_MainTex", blueTexture);
//			Debug.Log ("Pluto's health:  " + plutoHealthValue + "  Food's Health:  " + foodsHealth.getHealth());
		} 

		else {
			foodRenderer.materials[eyeColorIndex].SetTexture("_MainTex", redTexture);
		}
	}
}
