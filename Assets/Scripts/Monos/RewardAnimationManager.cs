using UnityEngine;
using System.Collections;

public class RewardAnimationManager : MonoBehaviour {

	public void AnimationEnded()
	{
        StateJigsawPuzzle.Instance.SwapBackgroundImage(); //StateChapterSelect.Instance.SwapBackgroundImage();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
