using UnityEngine;
using System.Collections;

public class projectileDamage : MonoBehaviour {
	public float baseDamage = 10f;

	private float damage;

	// Use this for initialization
	void Start () {
		baseDamage = baseDamage * 10f;
		damage = baseDamage * transform.localScale.x;
	}

	public float getDamage (){
		return damage;
	}

	public void setDamage (float newDamage){
		damage = newDamage;
	}
}
