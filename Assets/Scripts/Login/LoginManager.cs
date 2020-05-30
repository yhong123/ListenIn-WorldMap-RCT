using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MadLevelManager;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System;

public class LoginManager : MonoBehaviour
{
    private const string encryptionKey = "Softv01**";
    private const string loginUrl = "https://listeninsoftv.ucl.ac.uk/php/login.php";
    private const string registernUrl = "https://listeninsoftv.ucl.ac.uk/php/register.php";
    private const string reSendEmailUrl = "https://listeninsoftv.ucl.ac.uk/php/resend_email.php";
    private const string resetPasswordUrl = "https://listeninsoftv.ucl.ac.uk/php/reset_password_email.php";
    [SerializeField] private InputField passwordInputLogin;
    [SerializeField] private InputField emailInputLogin;
    [SerializeField] private Animator loginMessageAnimator;
    [SerializeField] private Text loginMessageText;
    [SerializeField] private InputField emailInputRegister;
    [SerializeField] private InputField passwordInputRegister;
    [SerializeField] private InputField reenterPasswordInputRegister;
    [SerializeField] private GameObject startVerificationEmailPanel;
    [SerializeField] private InputField emailInputResetPasword;
    [SerializeField] private CanvasGroup blockInputs;
    private bool isInit = false;
    
    private void Update()
    {
        if (NetworkManager.IsInitialInternetCheckDone && !isInit)
        {
            if (NetworkManager.HasInternet)
            {
                Init();
            }
            else
            {
                PlayerPrefManager.LogOut();
            }
            isInit = true;
        }
    }

    private void Init()
    {
        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefManager.IsLogged()) //if it doesn't exist, create
        {
            PlayerPrefManager.SetPlayerPrefData(string.Empty, string.Empty);
        }

        ClearInputs();
        //////////////////////////////////logInForm.SetActive(true);
        //////////////////////////////////registerForm.SetActive(false);


