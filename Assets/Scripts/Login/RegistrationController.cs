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

    [SerializeField] private Dropdown dayOfBirth;
    [SerializeField] private Dropdown monthOfBirth;
    [SerializeField] private Dropdown yearOfBirth;

    [SerializeField] private Dropdown monthOfOnset;
    [SerializeField] private Dropdown yearOfOnset;

    //REGISTRATION VALUES
    [HideInInspector] public string RegistrationGenre;
    [HideInInspector] public string RegistrationDateOfBirth;
    [HideInInspector] public string RegistrationCause;
    [HideInInspector] public string HasConcent;
    [HideInInspector] public string RegistrationDateOfOnset;
    //REGISTRATION VALUES
    private bool noOnsetDate = false;

    public void GenreSelect(string genre)
    {
        RegistrationGenre = genre;
        NextRegistrationStep();
    }

    public void CauseSelect(string cause)
    {
        RegistrationCause = cause;
        NextRegistrationStep();
    }

    public void ConcentSelect(string concent)
    {
        HasConcent = concent;
        NextRegistrationStep();
    }

    public void NextRegistrationStep()
    {
        CurrentRegistrationStep++;
    }

    public void NextRegistrationStepNoDateOnset()
    {
        noOnsetDate = true;
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
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.ParticipationConcent).Single().RegistrationObject.SetActive(false);
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
                //register DOB
                RegistrationDateOfBirth = string.Concat(dayOfBirth.options[dayOfBirth.value].text, "/", monthOfBirth.options[monthOfBirth.value].text, "/", yearOfBirth.options[yearOfBirth.value].text);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.DateOfBirth).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.DateOfOnset:
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == RegistrationStep.Cause).Single().RegistrationObject.SetActive(false);
                ListOfRegistrationStepObject.Where(step => step.RegistrationStep == CurrentRegistrationStep).Single().RegistrationObject.SetActive(true);
                break;
            case RegistrationStep.Registration:
                //register date of onset
                if (!noOnsetDate)
                {
                    RegistrationDateOfOnset = string.Concat(monthOfOnset.options[monthOfOnset.value].text, "/", yearOfOnset.options[yearOfOnset.value].text);
                }
                else
                {
                    RegistrationDateOfOnset = "not sure";
                }

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
        noOnsetDate = false;
    }

    public void SetConcentText()
    {
        if (currentConcentTextIndex == ListOfConcentText.Count)
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

