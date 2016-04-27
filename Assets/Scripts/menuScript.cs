using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas menu;

	public Button replay;
	public Button goToMainMenu;
	public Button start;
	public GameObject loadingText;
	public GameObject levelSelect;

	// Use this for initialization
	void Start () {
		menu = menu.GetComponent<Canvas> ();
		if (replay != null) { replay = replay.GetComponent<Button> (); }
		if (goToMainMenu != null) { goToMainMenu = goToMainMenu.GetComponent<Button> (); }
		if (start != null) { start = start.GetComponent<Button> (); }
	}

	public void replayPressed () {
		GameObject[] hideObjects = GameObject.FindGameObjectsWithTag ("hideButton");
		foreach (GameObject hideObject in hideObjects) {
			hideObject.SetActive (false);
		}

		loadingText.SetActive (true);

		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	public void startPressed () {
		
		GameObject[] hideObjects = GameObject.FindGameObjectsWithTag ("hideButton");
		foreach (GameObject hideObject in hideObjects) {
			hideObject.SetActive (false);
		}

		levelSelect.SetActive (true);
	}

	public void goToMenuPressed () {
		GameObject[] hideObjects = GameObject.FindGameObjectsWithTag ("hideButton");
		foreach (GameObject hideObject in hideObjects) {
			hideObject.SetActive (false);
		}

		loadingText.SetActive (true);

		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void loadLevel(int levelIndex) {
		GameObject[] hideObjects = GameObject.FindGameObjectsWithTag ("hideUI");
		foreach (GameObject hideObject in hideObjects) {
			hideObject.SetActive (false);
		}

		levelSelect.SetActive (false);
		loadingText.SetActive (true);

		UnityEngine.SceneManagement.SceneManager.LoadScene (levelIndex);
	}
}
