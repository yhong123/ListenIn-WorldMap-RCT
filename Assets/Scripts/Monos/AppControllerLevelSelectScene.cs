﻿using UnityEngine;
using System.Collections;

public class AppControllerLevelSelectScene : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GameController.Instance.ChangeState(GameController.States.Idle);
        //GameController.Instance.Init();
    }
	
	// Update is called once per frame
	void Update () {
        GameController.Instance.Update();
    }
}
