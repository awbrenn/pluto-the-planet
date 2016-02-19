using UnityEngine;
using System.Collections;

public class moveGrowDie : MonoBehaviour {
	public GameObject maxRangeBoundary;
	public float startScale = .5f;
	public float maxScale = 8f;
	public float speed = 6f;

//	private Vector3 originPoint;
	private Vector3 targetPoint;
	private float max;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (startScale, startScale, startScale);
		SphereCollider oV = maxRangeBoundary.transform.GetComponent<SphereCollider> () as SphereCollider;
		max = oV.radius * maxRangeBoundary.transform.localScale.x;
		Vector2 randGen = Random.insideUnitCircle * max;

		targetPoint = new Vector3 (randGen.x, 0, randGen.y);
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
	}
}
