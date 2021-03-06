﻿using UnityEngine;
using System.Collections;

public class StateIdle : State {

    #region singleton
    private static readonly StateIdle instance = new StateIdle();
    public static StateIdle Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    // Use this for initialization
    public override void Init()
    {
        DatabaseXML.Instance.SetTimerState(DatabaseXML.TimerType.WorldMap, true);
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public override void Exit()
    {
        //DatabaseXML.Instance.SetTimerState(DatabaseXML.TimerType.WorldMap, false);
    }
}
