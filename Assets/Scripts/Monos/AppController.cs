using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GameController.Instance.ChangeState(GameController.States.JigsawPuzzle);
	}
	
	// Update is called once per frame
    void Update()
    {
        GameController.Instance.Update();
	}
}
