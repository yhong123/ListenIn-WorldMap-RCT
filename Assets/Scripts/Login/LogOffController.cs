using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogOffController : MonoBehaviour
{
    [SerializeField] private GameObject logOutPanel;
    [SerializeField] private Text emailText;

    private void Start()
    {
        if (!PlayerPrefManager.IsLogged()) return;
        emailText.text = PlayerPrefManager.GetHiddenEmail();
        logOutPanel.SetActive(true);
    }

    public void LogOut()
    {
        PlayerPrefManager.LogOut();
        logOutPanel.SetActive(false);
    }

    public void CloseLogOutPanel()
    {
        logOutPanel.SetActive(false);
    }
}
