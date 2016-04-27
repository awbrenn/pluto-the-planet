using UnityEngine;
using System.Collections;

public class tutorialTexts : MonoBehaviour {
	public GameObject[] messages;

	private chatter chatterBox;
	// Use this for initialization
	void Start () {
		chatterBox = gameObject.GetComponent<chatter> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (messages != null) {
			foreach (GameObject message in messages) {
				chatterBox.queueTextMessage (message);
			}
		}

		messages = null;
	}
}
