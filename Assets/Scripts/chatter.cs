using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chatter : MonoBehaviour {
	public GUISkin chatterSkin;
	public Vector2 thumbSize;
	public Vector2 textureOffset = new Vector2(8, 8);
	public int Spacing = 4;
	public Vector2 boxSize;
	private int maxTextMessagesOnScreen = 3;
	public GameObject[] startTextMessages;

	private Queue<GameObject> queuedTextMessages;
	private Queue<GameObject> currentTextMessages;
	private Vector2 GroupPos;
	private Matrix4x4 GUIScaleMatrix;
	private int GUIScreenWidth;
	private int GUIScreenHeight;

	// Use this for initialization
	void Start () {
		GUIScreenWidth = Screen.width;
		GUIScreenHeight = Screen.height;

		queuedTextMessages = new Queue<GameObject>();
		currentTextMessages = new Queue<GameObject>();

		GroupPos.x = Screen.width - (boxSize.x);
		GroupPos.y = Screen.height - (boxSize.y);
	}

	void queueTextMessage(GameObject textMessage) {
		currentTextMessages.Enqueue (textMessage);
	}
	
	void FixedUpdate () {
		// don't add a text message if the max is reached
		if (currentTextMessages.Count > maxTextMessagesOnScreen) {
			return;
		}

		queueTextMessage (startTextMessages [Random.Range (0, startTextMessages.Length)]);
	}

	void OnGUI() {
		GUI.skin = chatterSkin;
		GUIScaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / GUIScreenWidth, 1.0f * Screen.height / GUIScreenHeight, 1.0F));
		GUI.matrix = GUIScaleMatrix;

		GUI.skin.box.padding.left = (int)(thumbSize.x + Spacing + 4);
		Rect TextureBox = new Rect(0, 0, thumbSize.x, thumbSize.y);
		TextureBox.x += textureOffset.x;
		TextureBox.y += textureOffset.y;
		TextureBox.width -= textureOffset.x;
		TextureBox.height -= textureOffset.y;

		int i = 0;
		foreach (GameObject textMessage in currentTextMessages) {
			int y = Screen.height - ((int)(boxSize.y + (i*boxSize.y)));
			Rect Group = new Rect(GroupPos.x, GroupPos.y, boxSize.x, boxSize.y);
			GUI.BeginGroup (Group);
			GUI.Box (new Rect (0, 0, boxSize.x, boxSize.y), textMessage.GetComponent<message> ().text);
			GUI.Box (new Rect (0, 0, thumbSize.x, thumbSize.y), "");
			GUI.DrawTexture (TextureBox, textMessage.GetComponent<message> ().characterThumb);
			GUI.EndGroup ();
			++i;
		}
	}
}
