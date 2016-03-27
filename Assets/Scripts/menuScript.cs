using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas menu;

	public Button replay;
	public Button goToMainMenu;
	public Button start;

	// Use this for initialization
	void Start () {
		menu = menu.GetComponent<Canvas> ();
		if (replay != null) { replay = replay.GetComponent<Button> (); }
		if (goToMainMenu != null) { goToMainMenu = goToMainMenu.GetComponent<Button> (); }
		if (start != null) { start = start.GetComponent<Button> (); }
	}

	public void replayPressed () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	public void startPressed () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	public void goToMenuPressed () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}
}
