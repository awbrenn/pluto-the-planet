using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Canvas menu;

	public Button replay;
	public Button goToMainMenu;
	public Button start;
	public GameObject loadingText;

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

		loadingText.SetActive (true);

//		GameObject hideObject = GameObject.FindGameObjectWithTag ("hideButton");
//		hideObject.GetComponent<Text> ().text = "Loading...";

		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	public void goToMenuPressed () {
		GameObject[] hideObjects = GameObject.FindGameObjectsWithTag ("hideButton");
		foreach (GameObject hideObject in hideObjects) {
			hideObject.SetActive (false);
		}

		loadingText.SetActive (true);

		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}
}
