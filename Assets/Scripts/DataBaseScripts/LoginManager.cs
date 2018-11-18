using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginManager : MonoBehaviour
{

    private string loginURL = "http://softvtech.website/ListenIn/php/login.php";
    [SerializeField] private Button loginButton;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private InputField emailInput;

    public void LoginButton()
    {
        loginButton.interactable = false;
        StartCoroutine(Login());
    }

    private IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);

        using (WWW www = new WWW(loginURL, form))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("ERROR CONNECTING TO THE DATABSE: "+ www.error);
                loginButton.interactable = true;
                yield break;
            }
            else
            {
                if (www.text == "true")
                {
                    Debug.Log("LOG IN SUCCESFUL");
                }
                else
                {
                    Debug.Log("LOG IN ERROR");
                }
                loginButton.interactable = true;
            }
        }
    }
}
