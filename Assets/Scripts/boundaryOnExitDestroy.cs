using UnityEngine;
using System.Collections;

public class boundaryOnExitDestroy : MonoBehaviour {

	void OnTriggerExit (Collider other){
		Destroy (other.gameObject);
	}

}
