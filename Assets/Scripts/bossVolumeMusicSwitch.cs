using UnityEngine;
using System.Collections;

public class bossVolumeMusicSwitch : MonoBehaviour {
	private AudioSource bossMusic;
	private AudioSource growthMusic;

	private float maxVolume;

	private bool musicSwitch = false;

	// Use this for initialization
	void Start () {
		GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera");
		AudioSource[] gameMusicChoices = cam.GetComponents<AudioSource> ();
		growthMusic = gameMusicChoices [0];
		bossMusic = gameMusicChoices [1];

		maxVolume = growthMusic.volume;
	}

	void Update () {
		if (musicSwitch){
			if (bossMusic.isPlaying){
				bossMusic.Stop ();
				growthMusic.Play ();
			}
		}

		else {
			if (growthMusic.isPlaying){
				growthMusic.Stop ();
				bossMusic.Play ();
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player"){
			musicSwitch = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player"){
			musicSwitch = false;
		}
	}
}
