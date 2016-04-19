using UnityEngine;
using System.Collections;

public class moveGrowDie : MonoBehaviour {
	public GameObject maxRangeBoundary;
	public float startScale = .1f;
	public float maxScale = 8f;
	public float speed = 2f;
	public float scaleSpeed = 3f;
	public float lifeLength = 2f;
	public AudioClip cough;

	public float maxDamage = 10f;
	public float damageRepeatTime = 1f;

//	private Vector3 originPoint;
	private Vector3 targetPoint;
	private Vector3 targetScale;

//	private float max;

	private bool isDoingDamage = false;
	private float timeSinceEnter;
	private AudioSource pASource;
	private Animator pAnimator;
	private float randLL;

	// Use this for initialization
	void Start () {
		GameObject pluto = GameObject.FindGameObjectWithTag ("Player");
		Vector3 plutoLoc = pluto.transform.position;
		GameObject smokeStart = GameObject.FindGameObjectWithTag ("BossMouthLoc");
		pASource = pluto.GetComponent <AudioSource>();
		pAnimator = pluto.GetComponent<Animator> ();

		transform.localScale = new Vector3 (startScale, startScale, startScale);
		transform.position = smokeStart.transform.position;

		float bossVolumeSize = ((GameObject.FindGameObjectWithTag ("Boss Volume").transform.localScale.x) / 2f) - 1f;

		Vector2 randGen = Random.insideUnitCircle * bossVolumeSize;
		targetPoint = new Vector3 (randGen.x, 0, randGen.y);

		int randScale = Random.Range (((int)(maxScale - 4)),((int)(maxScale + 4)));
		maxScale = (float)randScale;

		int randLength = Random.Range (((int)(lifeLength - 5)),((int)(lifeLength + 5)));
		randLL = (float)randLength;

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

		float timeNow = Time.time;
		if (isDoingDamage == true && (timeNow >= (timeSinceEnter + damageRepeatTime))) {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			objectHealth playerHealth = player.gameObject.GetComponent<objectHealth> ();
			if (playerHealth.getHealth() - ((int)maxDamage) <= 0) { 
				UnityEngine.SceneManagement.SceneManager.LoadScene ("youLost");
			}
			else {
				playerHealth.adjustHealth ((int) - maxDamage);
			}
			timeSinceEnter = timeNow;
		}
	}

	IEnumerator pauseAndKill (){
		yield return new WaitForSeconds (randLL);
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			timeSinceEnter = Time.time;
			isDoingDamage = true;
			pASource.PlayOneShot (cough, .5f);
			pAnimator.SetTrigger ("takeDamage");
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			isDoingDamage = false;
			pAnimator.SetTrigger ("leaveGas");
		}
	}
}
