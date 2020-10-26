using UnityEngine;
using UnityEngine.UI;
using MadLevelManager;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Text;

public class AppControllerSetupScreen : MonoBehaviour
{

    [SerializeField]
    private Text m_textScreen;
    private string m_textStringFormat = "Setting up... {0}%";

    [SerializeField]
    private Text m_feedbackTextScreen;
    private string m_resultOnScreen;

    [SerializeField]
    private Button m_playButton;

    [SerializeField]
    private GameObject switchPatient;

    private bool lockEmailSending = false;
    private int setupPorcentageProgress = 0;


    [SerializeField] private Text userID;

    [SerializeField] private bool isDebug = false;

    void Start()
    {
        if (isDebug) return;

        StartSetup();
    }

    public void StartGameWithoutLogin()
    {
        NetworkManager.UserId = userID.text;
        StartSetup();
    }

    public void StartSetup()
    {
        Debug.Log("USER ID: " + NetworkManager.UserId);
        m_playButton.interactable = false;
        m_playButton.gameObject.SetActive(false);
        switchPatient.gameObject.SetActive(false);
        GlobalVars.LiroGenActBasketFile = string.Empty; //GEN_ACT_BASKET
        GlobalVars.LiroGenActFile = string.Empty; //GEN_ACT

        try
        {
            StartCoroutine(SetupInitialization());
        }
        catch (Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }
    }

    private void UpdateFeedbackLog(String message, bool canContinue)
    {
        if (m_feedbackTextScreen != null)
            m_feedbackTextScreen.text = message;
        m_playButton.interactable = canContinue;
    }
    // Update is called once per frame
    void Update()
    {

    }

    //Called at every start of the game
    private IEnumerator SetupInitialization()
    {
        Application.targetFrameRate = 60;
        //Preventing the screen to go off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);

        //Assigning a Random seed at the    
        UnityEngine.Random.seed = System.Environment.TickCount;

        //AndreaLIRO: disabling logger 
        //Setting the logger
        //GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI_Canvas_Debug"));
        //Text debug_text = go.GetComponentInChildren<Text>();
        //ListenIn.Logger.Instance.SetLoggerUIFrame(debug_text);
        //ListenIn.Logger.Instance.SetLoggerLogToExternal(true);
        //ListenIn.Logger.Instance.Log("AppControllerSetup: Logger started", ListenIn.LoggerMessageType.Info);
        yield return new WaitForEndOfFrame();

        setupPorcentageProgress = 3;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);
        try
        {            
            UploadManager.Instance.Initialize();
            //AndreaLIRO: check if DatabaseXML can be stripped off.
            //DatabaseXML.Instance.InitializeDatabase();
            //DatabaseXML.Instance.OnSwitchedPatient += UpdateFeedbackLog;
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForSeconds(2);

        //AndreaLIRO: Insert therapy ladder new algorithm
        setupPorcentageProgress = 47;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);