        if (PlayerPrefManager.GetIdUser() != string.Empty && NetworkManager.HasInternet)
        {
            //SHOW EMAIL VERIFICATION
            NetworkManager.UserId = PlayerPrefManager.GetIdUser();
            startVerificationEmailPanel.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefManager.GetHiddenEmail();
            startVerificationEmailPanel.SetActive(true);
        }
    }

    public void LoginButton()
    {
        if (emailInputLogin.text == string.Empty || passwordInputLogin.text == string.Empty) return;

        SetBlockInputs(true);
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        Debug.Log(emailInputLogin.text);
        WWWForm form = new WWWForm();
        form.AddField("email_hash", PHPMd5Hash(emailInputLogin.text));

        using (WWW www = new WWW(loginUrl, form))
        {
            yield return www;
            Debug.Log(www.text);
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                SetBlockInputs(false);
                yield break;
            }
            else
            {
                if (www.text == "email_not_verified")
                {
                    Debug.Log("EMAIL NOT VERIFIED");
                    //TriggerLoginMessage("Email not verified, please check your email inbox/spam folder.", Color.red);
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginResendEmail;
                }
                else if (www.text == "no_user")
                {
                    Debug.Log("LOG IN ERROR");
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginCredentialFail;
                }
                else
                {
                    string password = www.text.Split(' ')[0];
                    string idUser = www.text.Split(' ')[1];
                    Debug.Log(password + " + " + PHPMd5Hash(passwordInputLogin.text));
                    if (PHPMd5Hash(passwordInputLogin.text) == password)
                    {
                        Debug.Log("LOG IN SUCCESFUL");
                        NetworkManager.UserId = idUser;
                        PlayerPrefManager.SetPlayerPrefData(emailInputLogin.text, NetworkManager.UserId);
                        MadLevel.LoadLevelByName("Setup Screen");
                    }
                    else
                    {
                        Debug.Log("LOG IN ERROR");
                        RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginCredentialFail;
                    }
                }
                SetBlockInputs(false);
            }
        }
    }

    public void Register()
    {
        Debug.Log(reenterPasswordInputRegister.text + " + " + passwordInputRegister.text);
        if (reenterPasswordInputRegister.text == string.Empty || passwordInputRegister.text == string.Empty || emailInputRegister.text == string.Empty)
        {
            return;
        }

        if (passwordInputRegister.text != reenterPasswordInputRegister.text)
        {
            TriggerLoginMessage("Passwords don't match.", Color.red);
            return;
        }

        SetBlockInputs(true);
        StartCoroutine(RegisterCoroutine());
    }

    public void Printregistarionshit()
    {
        Debug.Log
        (
            string.Concat
            (
                "email:", emailInputRegister.text, "\n",
                "password: ", passwordInputRegister.text, "\n",
                "concent: ", RegistrationController.Instance.RegistrationHasConcent, "\n",
                "RegistrationGenre: ", RegistrationController.Instance.RegistrationGenre, "\n",
                "RegistrationCause: ", RegistrationController.Instance.RegistrationCause, "\n",
                "RegistrationCanContact: ", RegistrationController.Instance.RegistrationCanContact, "\n",
                "RegistrationUnknownDateOfStroke: ", !RegistrationController.Instance.RegistrationUnknownDateOfStroke ? RegistrationController.Instance.MonthOfOnset.value.ToString() + " " + RegistrationController.Instance.YearOfOnset.value.ToString() : "false", "\n"
            )
        );
    }

    private IEnumerator RegisterCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputRegister.text);
        form.AddField("email_encrypted", StringCipher.Encrypt(emailInputRegister.text, encryptionKey));
        form.AddField("email_hash", PHPMd5Hash(emailInputRegister.text));
        form.AddField("password", passwordInputRegister.text);
        form.AddField("password_hash", PHPMd5Hash(passwordInputRegister.text));
        form.AddField("genre", RegistrationController.Instance.RegistrationGenre);
        form.AddField("cause", RegistrationController.Instance.RegistrationCause);
        form.AddField("date_of_onset", RegistrationController.Instance.RegistrationUnknownDateOfStroke ? "none" : RegistrationController.Instance.MonthOfOnset.value.ToString() + "/" + RegistrationController.Instance.YearOfOnset.value.ToString());
        form.AddField("concent", RegistrationController.Instance.RegistrationHasConcent.ToString());
        form.AddField("can_contact", RegistrationController.Instance.RegistrationCanContact.ToString());

        using (WWW www = new WWW(registernUrl, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                SetBlockInputs(false);
                yield break;
            }
            else
            {
                if (www.text == "bien")
                {
                    Debug.Log("REGISTER SUCCESFUL");
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.Login;
                }
                else if (www.text == "used")
                {
                    Debug.Log("ERROR EMAIL EXIST");
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.RegisterEmailInUse;
                }
                SetBlockInputs(false);
            }
        }
    }

    private void TriggerLoginMessage(string message, Color textColor)
    {
        loginMessageText.color = textColor;
        loginMessageText.text = message;
        loginMessageAnimator.Play("LoginMessage", -1, 0f);
    }

    private void ClearInputs()
    {
        emailInputLogin.text = string.Empty;
        emailInputRegister.text = string.Empty;
        passwordInputLogin.text = string.Empty;
        passwordInputRegister.text = string.Empty;
        reenterPasswordInputRegister.text = string.Empty;
    }

    public void ReSendVerificationEmail()
    {
        StartCoroutine(ReSendVerificationEmailCoroutine());
    }

    private IEnumerator ReSendVerificationEmailCoroutine()
    {
        WWWForm form = new WWWForm();
        Debug.Log(emailInputLogin.text);
        form.AddField("email", emailInputLogin.text);
        form.AddField("email_hash", PHPMd5Hash(emailInputLogin.text));

        using (WWW www = new WWW(reSendEmailUrl, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                yield break;
            }
            else
            {
                if (www.text == "bien")
                {
                    Debug.Log("RESEND SUCCESFUL");
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.Login;
                    TriggerLoginMessage("Please check your email inbox/spam folder .", Color.green);
                }
                else if (www.text == "mal")
                {
                    Debug.Log("<color=red>ERROR EMAIL EXIST</color>");
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginEmailNoExist;
                }
                SetBlockInputs(false);
            }
        }
    }

    public void IsThisThePatientEmail(bool positive)
    {
        if(positive)
        {
            //START GAME, USER ALREADY EXIST
            MadLevel.LoadLevelByName("Setup Screen");
        }
        else
        {
            PlayerPrefManager.LogOut();
        }
    }

    public void ResetPasswordForm()
    {
        emailInputResetPasword.text = string.Empty;
        //////////////////////////////////logInForm.SetActive(false);
        RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginResendEmail;
    }

    public void ResetPassword()
    {
        if (string.IsNullOrEmpty(emailInputResetPasword.text)) return;
        SetBlockInputs(true);
        StartCoroutine(ResetPasswordCoroutine());
    }

    public IEnumerator ResetPasswordCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputResetPasword.text);
        form.AddField("email_hash", PHPMd5Hash(emailInputResetPasword.text));

        using (WWW www = new WWW(resetPasswordUrl, form))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                SetBlockInputs(false);
                yield break;
            }
            else
            {
                if (www.text == "bien")
                {
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.FogottenSuccess;
                    TriggerLoginMessage("Please check your email inbox/spam folder.", Color.green);
                }
                else if (www.text == "mal")
                {
                    RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.LoginEmailNoExist;
                }
                SetBlockInputs(false);
            }
        }
        /////////////////CloseResetPasswordForm();
    }

    public void CloseResetPasswordForm()
    {
        RegistrationController.Instance.CurrentRegistrationStep = RegistrationStep.Login;
        //////////////////////////////////logInForm.SetActive(true);
    }

    private void SetBlockInputs(bool block)
    {
        blockInputs.blocksRaycasts = block;
    }

    #region Utilities
    public static byte[] GetHash(string inputString)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string PHPMd5Hash(string pass)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] input = Encoding.UTF8.GetBytes(pass);
            byte[] hash = md5.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    #endregion
}
