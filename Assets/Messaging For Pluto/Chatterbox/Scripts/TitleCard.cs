using UnityEngine;
using System.Collections;

public class TitleCard : MonoBehaviour {
    private Matrix4x4 GUIScaleMatrix;
    private int GUIScreenWidth = 1920;
    private int GUIScreenHeight = 1080;

    public Texture2D[] Images;
    private int CurrentImage = 0;
    public AudioSource Player;
    private float TimePerImage;
    private float TimeWithImage;
    public string NextLevel;

    void Awake() {
        TimePerImage = (Player.clip.length / Images.Length);
    }

    void Update() {
        TimeWithImage += Time.deltaTime;
        if (TimeWithImage >= TimePerImage) {
            if ((CurrentImage + 1) == Images.Length) {
                Application.LoadLevel(NextLevel);
            } else {
                CurrentImage++;
                TimeWithImage = 0;
            }
        }
    }

    void OnGUI() {
        GUIScaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / GUIScreenWidth, 1.0f * Screen.height / GUIScreenHeight, 1.0F));
        GUI.matrix = GUIScaleMatrix;

        GUI.DrawTexture(new Rect(0, 0, GUIScreenWidth, GUIScreenHeight), Images[CurrentImage]);
    }
}
