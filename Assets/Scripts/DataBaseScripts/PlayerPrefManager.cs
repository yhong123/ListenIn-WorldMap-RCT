using UnityEngine;
using System.Collections;
using MadLevelManager;

public class PlayerPrefManager : MonoBehaviour
{
    private const string usernamePlayerPref = "username";
    
    public static bool IsLogged()
    {
        return PlayerPrefs.HasKey(usernamePlayerPref);
    }

    public static string GetUsername()
    {
        return PlayerPrefs.GetString(usernamePlayerPref);
    }
    
    public static void SetPlayerPref(string value)
    {
        PlayerPrefs.SetString(usernamePlayerPref, value);
        PlayerPrefs.Save();
    }

    public static void LogOut()
    {
        PlayerPrefs.SetString(usernamePlayerPref, string.Empty);
        PlayerPrefs.Save();
        MadLevel.LoadLevelByName("Login");
    }
}
