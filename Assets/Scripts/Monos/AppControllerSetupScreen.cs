using UnityEngine;
using UnityEngine.UI;
using MadLevelManager;
using System.Collections;

public class AppControllerSetupScreen : MonoBehaviour {

    [SerializeField]
    private Text m_textScreen;
    private string m_textStringFormat = "Setting up... {0}%";

    [SerializeField]
    private Text m_feedbackTextScreen;
    private string m_resultOnScreen;

    [SerializeField]
    private Button m_playButton;

    // Use this for initialization
    void Start () {
        m_playButton.interactable = false;
        m_playButton.gameObject.SetActive(false);
        StartCoroutine(SetupInitialization());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator SetupInitialization()
    {
        Application.targetFrameRate = 60;
        int percentage = 1;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);

        //Setting the logger
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI_Canvas_Debug"));
        Text debug_text = go.GetComponentInChildren<Text>();
        ListenIn.Logger.Instance.SetLoggerUIFrame(debug_text);
        ListenIn.Logger.Instance.SetLoggerLogToExternal(true);
        ListenIn.Logger.Instance.Log("Log started", ListenIn.LoggerMessageType.Info);
        yield return new WaitForEndOfFrame();
        
        percentage = 3;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            DatabaseXML.Instance.InitializeDatabase();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 18;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            UploadManager.Instance.Initialize();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 33;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            CUserTherapy.Instance.LoadDataset_UserProfile();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 48;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            StateJigsawPuzzle.Instance.OnGameLoadedInitialization();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 63;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            IMadLevelProfileBackend backend = MadLevelProfile.backend;
            string profile = backend.LoadProfile(MadLevelProfile.DefaultProfile);
            ListenIn.Logger.Instance.Log(string.Format("Loaded profile: {0}", profile), ListenIn.LoggerMessageType.Info);
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 78;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        try
        {
            GameStateSaver.Instance.Load();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        percentage = 100;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);
        m_playButton.interactable = true;
        m_playButton.gameObject.SetActive(true);

    }

    public void GoToWorldMap()
    {
        //Debug.Log("PressedButton");
        MadLevel.LoadLevelByName("World Map Select");
    }
}
