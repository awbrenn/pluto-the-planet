using UnityEngine;
using System.Collections;

public class ChatterTriggerExample : MonoBehaviour {
    private Matrix4x4 GUIScaleMatrix;
    private int GUIScreenWidth = 1920;
    private int GUIScreenHeight = 1080;

    public GUISkin Skin;
    public GameObject TargetChatterbox;
    public GameObject ConditionChatterbox;
    private bool Halt = false;

    public string Text;

    void Awake() {
        if (TargetChatterbox.GetComponent<chatter>() == null) {
            Debug.LogError("TargetChatterbox must contain Chatter script.");
            Halt = true;
            return;
        } else if (TargetChatterbox.GetComponent<chatter>().StartTime >= 0) {
            Debug.LogError("TargetChatterbox must have a StartTime of -1 to prevent it from starting on its own." + System.Environment.NewLine + "NOTE: It's possible to use a script like this with a StartTime above 0, so it could start by time or trigger, but it'd be wise to have such a script destroy itself if it triggers by time on its own. For the purposes of this example, we're not doing that.");
            Halt = true;
            return;
        } else if (Skin == null) {
            Debug.LogError("Skin not set.");
            Halt = true;
            return;
        }
    }

    void OnGUI() {
        if (Halt) {
            return;
        }

        GUI.skin = Skin;
        GUIScaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / GUIScreenWidth, 1.0f * Screen.height / GUIScreenHeight, 1.0F));
        GUI.matrix = GUIScaleMatrix;

        GUI.skin.button.wordWrap = true;
        Skin.button.fontSize = 30;

        if (ConditionChatterbox == null) {
            if (GUI.Button(new Rect(750, 440, 420, 200), Text)) {
                TargetChatterbox.GetComponent<chatter>().StartTime = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
