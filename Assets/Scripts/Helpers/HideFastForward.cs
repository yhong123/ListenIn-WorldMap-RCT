using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HideFastForward : MonoBehaviour {

    private GameObject fastForward;

    // Use this for initialization
    void OnEnable() {

        if (fastForward == null)
        {
            Transform fastForwardtransform = transform.FindChild("FastForward");

            if (fastForwardtransform != null)
            {
                fastForward = fastForwardtransform.gameObject;
            }
            else
            {
                Debug.Log("Fast Forward button not found: returning...");
                return;
            }

        }
        

        Scene currScene = SceneManager.GetActiveScene();

        //Andrea
        if (currScene.name == "GameLoop")
        {
            GameObject challengeTherapy = GameObject.FindGameObjectWithTag("Challenge");
            if (challengeTherapy != null)
            {
                MenuManager mm = challengeTherapy.GetComponentInChildren<MenuManager>();
                if (mm != null)
                {
                    Debug.Log("Opening menu in therapy: hiding fast forward button");
                    fastForward.SetActive(false);
                    return;
                }
            }

            GameObject jigsawPuzzle = GameObject.FindGameObjectWithTag("PinballPrefab");
            if (jigsawPuzzle != null)
            {
                MenuManager csm = jigsawPuzzle.GetComponentInChildren<MenuManager>();
                if (csm != null)
                {
                    Debug.Log("Opening menu in pinball game: enabling");
                    fastForward.SetActive(true);
                    
                }
            }

        }

     }
	
	// Update is called once per frame
	void Update () {
	
	}
}
