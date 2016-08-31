using UnityEngine;
using System.Collections;

public class ChallengeMenuController : MonoBehaviour {

	public void UnlockChallenges()
	{
		StateChallenge.Instance.UnlockChallenged();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
