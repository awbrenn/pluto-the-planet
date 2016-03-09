using UnityEngine;
using System.Collections;

public class chatter : MonoBehaviour {
	public GUISkin chatterSkin;
	public Vector2 thumbSize;
	public Vector2 textureOffset = new Vector2(8, 8);
	public int Spacing = 4;
	public Vector2 boxSize;
	public GameObject[] textMessages;

	private Vector2 GroupPos;
	private Rect Group;
	private Matrix4x4 GUIScaleMatrix;
	private int GUIScreenWidth;
	private int GUIScreenHeight;
	private message textMessage;

	// Use this for initialization
	void Start () {
		GUIScreenWidth = Screen.width;
		GUIScreenHeight = Screen.height;

		GroupPos.x = Screen.width - (boxSize.x);
		GroupPos.y = Screen.height - (boxSize.y);
		Group = new Rect(GroupPos.x, GroupPos.y, boxSize.x, boxSize.y);
		textMessage = textMessages [0].GetComponent<message> () as message;
	}
	
	// Update is called once per frame
	void Update () {
		
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


		GUI.BeginGroup(Group);
		GUI.Box(new Rect(0, 0, boxSize.x, boxSize.y), textMessage.text);
		GUI.Box(new Rect(0, 0, thumbSize.x, thumbSize.y), "");
		GUI.DrawTexture(TextureBox, textMessage.characterThumb);
		GUI.EndGroup();
	}
}
