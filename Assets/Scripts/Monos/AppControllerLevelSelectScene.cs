﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using MadLevelManager;

public class AppControllerLevelSelectScene : MonoBehaviour {

    public GameObject IniatilizeTherapyCore;

	// Use this for initialization
	void Awake () {
        GameController.Instance.ChangeState(GameController.States.Idle);
        if (IniatilizeTherapyCore == null)
        {
            Debug.LogError("Please assign the components to AppControllerLevelSelectScene");
        }

        TherapyLIROManager.Instance.m_onAdvancingTherapy += PrepareWorldMapSection;
        //GameController.Instance.Init();
    }

    void Start()
    {
        
    }

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level Loaded calling TherapyLiroManager");
        TherapyLIROManager.Instance.CheckCurrentSection();
    }

    void OnDestroy()
    {
        if(TherapyLIROManager.Instance != null)
            TherapyLIROManager.Instance.m_onAdvancingTherapy -= PrepareWorldMapSection;
    }

    // Update is called once per frame
    void Update () {
        GameController.Instance.Update();
    }

    void PrepareWorldMapSection(TherapyLadderStep currAdvancedStep)
    {
        switch (currAdvancedStep)
        {
            case TherapyLadderStep.CORE:
                IniatilizeTherapyCore.SetActive(true);
                StartCoroutine(ChangeScene(currAdvancedStep));
                break;
            case TherapyLadderStep.ACT:
                break;
            default:
                break;
        }
    }

    public IEnumerator ChangeScene(TherapyLadderStep currAdvanceStep)
    {
        yield return new WaitForSeconds(3);

        switch (currAdvanceStep)
        {
            case TherapyLadderStep.CORE:
                MadLevel.LoadLevelByName("Basket Selection");
                break;
            case TherapyLadderStep.ACT:
                break;
            default:
                break;
        }

    }

}
