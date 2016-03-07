using UnityEngine;
using System.Collections;

public class chatter : MonoBehaviour {
    public GUISkin ChatterSkin;
    private Matrix4x4 GUIScaleMatrix;
    private int GUIScreenWidth = 1920;
    private int GUIScreenHeight = 1080;

    private chatter[] AllChatterboxes;

    private static bool Talking = false;
    private bool MyTurn;
    public int Priority;

    public float StartTime;
    public float WaitTime = 2;

    public int TotalBoxes;
    private Rect[] Group;
    public Vector2[] GroupPos;
    private bool[] ShowBox;
    private Texture2D[] BoxThumb;
    private string[] BoxText;
    private bool[] DonePlayingLine;
    private float[] Disappear;
    public float[] PlayerPan;
    public GameObject GenericPlayer;
    private AudioSource[] Player;

    public TextAsset ScriptFile;
    private string[] ScriptLines;
    private int CurrentLine;
    public Texture2D[] CharacterThumbs;
    public AudioClip[] VoiceClips;

    public Vector2 ThumbSize;
    public Vector2 TextureOffset = new Vector2(8, 8);
    public int Spacing = 4;
    public Vector2 TextBox;
    private bool AllWait;
    private bool AllDone;

    private bool Halt = false;

    void Awake() {
        if (ScriptFile != null) {
            ScriptLines = ScriptFile.text.Split("\n"[0]);
            CurrentLine = 0;
        } else {
            Debug.LogError("Script not set.");
            Halt = true;
        }

        if (TotalBoxes < 1) {
            Debug.LogError("We're gonna need AT LEAST one box for this to work. Even if it's a 0x0 one...");
            Halt = true;
        }

        if (CharacterThumbs.Length < 1) {
            Debug.LogError("How does it feel to have no thumbs?");
            Halt = true;
        } else {
            bool NoNullThumbs = true;
            for (int i = 0; i < CharacterThumbs.Length; i++) {
                if (NoNullThumbs) {
                    if (CharacterThumbs[i] == null) {
                        NoNullThumbs = false;
                    }
                }
            }

            if (!NoNullThumbs) {
                Debug.LogError("None of the elements of CharacterThumbs may be empty. If you want to be able to have no image, make your own null image.");
                Halt = true;
            }
        }

        if (VoiceClips.Length < 1) {
            Debug.LogError('"' + "..." + '"' + " (Help! I have no voice! Even setting the Size of Voice Clips to 1 and leaving it blank would be OK! ...Anyone there?)");
            Halt = true;
        }

        if (GroupPos.Length != TotalBoxes) {
            Debug.LogError("Amount of elements in GroupPos must be the same as the value of TotalBoxes.");
            Halt = true;
        }

        if (PlayerPan.Length != TotalBoxes) {
            Debug.LogError("Amount of elements in PlayerPan must be the same as the value of TotalBoxes.");
            Halt = true;
        }

        if (ChatterSkin == null) {
            Debug.LogError("Skin not set.");
            Halt = true;
        }
    }

    void Start() {
        if (Halt) {
            return;
        }

        Player = new AudioSource[TotalBoxes];
        for (int i = 0; i < Player.Length; i++) {
            Instantiate(GenericPlayer);
            GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag(GenericPlayer.tag);
            bool FoundPlayer = false;
            foreach (GameObject player in AllPlayers) {
                if (!FoundPlayer) {
                    if (player.transform.parent == null) {
                        player.name = (GenericPlayer.name + " " + i);
                        player.transform.parent = this.gameObject.transform;
                        Player[i] = player.GetComponent<AudioSource>();
                        Player[i].panStereo = PlayerPan[i];
                    }
                }
            }
        }

        Group = new Rect[TotalBoxes];
        for (int i = 0; i < Group.Length; i++) {
            Group[i] = new Rect(GroupPos[i].x, GroupPos[i].y, TextBox.x, TextBox.y);
        }

        BoxThumb = new Texture2D[TotalBoxes];
        for (int i = 0; i < Group.Length; i++) {
            BoxThumb[i] = CharacterThumbs[0];
        }

        ShowBox = new bool[TotalBoxes];
        BoxText = new string[TotalBoxes];
        DonePlayingLine = new bool[TotalBoxes];
        Disappear = new float[TotalBoxes];
    }

	IEnumerator MakeWait() {
		yield return new WaitForSeconds(3);
	}

    void Update() {
        if (Halt) {
            return;
        }

        if (MyTurn) {
            CheckDone();
            if (StartTime != -1) {
                if (Time.timeSinceLevelLoad >= StartTime) {
                    DisappearCountdown();
                    if (!AllWait) {
                        CheckScript();
                    } else {
                        EndWait();
                    }

                    CheckSound();
                }
            }
        } else {
            TurnCheck();
        }
    }

