using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MadLevelManager;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class LoginManager : MonoBehaviour
{
    private const string encryptionKey = "Softv01**";
    private const string loginUrl = "http://softvtech.website/ListenIn/php/login.php";
    private const string registernUrl = "http://softvtech.website/ListenIn/php/register.php";
    private const string reSendEmailUrl = "http://softvtech.website/ListenIn/php/resend_email.php";
    [SerializeField] private List<Image> listOfButtons;
    [SerializeField] private InputField passwordInputLogin;
    [SerializeField] private InputField emailInputLogin;
    [SerializeField] private Animator loginMessageAnimator;
    [SerializeField] private Text loginMessageText;
    [SerializeField] private InputField emailInputRegister;
    [SerializeField] private InputField passwordInputRegister;
    [SerializeField] private InputField reenterPasswordInputRegister;
    [SerializeField] private GameObject logInForm;
    [SerializeField] private GameObject registerForm;
    [SerializeField] private RegistrationController registrationController;
    [SerializeField] private GameObject reSendEmailButton;
    [SerializeField] private GameObject startVerificationEmailPanel;
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
        logInForm.SetActive(true);
        registerForm.SetActive(false);


        if (PlayerPrefManager.GetIdUser() != string.Empty && NetworkManager.HasInternet)
        {
            //SHOW EMAIL VERIFICATION
            NetworkManager.UserId = PlayerPrefManager.GetIdUser();
            startVerificationEmailPanel.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefManager.GetHiddenEmail();
            startVerificationEmailPanel.SetActive(true);
        }
    }

    public void RegisterButton()
    {
        ClearInputs();
        logInForm.SetActive(false);
        registerForm.SetActive(true);
    }

    public void BackButton()
    {
        ClearInputs();
        logInForm.SetActive(true);
        registerForm.SetActive(false);
        registrationController.BackToLogin();
    }

    public void LoginButton()
    {
        if (emailInputLogin.text == string.Empty || passwordInputLogin.text == string.Empty) return;

        ToggleButtons(false);
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email_hash", GetHashString(emailInputLogin.text));

        using (WWW www = new WWW(loginUrl, form))
        {
            yield return www;
            Debug.Log(www.text);
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                ToggleButtons(true);
                yield break;
            }
            else
            {
                if (www.text == "email_not_verified")
                {
                    Debug.Log("EMAIL NOT VERIFIED");
                    TriggerLoginMessage("Email not verified, please check your email inbox/spam folder.", Color.red);
                    reSendEmailButton.SetActive(true);
                }
                else if (www.text == "no_user")
                {
                    Debug.Log("LOG IN ERROR");
                    TriggerLoginMessage("Wrong ID/password.", Color.red);
                }
                else
                {
                    string password = www.text.Split(' ')[0];
                    string idUser = www.text.Split(' ')[1];
                    Debug.Log(password + " + " + GetHashString(passwordInputLogin.text));
                    if (GetHashString(passwordInputLogin.text) == password)
                    {
                        Debug.Log("LOG IN SUCCESFUL");
                        NetworkManager.UserId = idUser;
                        PlayerPrefManager.SetPlayerPrefData(emailInputLogin.text, NetworkManager.UserId);
                        MadLevel.LoadLevelByName("Setup Screen");
                    }
                    else
                    {
                        Debug.Log("LOG IN ERROR");
                        TriggerLoginMessage("Wrong ID/password.", Color.red);
                    }
                }
                ToggleButtons(true);
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
        ToggleButtons(false);
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputRegister.text);
        form.AddField("email_encrypted", StringCipher.Encrypt(emailInputRegister.text, encryptionKey));
        form.AddField("email_hash", GetHashString(emailInputRegister.text));
        form.AddField("password", passwordInputRegister.text);
        form.AddField("password_hash", GetHashString(passwordInputRegister.text));
        form.AddField("genre", registrationController.RegistrationGenre);
        form.AddField("date_of_birth", registrationController.RegistrationDateOfBirth);
        form.AddField("cause", registrationController.RegistrationCause);
        form.AddField("date_of_onset", registrationController.RegistrationDateOfOnset);
        form.AddField("concent", registrationController.HasConcent);

        using (WWW www = new WWW(registernUrl, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.", Color.red);
                ToggleButtons(true);
                yield break;
            }
            else
            {
                if (www.text == "bien")
                {
                    Debug.Log("REGISTER SUCCESFUL");
                    BackButton();
                }
                else if (www.text == "used")
                {
                    Debug.Log("ERROR EMAIL EXIST");
                    TriggerLoginMessage("Email already exist.", Color.red);
                }
                ToggleButtons(true);
            }
        }
    }

    private void TriggerLoginMessage(string message, Color textColor)
    {
        loginMessageText.color = textColor;
        loginMessageText.text = message;
        loginMessageAnimator.Play("LoginMessage", -1, 0f);
    }

    private void ToggleButtons(bool isInteractible)
    {
        foreach (Image item in listOfButtons)
        {
            Color temp = item.color;
            temp.a = isInteractible ? 1f : 0.2f;
            item.color = temp;
            item.transform.parent.GetComponent<Image>().enabled = isInteractible;
        }
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
        reSendEmailButton.SetActive(false);
        WWWForm form = new WWWForm();
        form.AddField("email", emailInputLogin.text);
        form.AddField("email_hash", GetHashString(emailInputLogin.text));

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
                    Debug.Log("REGISTER SUCCESFUL");
                    TriggerLoginMessage("Please check your email inbox/spam folder .", Color.green);
                }
                else if (www.text == "mal")
                {
                    Debug.Log("ERROR EMAIL EXIST");
                    TriggerLoginMessage("Email doesn't exist.", Color.red);
                }
                ToggleButtons(true);
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

    #region Utilities
    public static byte[] GetHash(string inputString)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
    #endregion
}
