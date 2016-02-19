using UnityEngine;
using System.Collections;

public class sizeScale : MonoBehaviour 
{
	public float startingSize = 100f;
	public float baseSize = 100f;
	public float sizeChangeOverTimeScale = 1f;

	private float size;
	private float setScale;
	private Vector3 scaleHolder;
	private float newSize;
	private float sC;

	// Use this for initialization
	void Start () {
		size = startingSize;
		setScale = size/baseSize;
		scaleHolder = new Vector3 (setScale, setScale, setScale);
		transform.localScale = scaleHolder;
		newSize = size;
	}
	
	// Update is called once per frame
	void Update () {
		if (size < newSize) {

			size += Time.deltaTime * sC;
			if (size > newSize) {
				size = newSize;
				changeSize ();
			}
			changeSize ();
		}
		if (size > newSize) {
			
			size -= Time.deltaTime * sC;
			if (size > newSize) {
				size = newSize;
				changeSize ();
			}
			changeSize ();
		}
	}

/*		if (volume < newVolume) {

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

	}*/

	public float getSize () {
		return size;
	}

	public void setSize (float randSize) {
		startingSize = randSize;
	}

	public void changeSize() {
		setScale = size / baseSize;
		scaleHolder = new Vector3 (setScale, setScale, setScale);
		transform.localScale = scaleHolder;
	}

	public void addSize (float sizeChange) {
		print ("incoming sizeChange:  "+sizeChange);
		newSize += sizeChange;
		print ("updated size" + newSize);
		sC = newSize - size;
		sC = sC * sizeChangeOverTimeScale;
		print ("sizeChange scaler" + sC);

	}
}
