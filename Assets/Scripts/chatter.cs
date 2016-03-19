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

	private int spacing;
	private Queue<GameObject> queuedTextMessages;
	private Queue<GameObject> currentTextMessages;
	private Vector2 GroupPos;
	public Vector2 thumbSize;
	private Matrix4x4 GUIScaleMatrix;
	private int GUIScreenWidth;
	private int GUIScreenHeight;
	private Vector2 textBoxSize;

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

	void queueTextMessage(GameObject textMessage) {
		currentTextMessages.Enqueue (textMessage);
	}
	
	void FixedUpdate () {
		// don't add a text message if the max is reached
		if (currentTextMessages.Count >= maxTextMessagesOnScreen) {
			return;
		}

		queueTextMessage (startTextMessages [Random.Range (0, startTextMessages.Length)]);
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

		int i = 0;
		foreach (GameObject textMessage in currentTextMessages) {
			int y = (int)(i*textBoxSize.y);
			Rect Group = new Rect(GroupPos.x, GroupPos.y - y, textBoxSize.x, textBoxSize.y);
			GUI.BeginGroup (Group);
			GUI.Box (new Rect (0, 0, textBoxSize.x, textBoxSize.y), textMessage.GetComponent<message> ().text + " " + i);
			GUI.Box (new Rect (0, 0, thumbSize.x, thumbSize.y), "");
			GUI.DrawTexture (TextureBox, textMessage.GetComponent<message> ().characterThumb);
			GUI.EndGroup ();
			++i;
		}
	}
}
