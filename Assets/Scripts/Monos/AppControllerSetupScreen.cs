using UnityEngine;
using UnityEngine.UI;
using MadLevelManager;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using System.Net;
using System.Net.Mail;

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

        percentage = 85;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);

        try
        {
            CleaningUpOlderLogs();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
        }

        percentage = 100;
        m_textScreen.text = string.Format(m_textStringFormat, percentage);

        m_playButton.interactable = true;
        m_playButton.gameObject.SetActive(true);

    }

    private void CleaningUpOlderLogs()
    {
        //Andrea: need to implement this function
    }

    public void GoToWorldMap()
    {
        //Debug.Log("PressedButton");
        MadLevel.LoadLevelByName("World Map Select");
    }

    public void SendLogToEmail()
    {
        string path = ListenIn.Logger.Instance.GetLogPath;
        if (!string.IsNullOrEmpty(path))
        {
            var topFile = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            string fromEmail = "ListenIn" + Application.version + "@gmail.com";
            mailMessage.From = new MailAddress(fromEmail);
            mailMessage.To.Add("listeninlog@gmail.com");
            string identifier = "";
            //mailMessage.Subject()
            //http://answers.unity3d.com/questions/473469/email-a-file-from-editor-script.html
        }
    }
}
