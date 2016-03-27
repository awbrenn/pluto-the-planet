using UnityEngine;
using System.Collections;

public class destroyFoodInsideOtherFood : MonoBehaviour {
	void OnTriggerEnter (Collider other){
		float currTime = Time.time;
		// Hack to make sure that food that spawns inside other food is destroyed
		if (other.CompareTag ("food")  && Time.timeSinceLevelLoad < 1.0f) {
			Destroy (other.gameObject);
		}
	}
}
