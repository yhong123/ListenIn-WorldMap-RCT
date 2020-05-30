using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using MadLevelManager;

public class GameController : Singleton<GameController> {

    //Insert FSM in this


    //Andrea: This should be eliminated 
    private State[] m_States = new State[]{
        StateIdle.Instance,
        StateSplash.Instance,
        StateJigsawPuzzle.Instance,
        StateChapterSelect.Instance,
        StateChallenge.Instance,
        StateTutorialChallenge.Instance,
        StateInitializePinball.Instance,
        StatePinball.Instance,
        StateReward.Instance
    };
    public enum States
    {
        Idle = 0,
        Splash,
        JigsawPuzzle,
        ChapterSelect,
        StateChallenge,
        StateTutorialChallenge,
		StateInitializePinball,
        StatePinball,
		StateReward
    };

    private State m_CurState;
    private bool m_Initializtion = false;

    public void ChangeState(States state)
    {
        try
        {
            // Debug.Log("Change State");
            if (m_CurState != null)
            {
                m_CurState.Exit();
            }
            m_CurState = m_States[(int)state];
            m_CurState.Init();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }

    }

    public void Update()
    {
        try
        {
            if (m_CurState != null)
            {
                m_CurState.Update();
            }
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }

    }

    

}
