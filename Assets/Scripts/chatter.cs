using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chatter : MonoBehaviour {
	public GUISkin chatterSkin;
	public Vector2 textureOffset = new Vector2(8, 8);
	public float spacingPercentage = 0.004f;
	public float textBoxWidthPercentage = 0.33f;
	public float textBoxHeightPercentage = 0.33f;
	public int maxTextMessagesOnScreen = 3;
	public GameObject[] startTextMessages;
	public float timeBetweenRandomConversation = 5.0f;
	public float timeBetweenTextMessages = 1.0f;
	public float timeUntilClearTextMessages = 4.0f;

	private int spacing;
	public Queue<GameObject> queuedTextMessages;
	private Queue<GameObject> currentTextMessages;
	private Vector2 GroupPos;
	public Vector2 thumbSize;
	private Matrix4x4 GUIScaleMatrix;
	private int GUIScreenWidth;
	private int GUIScreenHeight;
	private Vector2 textBoxSize;
	private float startTimeOfRandomConversation;
	private float timeOfLastTextMessage = 0.0f;

	// Use this for initialization
	void Start () {
		GUIScreenWidth = Screen.width;
		GUIScreenHeight = Screen.height;

		textBoxSize.x = Screen.width * textBoxWidthPercentage;
		textBoxSize.y = Screen.height * textBoxHeightPercentage;

		thumbSize.x = textBoxSize.y;
		thumbSize.y = textBoxSize.y;

		spacing = Screen.width * spacing;

		GroupPos.x = Screen.width - textBoxSize.x;
		GroupPos.y = Screen.height - textBoxSize.y;

		queuedTextMessages = new Queue<GameObject>();
		currentTextMessages = new Queue<GameObject>();
	}

	public void queueTextMessage(GameObject textMessage) {
		if (textMessage == null) {
			return;
		} else {
			queuedTextMessages.Enqueue (textMessage);
			GameObject[] responses = textMessage.GetComponent<message> ().responses;
			GameObject response = responses[Random.Range (0, responses.Length)];
			queueTextMessage (response);
		}
	}

	void updateCurrentTextMessages() {
		if (Time.time - timeOfLastTextMessage > timeBetweenTextMessages && queuedTextMessages.Count > 0) {
			currentTextMessages.Enqueue(queuedTextMessages.Dequeue());
			timeOfLastTextMessage = Time.time;
		} 
		else if (timeOfLastTextMessage == 0.0f && queuedTextMessages.Count > 0) {
			currentTextMessages.Enqueue(queuedTextMessages.Dequeue());
			timeOfLastTextMessage = Time.time;
		}

		if (currentTextMessages.Count > maxTextMessagesOnScreen) {
			currentTextMessages.Dequeue ();
		}

		// clear text messages after a certain amount of time
		if (Time.time - timeOfLastTextMessage > timeUntilClearTextMessages) {
			
		}
	}

	void FixedUpdate () {
//		if (Time.time - startTimeOfRandomConversation > timeBetweenRandomConversation) {
//			queueTextMessage (startTextMessages [Random.Range (0, startTextMessages.Length)]);
//			startTimeOfRandomConversation = Time.time;
//		}

		updateCurrentTextMessages ();
	}

	void OnGUI() {
		GUI.skin = chatterSkin;
		GUIScaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / GUIScreenWidth, 1.0f * Screen.height / GUIScreenHeight, 1.0F));
		GUI.matrix = GUIScaleMatrix;

		GUI.skin.box.padding.left = (int)(thumbSize.x + spacing + 4);
		Rect TextureBox = new Rect(0, 0, thumbSize.x, thumbSize.y);
		TextureBox.x += textureOffset.x;
		TextureBox.y += textureOffset.y;
		TextureBox.width -= textureOffset.x;
		TextureBox.height -= textureOffset.y;

		for (int i = currentTextMessages.Count-1; i >= 0; --i) {
			GameObject textMessage = currentTextMessages.ToArray()[i];
			int x_offset = (int)(Screen.width * spacingPercentage);
			int y_offset = (int)(((currentTextMessages.Count - 1) - i) * (textBoxSize.y) + (currentTextMessages.Count - i) * Screen.height * spacingPercentage);
			Rect Group = new Rect(GroupPos.x - x_offset, GroupPos.y - y_offset, textBoxSize.x, textBoxSize.y);
			GUI.BeginGroup (Group);
			GUI.Box (new Rect (0, 0, textBoxSize.x, textBoxSize.y), textMessage.GetComponent<message> ().text + " ");
			GUI.Box (new Rect (0, 0, thumbSize.x, thumbSize.y), "");
			GUI.DrawTexture (TextureBox, textMessage.GetComponent<message> ().characterThumb);
			GUI.EndGroup ();
		}
	}
}
