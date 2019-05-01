using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public enum RegistrationStep { AgeConcent, ParticipationConcent, Concent, Genre, DateOfBirth, Cause, DateOfOnset, Registration }

public class RegistrationController : MonoBehaviour
{
    public List<RegistrationStepObject> ListOfRegistrationStepObject;

    private RegistrationStep currentRegistrationStep;
    [HideInInspector]
    public RegistrationStep CurrentRegistrationStep
    {
        set
        {
            currentRegistrationStep = value;
            CheckRegistrationStepStatus();
        }

        get
        {
            return currentRegistrationStep;
        }
    }

    public Button ConcentNext;
    public Toggle ConcentAcceptCurrent;
    public List<string> ListOfConcentText;
    public Text ConcentText;
    private int currentConcentTextIndex = 0;

    public GameObject RegistrationObject;

    public void NextRegistrationStep()
    {
        CurrentRegistrationStep++;
    }

    private void CheckRegistrationStepStatus()
    {
        switch (CurrentRegistrationStep)
        {
            case RegistrationStep.AgeConcent:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.ParticipationConcent:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.AgeConcent).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.Concent:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep ==  RegistrationStep.ParticipationConcent).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                SetConcentText();
                break;
            case RegistrationStep.Genre:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.Concent).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.DateOfBirth:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.Genre).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.Cause:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.DateOfBirth).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.DateOfOnset:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.Cause).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.Registration:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.DateOfOnset).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Restart()
    {
        foreach (RegistrationStepObject item in ListOfRegistrationStepObject)
        {
            item.RegistrationObject.SetActive(false);
        }
        currentConcentTextIndex = 0;
    }

    public void SetConcentText()
    {
        if(currentConcentTextIndex == ListOfConcentText.Count)
        {
            NextRegistrationStep();
            return;
        }

        ConcentNext.interactable = false;
        ConcentAcceptCurrent.isOn = false;

        ConcentText.text = ListOfConcentText[currentConcentTextIndex];

        currentConcentTextIndex++;
    }

    public void StartRegistration()
    {
        Restart();
        CurrentRegistrationStep = RegistrationStep.AgeConcent;
        RegistrationObject.SetActive(true);
    }

    public void BackToLogin()
    {
        RegistrationObject.SetActive(false);
    }
}

[Serializable]
public class RegistrationStepObject
{
    public RegistrationStep RegistrationStep;
    public GameObject RegistrationObject;
}

