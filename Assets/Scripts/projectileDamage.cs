using UnityEngine;
using System.Collections;

public class projectileDamage : MonoBehaviour {
	public float baseDamage = 10f;
	public float damageSizeScale = 100f;

	private float damage;
	private float setScale;
	private Vector3 scaleHolder;

	// Use this for initialization
	void Start () {
		damage = baseDamage;
		setScale = damage / (damageSizeScale/2); //makes larger projectiles for visibility
		scaleHolder = new Vector3 (setScale, setScale, setScale);
		transform.localScale = scaleHolder;
	}

	public float getDamage (){
		return damage;
	}

	public void setDamage (float newDamage){
		damage = newDamage;
	}
}
