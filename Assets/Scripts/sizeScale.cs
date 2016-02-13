using UnityEngine;
using System.Collections;

public class sizeScale : MonoBehaviour 
{
	public float startingSize = 100f;
	public float baseSize = 100f;
	public float sizeChangeOverTimeScale = 1f;

	private float volume;
	private float baseVolume;
	private float newVolume;

	private float size;
	private float setScale;
	private Vector3 scaleHolder;
	private float newSize;
	private float sC;

	// Use this for initialization
	void Start () {
		volume = (4f / 3f) * (3.14f) * (startingSize) * (startingSize) * (startingSize);
		baseVolume= (4f / 3f) * (3.14f) * (baseSize) * (baseSize) * (baseSize);

//		size = startingSize;
		setScale = volume / baseVolume;
		scaleHolder = new Vector3 (setScale, setScale, setScale);
		transform.localScale = scaleHolder;
		newVolume = volume;

//		newSize = startingSize;
	}
	
	// Update is called once per frame
	void Update () {
/*		if (size < newSize) {

			size += Time.deltaTime * sC;
			if (size > newSize) 
				{
				size = newSize;
				changeSize ();
				}
			changeSize ();
		}
		if (size > newSize) {
			
			size -= Time.deltaTime * sC;
			if (size > newSize) 
			{
				size = newSize;
				changeSize ();
			}
			changeSize ();
		}
*/
		if (volume < newVolume) {

			volume += Time.deltaTime * sC;
			if (volume > newVolume) 
			{
				volume = newVolume;
				changeSize ();
			}
			changeSize ();
		}
		if (volume > newVolume) {

			volume -= Time.deltaTime * sC;
			if (volume > newVolume) 
			{
				volume = newVolume;
				changeSize ();
			}
			changeSize ();
		}

	}

	public float getSize () {
		size = volume;
		return size;
	}

	public void setSize (float randSize) {
		volume = randSize;
		startingSize = volume;
	}

	public void changeSize() {
		setScale = volume / baseVolume;
		scaleHolder = new Vector3 (setScale, setScale, setScale);
		transform.localScale = scaleHolder;
	}

	public void addSize (float sizeChange) {
		newVolume += sizeChange;
		sC = newVolume - volume;
		sC = sC * sizeChangeOverTimeScale;
	}
}
