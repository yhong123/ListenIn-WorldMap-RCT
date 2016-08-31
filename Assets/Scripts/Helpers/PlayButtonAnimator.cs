using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayButtonAnimator : MonoBehaviour {

	private Image image;

	private Color startColor;
	public Color endColor;
	public float blinkTime;
	private Color currentColor;

	private float currTime = 0.0f;

	private bool activePlay;
	public bool ActivePlay {
		get{return activePlay;}
		set{activePlay = value;}
	}

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		image = GetComponent<Image>();
		currentColor = startColor = image.color;
		activePlay = false;
	}

	// Update is called once per frame
	void Update () {
		if(activePlay)
		{
			currTime += Time.deltaTime;
			currentColor = Color.Lerp(startColor,endColor, currTime / blinkTime);

			if(currTime > blinkTime)
			{
				currTime = 0f;
				Color tempColor = startColor;
				currentColor = startColor = endColor;
				endColor = tempColor;
			}

			image.color = currentColor;
		}
	}
}
