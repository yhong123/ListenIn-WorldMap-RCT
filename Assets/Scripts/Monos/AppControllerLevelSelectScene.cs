using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AppControllerLevelSelectScene : MonoBehaviour {

    GameObject IniatilizeTherapyCore;

	// Use this for initialization
	void Awake () {
        GameController.Instance.ChangeState(GameController.States.Idle);
        if (IniatilizeTherapyCore != null)
        {
            Debug.LogError("Please assign the components to AppControllerLevelSelectScene");
        }
        //GameController.Instance.Init();
    }

    void Start()
    {
        
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level Loaded calling TherapyLiroManager");
        TherapyLIROManager.Instance.CheckCurrentSection();
    }

    // Update is called once per frame
    void Update () {
        GameController.Instance.Update();
    }

}
