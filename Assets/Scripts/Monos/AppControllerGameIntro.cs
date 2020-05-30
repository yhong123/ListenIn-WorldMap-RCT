using UnityEngine;
using System.Collections;

public class AppControllerGameIntro : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GameController.Instance.ChangeState(GameController.States.Splash);
	}
	
	// Update is called once per frame
	void Update () {
        GameController.Instance.Update();
    }
}
