using UnityEngine;
using System.Collections;

public class projectileDamage : MonoBehaviour {
	public float baseDamage = 10f;
	public float lifeLength = 4f;

	private float damage;

	// Use this for initialization
	void Start () {
		damage = baseDamage;

		StartCoroutine (lifeTimer());
	}

	public float getDamage (){
		return damage;
	}

	public void setDamage (float newDamage){
		damage = newDamage;
		baseDamage = newDamage;
	}

	IEnumerator lifeTimer (){
		yield return new WaitForSeconds (lifeLength);
		Destroy (gameObject);
	}
}
