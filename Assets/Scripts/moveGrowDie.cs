using UnityEngine;
using System.Collections;

public class moveGrowDie : MonoBehaviour {
	public GameObject maxRangeBoundary;
	public float startScale = .1f;
	public float maxScale = 8f;
	public float speed = 3f;
	public float scaleSpeed = 3f;
	public float lifeLength = 5f;

//	private Vector3 originPoint;
	private Vector3 targetPoint;
	private Vector3 targetScale;

	private float max;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (startScale, startScale, startScale);
		SphereCollider oV = maxRangeBoundary.transform.GetComponent<SphereCollider> () as SphereCollider;
		max = oV.radius * maxRangeBoundary.transform.localScale.x;
		Vector2 randGen = Random.insideUnitCircle * max;

		targetPoint = new Vector3 (randGen.x, 0, randGen.y);
		targetScale = new Vector3 (maxScale, maxScale, maxScale);
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		float scaleStep = scaleSpeed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPoint, step);
		transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, scaleStep);
		if (transform.localScale.x == maxScale && transform.position == targetPoint) {
			StartCoroutine (pauseAndKill());
		}
	}

	IEnumerator pauseAndKill (){
		yield return new WaitForSeconds (5);
		Destroy (gameObject);
	}

}