        //AndreaLIRO_TB : what happens if get profile back arrives before dowload game progress?
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.UserId);
        NetworkManager.SendDataServer(form, NetworkUrl.SqlGetGameUserProfile, GetProfileCallback);

        ///The progression on the world map
        GameStateSaver.Instance.DownloadWorldMapProgress();
        ///Jigsaw pieces information for each level
        GameStateSaver.Instance.DownloadGameProgress();


    }

    public void GetProfileCallback(string response)
    {
        if(TherapyLIROManager.Instance.SetUserProfile(response))
        {
            StartCoroutine(InitializationACTPairChoose());
        }
    }

    /// <summary>
    /// Function being called during SetupScreen starting after login. The Initialization of ACT pairs is executed only if isFirstInit is true.
    /// </summary>
    /// <returns>Nothing</returns>
    public IEnumerator InitializationACTPairChoose()
    {
        setupPorcentageProgress = 55;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);
        //AndreaLIRO: Checking first ever initialization for ACT pair randomization
        yield return StartCoroutine(TherapyLIROManager.Instance.LIROInitializationACTPairChoose());

        //AndreaLIRO: need to wait until the files are loaded from the server before continuing with the progress
        while (string.IsNullOrEmpty(GlobalVars.LiroGenActBasketFile) && string.IsNullOrEmpty(GlobalVars.LiroGenActFile) && string.IsNullOrEmpty(GlobalVars.GameProgressFile) && string.IsNullOrEmpty(GlobalVars.GameWorldMapProgressFile))
        {
            yield return null;
        }
        //try
        //{
        //    StartCoroutine(TherapyLIROManager.Instance.LIROInitializationACTPairChoose());
        //}
        //catch (Exception ex)
        //{
        //    ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        //}

        //AndreaLIRO: Preparing the jigsaw pieces
        setupPorcentageProgress = 77;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);
        try
        {
            StateJigsawPuzzle.Instance.OnGameLoadedInitialization();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }

        try
        {
            if (GlobalVars.isProfileNewOrChanged)
            {
                //AndreaLIRO_TB: This is not required anymore because if we switch reset Listen In is called before anyway and we don t need to save stuff locally anymore.
                GlobalVars.isProfileNewOrChanged = false;
                //GameStateSaver.Instance.ResetListenIn();
            }
        }
        catch (Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }

        yield return new WaitForSeconds(2);

        setupPorcentageProgress = 85;
        //AndreaLIRO: Loading current world map configuration
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);
        try
        {
            //AndreaLIRO_TB: this is is not done anymre as the progress is saved on the server
            //IMadLevelProfileBackend backend = MadLevelProfile.backend;
            //String profile = backend.LoadProfile(MadLevelProfile.DefaultProfile);
            //ListenIn.Logger.Instance.Log(String.Format("AppControllerSetupScreen: SetupInitialization() loaded pinball level profile: {0}", profile), ListenIn.LoggerMessageType.Info);
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetupScreen: SetupInitialization() {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        setupPorcentageProgress = 92;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);
        try
        {
            //AndreaLIRO_TB: load must happen from the server
            GameStateSaver.Instance.LoadGameProgress();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }
        yield return new WaitForEndOfFrame();

        setupPorcentageProgress = 95;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);

        //AndreaLIRO: Commenting out the last three log files
        // yield return SendLogs();
        yield return new WaitForEndOfFrame();

        setupPorcentageProgress = 99;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);

        try
        {
            CleaningUpOlderLogs();
        }
        catch (System.Exception ex)
        {
            ListenIn.Logger.Instance.Log(String.Format("AppControllerSetup: {0}", ex.Message), ListenIn.LoggerMessageType.Error);
        }

        setupPorcentageProgress = 100;
        m_textScreen.text = String.Format(m_textStringFormat, setupPorcentageProgress);

        //m_playButton.interactable = true;
        //m_playButton.gameObject.SetActive(true);
        //switchPatient.gameObject.SetActive(true);
        //GameStateSaver.Instance.SaveGameProgress();
        yield return new WaitForSeconds(2);
        //DatabaseXML.Instance.OnSwitchedPatient -= UpdateFeedbackLog;
        MadLevel.LoadLevelByName("MainHUB");
    }

    private void CleaningUpOlderLogs()
    {
        //Andrea: need to implement this function
        String path = ListenIn.Logger.Instance.GetLogPath;
        if (!String.IsNullOrEmpty(path))
        {
            List<String> files = new DirectoryInfo(path).GetFiles().OrderBy(f => f.LastWriteTime).Select(x => x.FullName).ToList();
            if (files != null)
            {
                int currCount = files.Count();
                Debug.Log(String.Format("SetupScreen: log count: {0}",currCount));
                if (currCount > 50)
                {
                    Debug.Log("SetupScreen: removing oldest logs");
                    //Removing oldest one, leaving 50 total logs
                    for (int i = 0; i < currCount - 50; i++)
                    {
                        File.Delete(files[i]);
                    }
                }
            }
        }
    }

    //private IEnumerator UploadProfileHistory()
    //{
    //    yield return StartCoroutine(DatabaseXML.Instance.UploadHistory2());
    //}

    //public void GoToWorldMap()
    //{
    //    //Debug.Log("PressedButton");
    //    //DatabaseXML.Instance.OnSwitchedPatient -= UpdateFeedbackLog;
    //    MadLevel.LoadLevelByName("MainHUB");
    //}

    /// <summary>
    /// AndreaLIRO: must be changed to be consistent with new database
    /// </summary>
    /// <returns></returns>
    private IEnumerator SendLogs()
    {
        if (!lockEmailSending)
        {
            yield return StartCoroutine(SendLatestLogs());
        }
        else
        {
            m_feedbackTextScreen.text = "Wait...";
        }
    }

    IEnumerator SendLatestLogs()
    {
        lockEmailSending = true;
        m_feedbackTextScreen.text = "Uploading latest logs...";

        yield return new WaitForEndOfFrame();

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            string path = ListenIn.Logger.Instance.GetLogPath;
            if (!string.IsNullOrEmpty(path))
            {
                var topFiles = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.LastWriteTime).Take(5).ToList();

                if (topFiles != null)
                {
                    m_feedbackTextScreen.text = "Uploading logs...";
                    byte[] logsFile;
                    foreach (var singleFile in topFiles)
                    {
                        logsFile = File.ReadAllBytes(singleFile.FullName);

                        WWWForm form = new WWWForm();
                        form.AddField("patient_id", NetworkManager.UserId);
                        form.AddField("file_log", "file_log");
                        form.AddBinaryData("file_log", logsFile, singleFile.Name);

                        //change the url to the url of the php file
                        WWW w = new WWW("http://italk.ucl.ac.uk/listenin_rct/upload_log.php", form);

                        yield return w;

                        if (w.error == null)
                        {
                            File.Delete(singleFile.FullName);
                        }
                    }

                    m_feedbackTextScreen.text = "Logs uploaded...";
                    yield return new WaitForEndOfFrame();

                }
            }
            else
            {
                m_feedbackTextScreen.text = "Uploading latest logs...";
                yield return new WaitForEndOfFrame();
            }
        }


    }

    //private IEnumerator SendLogToEmail()
    //{

    //    lockEmailSending = true;
    //    //http://answers.unity3d.com/questions/473469/email-a-file-from-editor-script.html
    //    //For setting up accounts this must be turned on: https://www.google.com/settings/security/lesssecureapps
    //    m_feedbackTextScreen.text = "Praparing email...";

    //    yield return new WaitForEndOfFrame();

    //    if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
    //    {
    //        string path = ListenIn.Logger.Instance.GetLogPath;
    //        if (!string.IsNullOrEmpty(path))
    //        {
    //            var topFiles = new DirectoryInfo(path).GetFiles().OrderByDescending(f => f.LastWriteTime).Take(3).Select(x => x.FullName).ToList();

    //            if (topFiles != null)
    //            {
    //                string fromEmail = "listeninlog@gmail.com";
    //                string subject = "Patient id " + NetworkManager.UserId;

    //                using (MailMessage mailMessage = new MailMessage())
    //                {
    //                    mailMessage.From = new MailAddress(fromEmail);
    //                    mailMessage.To.Add("listeninlog@gmail.com");
    //                    mailMessage.Subject = subject;// subject;
    //                    mailMessage.Body = "Log created from application version " + Application.version;

    //                    for (int i = 0; i < topFiles.Count; i++)
    //                    {
    //                        //Adding attachments
    //                        Attachment attachment;
    //                        attachment = new System.Net.Mail.Attachment(topFiles[i]);
    //                        mailMessage.Attachments.Add(attachment);
    //                    }

    //                    {
    //                        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
    //                        smtpServer.Port = 587;
    //                        smtpServer.Credentials = new NetworkCredential("listeninlog@gmail.com", "listeninlogger");
    //                        smtpServer.EnableSsl = true;
    //                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //                        {
    //                            return true;
    //                        };

    //                        yield return new WaitForEndOfFrame();

    //                        try
    //                        {
    //                            smtpServer.Send(mailMessage);
    //                            m_feedbackTextScreen.text = "Thanks for feedback!";
    //                        }
    //                        catch (System.Exception ex)
    //                        {
    //                            m_feedbackTextScreen.text = "Log not uploaded...";
    //                            ListenIn.Logger.Instance.Log(ex.Message, ListenIn.LoggerMessageType.Error);
    //                        }
    //                        finally
    //                        {
    //                            lockEmailSending = false;
    //                        }
    //                    }
    //                }
    //            }



    //            yield return new WaitForEndOfFrame();

    //        }
    //        else
    //        {
    //            lockEmailSending = false;
    //            m_feedbackTextScreen.text = "No internet detected...";
    //        }
    //        yield return null;
    //    }
    //}

    public void SendEmailButton()
    {
        if (!lockEmailSending)
        {
            //StartCoroutine(SendLogToEmail());
        }
        else
        {
            m_feedbackTextScreen.text = "Wait...";
        }


    }
}
