using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MadLevelManager;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{

    private const string loginURL = "http://softvtech.website/ListenIn/php/login.php";
    private const string registernURL = "http://softvtech.website/ListenIn/php/register.php";
    [SerializeField] private List<Button> listOfButtons;
    [SerializeField] private InputField passwordInputLogin;
    [SerializeField] private InputField emailInputLogin;
    [SerializeField] private Animator loginMessageAnimator;
    [SerializeField] private Text loginMessageText;
    [SerializeField] private InputField emailInputRegister;
    [SerializeField] private InputField passwordInputRegister;
    [SerializeField] private InputField reenterPasswordInputRegister;
    [SerializeField] private GameObject logInForm;
    [SerializeField] private GameObject registerForm;
    private const string usernamePlayerPref = "username";

    public void Awake()
    {
        if(!PlayerPrefs.HasKey(usernamePlayerPref))
        {
            SetPlayerPref(string.Empty);
        }

        ClearInputs();
        logInForm.SetActive(true);
        registerForm.SetActive(false);

        
        if(PlayerPrefs.GetString(usernamePlayerPref) != string.Empty)
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
        form.AddField("email", emailInputLogin.text);
        form.AddField("password", passwordInputLogin.text);

        using (WWW www = new WWW(loginURL, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: "+ www.error);
                TriggerLoginMessage("Error 404. Contact support.");
                ToggleButtons(true);
                yield break;
            }
            else
            {
                if (www.text == "true")
                {
                    Debug.Log("LOG IN SUCCESFUL");
                    NetworkManager.UserId = emailInputLogin.text;
                    SetPlayerPref(NetworkManager.UserId);
                    MadLevel.LoadLevelByName("Setup Screen");
                }
                else
                {
                    Debug.Log("LOG IN ERROR");
                    TriggerLoginMessage("Wrong email/password.");
                }
                ToggleButtons(true);
            }
        }
    }

    public void LogOut()
    {
        SetPlayerPref(string.Empty);
    }

    private void SetPlayerPref(string value)
    {
        PlayerPrefs.SetString(usernamePlayerPref, value);
        PlayerPrefs.Save();
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
        form.AddField("email", emailInputRegister.text);
        form.AddField("password", passwordInputRegister.text);

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
                else if(www.text == "used")
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
        emailInputLogin.text = string.Empty;
        emailInputRegister.text = string.Empty;
        passwordInputLogin.text = string.Empty;
        passwordInputRegister.text = string.Empty;
        reenterPasswordInputRegister.text = string.Empty;
    }
}
