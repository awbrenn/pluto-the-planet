using UnityEngine;
using System.Collections;

public class objectHealth : MonoBehaviour {
	public int startingHealth = 100;
	public int baseHealth = 100;
	public float timePeriodOfHealthChange = 1.0f;

	private float scaleConstant;
	private int newHealth;
	private int health;
	// Use this for initialization
	void Start () {
		health = startingHealth;
		newHealth = health;
		scaleConstant = 1.0f / calculateDiameter (baseHealth);
		updateScale ();
	}

	// health getters and setters
	public void adjustHealth (int healthChange) {
		newHealth += healthChange;
	}

	public void setHealth (int nHealth) {
		newHealth = nHealth;
	}

	public void instantiateHealth(int iHealth) {
		health = iHealth;
		newHealth = iHealth;
		startingHealth = iHealth;
	}

	public int getHealth () {
		return health;
	}


	// updating health core
	void updateHealth (float timeDelta, int healthDifference) {
		int healthIncrement = (int) (healthDifference * (timeDelta / timePeriodOfHealthChange));
		int updatedHealth;

		// TODO: make sure healthIncrement is at least one

		// increment health either positively or negatively based off of healthDifference
		updatedHealth = health + healthIncrement;

		// check if exceeded newHealth if increasing in health
		if (healthDifference > 0 && updatedHealth > newHealth) {
			updatedHealth = newHealth;
		}

		// check if we went below newHealth if decreasing in health
		if (healthDifference < 0 && updatedHealth < newHealth) {
			updatedHealth = newHealth;
		}

		health = updatedHealth;
	}


	float calculateDiameter (int inputHealth) {
		return 0.5f * Mathf.Pow (((3.0f / (4.0f * Mathf.PI)) * ((float)inputHealth)), 1.0f / 3.0f);
	}


	void updateScale () {
		float setScale = calculateDiameter(health) * scaleConstant;

		transform.localScale = new Vector3 (setScale, setScale, setScale);
	}

	// Update is called once per frame
	void Update () {

		// if health changes update it
		if (health != newHealth) {
			updateHealth (Time.deltaTime, newHealth-health);
			updateScale ();
		}
	}
}
