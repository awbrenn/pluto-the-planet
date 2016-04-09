using UnityEngine;
using System.Collections;

public class ControlGUIScript : MonoBehaviour {

	public GameObject whosHealth;

	public float Value = 0.5f;
	public float Fade = 0.01f;

	public GUIBarScript GBS;

	public Vector2 Offset= new Vector2 (0, 200);

	public Vector2 LabelOffSet;

	public string playText = "Play";
	public bool IsPlaying = false;

	private float currentHealth;
	private float maxHealth;
	private float currentHealthPercentage;

	private objectHealth oHealth;
	private maximumSize mHealth;

	void Start()
	{
		//GBS = GameObject.FindGameObjectWithTag ("PlutoHealthBar").GetComponent<GUIBarScript>;
		oHealth = whosHealth.GetComponent<objectHealth>();
		mHealth = whosHealth.GetComponent<maximumSize>();

		int cH = oHealth.getStartingHealth();
		int mH = mHealth.getMaxHealth();

		currentHealth =  (float)cH;
		maxHealth = (float) mH;
		currentHealthPercentage = currentHealth / maxHealth;
		Value = currentHealthPercentage;
	}

	void OnGUI() 
	{
		if (GBS == null)
		{
			return;
		}

		if (IsPlaying != true)
		{
			GUIStyle LabelStyle =  new GUIStyle();
			LabelStyle.normal.textColor = Color.black;
			LabelStyle.fontSize = 18;
//			GUI.Label(new Rect(GBS.Position.x + Offset.x + LabelOffSet.x, GBS.Position.y + Offset.y + LabelOffSet.y - 40, 100, 25),"Value",LabelStyle);
			Value = GUI.HorizontalSlider(new Rect(GBS.Position.x + Offset.x , GBS.Position.y + Offset.y - 40, 180, 25), Value, 0.0F, 1F);
		}

/*		if (GUI.Button(new Rect(GBS.Position.x + Offset.x + LabelOffSet.x, GBS.Position.y + Offset.y + LabelOffSet.y - 80, 100, 25),playText ))
		{
			if (IsPlaying == true)
			{
				IsPlaying = false;
				playText = "Play";
			}
			else
			{
				IsPlaying = true;
				playText = "Stop";
            }
            
        }*/

	}

	void Update () 
	{
		int cH = oHealth.getHealth();
		int mH = mHealth.getMaxHealth();

		Debug.Log (cH);

		currentHealth =  (float)cH;
		maxHealth = (float) mH;
		currentHealthPercentage = currentHealth / maxHealth;

		Debug.Log (currentHealthPercentage);

		if (GBS == null)
		{
			return;
		}

		if (IsPlaying == true)
		{
			GBS.Value = currentHealthPercentage;
        }
		else
		{
			GBS.Value = currentHealthPercentage;

//			Debug.Log ("I'm happening");
		}
	}
}
