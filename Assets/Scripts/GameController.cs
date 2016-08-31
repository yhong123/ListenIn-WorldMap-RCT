using UnityEngine;
using System.Collections;
using MadLevelManager;

public class GameController {

    #region singleton
    private static readonly GameController instance = new GameController();
    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion


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

    public State GetState(States state)
    {
        return m_States[(int)state];
    }

    public void ReloadState()
    {
        if (m_CurState != null)
        {
            m_CurState.Init();
        }
    }

    public void ChangeState(States state)
    {
        // Debug.Log("Change State");
        if (m_CurState!=null)
        {
            m_CurState.Exit();
        }
        m_CurState = m_States[(int)state];
        m_CurState.Init();
    }

    public void Init()
    {
		if (!m_Initializtion)
        {
            m_Initializtion = true;
            Application.targetFrameRate = 60;
            DatabaseXML.Instance.InitializeDatabase();
            CUserTherapy.Instance.LoadDataset_UserProfile();
            StateJigsawPuzzle.Instance.OnGameLoadedInitialization();

            IMadLevelProfileBackend backend = MadLevelProfile.backend;
            string profile = backend.LoadProfile(MadLevelProfile.DefaultProfile);
            Debug.Log(profile);

            try
            {
                GameStateSaver.Instance.Load();
            }
            catch (System.Exception ex)
            {

                Debug.LogError(ex.Message);
                GameStateSaver.Instance.ResetListenIn();
            }
            ChangeState(States.Idle);
            //ChangeState(States.Splash);
        }
        else
        {
            ChangeState(States.Idle);
            //TODO this was the default behaviour
            //ReloadState();
        }		
	}
    public void Update()
    {
        if (m_CurState != null)
        {
            m_CurState.Update();
        }
    }

}
