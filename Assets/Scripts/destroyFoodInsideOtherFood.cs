using UnityEngine;
using System.Collections;

public class destroyFoodInsideOtherFood : MonoBehaviour {
	void OnTriggerEnter (Collider other){
		// Hack to make sure that food that spawns inside other food is destroyed
		if (other.CompareTag ("food")  && Time.time < 1.0f) {
			Destroy (other.gameObject);
		}
	}
}