    void CheckShow() {
        AllDone = true;
        for (int i = 0; i < ShowBox.Length; i++) {
            if (AllDone) {
                AllDone = !ShowBox[i];
            }
        }
    }

    void CheckDone() {
        if (CurrentLine == ScriptLines.Length) {
            CheckShow();
            if (AllDone) {
                Talking = false;
                Destroy(this.gameObject);
                return;
            }
        }
    }

    void TurnCheck() {
        if (!Talking) {
            FindChatterboxes();
            bool HighestPriority = true;
            foreach (chatter chatter in AllChatterboxes) {
                if (HighestPriority) {
                    if (chatter.Priority < Priority) {
                        HighestPriority = false;
                    } else if (chatter.Priority == Priority) {
                        if (chatter.StartTime < StartTime) {
                            HighestPriority = false;
                        }
                    }
                }
            }

            if (HighestPriority) {
                Talking = true;
                MyTurn = true;
            }
        }
    }

    void FindChatterboxes() {
        GameObject[] Chatterboxes = GameObject.FindGameObjectsWithTag("Chatter");
        AllChatterboxes = new chatter[Chatterboxes.Length];
        int ChatterNumber = 0;
        foreach (GameObject Chatterbox in Chatterboxes) {
            AllChatterboxes[ChatterNumber] = Chatterbox.GetComponent<chatter>();
            ChatterNumber++;
        }
    }

    void DisappearCountdown() {
        for (int i = 0; i < Player.Length; i++) {
            if (ShowBox[i] && DonePlayingLine[i]) {
                if (Disappear[i] > 0) {
                    Disappear[i] -= Time.deltaTime;
                } else {
                    ShowBox[i] = false;
                    DonePlayingLine[i] = false;
                    Disappear[i] = WaitTime;
                }
            } else {
                Disappear[i] = WaitTime;
            }
        }
    }

    void CheckScript() {
        if ((CurrentLine + 1) < ScriptLines.Length) {
            //Start with "Pos".
            int Pos = int.Parse(ScriptLines[CurrentLine].Substring(3));
            if (Pos >= TotalBoxes) {
                Debug.LogError("Invalid Pos. Must be a number below what is set to TotalBoxes.");
                return;
            }

            ShowBox[Pos] = true;
            CurrentLine++;
            //Start with "Thumb".
            BoxThumb[Pos] = CharacterThumbs[int.Parse(ScriptLines[CurrentLine].Substring(5))];
            CurrentLine++;
            //Start with "Clip".
            Player[Pos].clip = VoiceClips[int.Parse(ScriptLines[CurrentLine].Substring(4))];
            Player[Pos].Play();
            CurrentLine++;
            //Type as normal.
            BoxText[Pos] = ScriptLines[CurrentLine];
            CurrentLine++;
            //Start with "Wait is ".
            AllWait = bool.Parse(ScriptLines[CurrentLine].Substring(8));
            CurrentLine++;
        }
    }

    void EndWait() {
        CheckShow();
        if (AllDone) {
            AllWait = false;
        }
    }

    void CheckSound() {
        for (int i = 0; i < Player.Length; i++) {
            if (ShowBox[i]) {
                if (!Player[i].isPlaying) {
                    DonePlayingLine[i] = true;
                }
            }
        }
    }

    void OnGUI() {
        if (Halt) {
            return;
        }

        GUI.skin = ChatterSkin;
        GUIScaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / GUIScreenWidth, 1.0f * Screen.height / GUIScreenHeight, 1.0F));
        GUI.matrix = GUIScaleMatrix;

        GUI.skin.box.padding.left = ((int)ThumbSize.x + Spacing + 4);
        Rect TextureBox = new Rect(0, 0, ThumbSize.x, ThumbSize.y);
        TextureBox.x += TextureOffset.x;
        TextureBox.y += TextureOffset.y;
        TextureBox.width -= TextureOffset.x;
        TextureBox.height -= TextureOffset.y;

        for (int i = 0; i < Player.Length; i++) {
            if (ShowBox[i]) {
                GUI.BeginGroup(Group[i]);
                GUI.Box(new Rect(0, 0, TextBox.x, TextBox.y), BoxText[i]);
                GUI.Box(new Rect(0, 0, ThumbSize.x, ThumbSize.y), "");
                GUI.DrawTexture(TextureBox, BoxThumb[i]);
                GUI.EndGroup();
            }
        }
    }
}
