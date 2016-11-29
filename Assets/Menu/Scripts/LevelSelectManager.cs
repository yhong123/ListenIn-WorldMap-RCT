﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectManager : MonoBehaviour {

    [SerializeField] private GameObject OptionTint;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject PauseButton;

    public void OpenPauseMenu()
    {
        Time.timeScale = 0.0f;
        DatabaseXML.Instance.ResetTimer(DatabaseXML.TimerType.Idle);
        DatabaseXML.Instance.SetIsMenu = true;
        OptionTint.GetComponent<CanvasGroup>().alpha = 1.0f;
        PauseMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
        PauseButton.SetActive(false);
        OptionTint.SetActive(true);
        PauseMenu.SetActive(true);
    }

    public void HideMenu()
    {
        Time.timeScale = 1.0f;
        DatabaseXML.Instance.ResetTimer(DatabaseXML.TimerType.Idle);
        DatabaseXML.Instance.SetIsMenu = false;
        OptionTint.GetComponent<CanvasGroup>().alpha = 0.0f;
        PauseMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        PauseButton.SetActive(true);
        OptionTint.SetActive(false);
        PauseMenu.SetActive(false);
    }

    int pico = 0;
    public void testa()
    {
        Dictionary<string, string> dailyTherapy = new Dictionary<string, string>();

        dailyTherapy.Add("patient", pico.ToString());
        dailyTherapy.Add("level_start", "level");
        dailyTherapy.Add("date", "aaaaaaaa");

        DatabaseXML.Instance.WriteDatabaseXML(dailyTherapy, DatabaseXML.Instance.therapy_session_update);
        pico++;
    }

}
