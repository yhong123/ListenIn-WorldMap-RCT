using UnityEngine;
using System.Collections;

public class AnimationMonitor : MonoBehaviour {

	public void AcknoledgeEndAnimation () {
		StateSplash.Instance.ActivateGameIntro();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
