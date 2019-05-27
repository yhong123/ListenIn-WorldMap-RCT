using UnityEngine;
using System.Collections;
using MadLevelManager;

public class PlayerPrefManager : MonoBehaviour
{
    private const string idUserPlayerPref = "IdUser";
    private const string emailPlayerPref = "Email";

    public static bool IsLogged()
    {
        return PlayerPrefs.HasKey(idUserPlayerPref);
    }

    public static string GetIdUser()
    {
        return PlayerPrefs.GetString(idUserPlayerPref);
    }

    public static string GetEmail()
    {
        return PlayerPrefs.GetString(emailPlayerPref);
    }

    public static void SetPlayerPrefData(string email, string idUser)
    {
        PlayerPrefs.SetString(idUserPlayerPref, idUser);
        PlayerPrefs.SetString(emailPlayerPref, email);
        PlayerPrefs.Save();
    }

    public static void LogOut()
    {
        PlayerPrefs.SetString(idUserPlayerPref, string.Empty);
        PlayerPrefs.SetString(emailPlayerPref, string.Empty);
        PlayerPrefs.Save();
        MadLevel.LoadLevelByName("Login");
    }
}
