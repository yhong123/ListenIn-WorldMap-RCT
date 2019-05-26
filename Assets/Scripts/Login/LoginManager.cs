using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MadLevelManager;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{

    private const string encryptionKey = "Softv01**";
    private const string loginURL = "http://softvtech.website/ListenIn/php/login.php";
    private const string registernURL = "http://softvtech.website/ListenIn/php/register.php";
    [SerializeField]
    private List<Button> listOfButtons;
    [SerializeField]
    private InputField passwordInputLogin;
    [SerializeField]
    private InputField idInputLogin;
    [SerializeField]
    private Animator loginMessageAnimator;
    [SerializeField]
    private Text loginMessageText;
    [SerializeField]
    private InputField emailInputRegister;
    [SerializeField]
    private InputField passwordInputRegister;
    [SerializeField]
    private InputField reenterPasswordInputRegister;
    [SerializeField]
    private GameObject logInForm;
    [SerializeField]
    private GameObject registerForm;
    [SerializeField]
    private RegistrationController registrationController;
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
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefManager.IsLogged())
        {
            PlayerPrefManager.SetPlayerPref(string.Empty);
        }

        ClearInputs();
        logInForm.SetActive(true);
        registerForm.SetActive(false);


        if (PlayerPrefManager.GetUsername() != string.Empty && NetworkManager.HasInternet)
        {
            //START GAME, USER ALREADY EXIST
            MadLevel.LoadLevelByName("Setup Screen");
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
        if (idInputLogin.text == string.Empty || passwordInputLogin.text == string.Empty) return;

        ToggleButtons(false);
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", idInputLogin.text);

        using (WWW www = new WWW(loginURL, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.");
                ToggleButtons(true);
                yield break;
            }
            else
            {
                if (www.text != "false")
                {
                    Debug.Log(StringCipher.Decrypt(www.text, encryptionKey));
                    if (passwordInputLogin.text == StringCipher.Decrypt(www.text, encryptionKey))
                    {
                        Debug.Log("LOG IN SUCCESFUL");
                        NetworkManager.UserId = idInputLogin.text;
                        PlayerPrefManager.SetPlayerPref(NetworkManager.UserId);
                        MadLevel.LoadLevelByName("Setup Screen");
                    }
                    else
                    {
                        Debug.Log("LOG IN ERROR");
                        TriggerLoginMessage("Wrong ID/password.");
                    }
                }
                else
                {
                    Debug.Log("LOG IN ERROR");
                    TriggerLoginMessage("Wrong ID/password.");
                }
                ToggleButtons(true);
            }
        }
    }

    public void Register()
    {
        if (reenterPasswordInputRegister.text == string.Empty || passwordInputRegister.text == string.Empty || emailInputRegister.text == string.Empty)
        {
            return;
        }
        if (passwordInputRegister.text != reenterPasswordInputRegister.text)
        {
            TriggerLoginMessage("Passwords don't match.");
            return;
        }
        ToggleButtons(false);
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", StringCipher.Encrypt(emailInputRegister.text, encryptionKey));
        form.AddField("password", StringCipher.Encrypt(passwordInputRegister.text, encryptionKey));
        form.AddField("genre", registrationController.RegistrationGenre);
        form.AddField("date_of_birth", registrationController.RegistrationDateOfBirth);
        form.AddField("cause", registrationController.RegistrationCause);
        form.AddField("date_of_onset", registrationController.RegistrationDateOfOnset);

        using (WWW www = new WWW(registernURL, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: " + www.error);
                TriggerLoginMessage("Error 404. Contact support.");
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
                    TriggerLoginMessage("Email already exist.");
                }
                ToggleButtons(true);
            }
        }
    }

    private void TriggerLoginMessage(string message)
    {
        loginMessageText.text = message;
        loginMessageAnimator.Play("LoginMessage");
    }

    private void ToggleButtons(bool isInteractible)
    {
        foreach (Button item in listOfButtons)
        {
            item.interactable = isInteractible;
        }
    }

    private void ClearInputs()
    {
        idInputLogin.text = string.Empty;
        emailInputRegister.text = string.Empty;
        passwordInputLogin.text = string.Empty;
        passwordInputRegister.text = string.Empty;
        reenterPasswordInputRegister.text = string.Empty;
    }
}
