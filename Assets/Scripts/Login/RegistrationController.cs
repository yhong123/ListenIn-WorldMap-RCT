using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public enum RegistrationStep
{
    Splash,
    Login,
    Register,
    RegisterEmailInUse,
    RegisterSuccess,
    LoginCredentialFail,
    ForgottenPassword,
    LoginIncorrectCredentials,
    LoginResendEmail,
    LoginEmailNoExist,
    FogottenSuccess,
    StudyInformation,
    ResearchStudy,
    ResearchStudyDataCollection,
    ResearchStudyConfidentiality,
    ResearchStudyPersonalInformation,
    ResearchStudyInformationSheet,
    ResearchStudyWithdraw,
    ResearchStudyDecision,
    ResearchStudyThanks,
    ResearchStudySubmitted,
    ResearchStudyDeny,
    Questions,
    QuestionsGenre,
    QuestionsCause,
    QuestionsDate,
    QuestionsContact,
    QuestionsComplete,
    Tests,
    TestsBrief,
    TestsContinue
}

public class RegistrationController : MonoBehaviour
{
    public Text CurrentShit;
    public static RegistrationController Instance;

    public List<RegistrationStepObject> ListOfRegistrationStepObject;

    private RegistrationStep currentRegistrationStep;
    [HideInInspector]
    public RegistrationStep CurrentRegistrationStep
    {
        set
        {
            SetCurrentPanel(currentRegistrationStep, false);
            currentRegistrationStep = value;
            SetCurrentPanel(currentRegistrationStep, true);
        }

        get
        {
            return currentRegistrationStep;
        }
    }

    [SerializeField] private Image concentNext;
    public Toggle ConcentAcceptCurrent;
    public List<string> ListOfConcentText;
    public Text ConcentText;
    private int currentConcentTextIndex = 0;

    public GameObject RegistrationObject;
    [SerializeField] private GameObject loginObject;
    [SerializeField] private Toggle concentToggle;

    //REGISTRATION VALUES
    [HideInInspector] public bool RegistrationHasConcent = false;
    [HideInInspector] public string RegistrationGenre;
    [HideInInspector] public string RegistrationCause;
    public Dropdown MonthOfOnset;
    public Dropdown YearOfOnset;
    [HideInInspector] public bool RegistrationCanContact = false;
    [HideInInspector] public bool RegistrationUnknownDateOfStroke = false;
    //REGISTRATION VALUES

    private bool noOnsetDate = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (RegistrationStepObject item in ListOfRegistrationStepObject)
        {
            SetCanvasGroupVisibility(item.RegistrationCanvas, false);
        }

        SetCanvasGroupVisibility(ListOfRegistrationStepObject.Where(type => type.RegistrationStep == RegistrationStep.Splash).Single().RegistrationCanvas, true);
    }

    public void SetDateForStrokeToDefault()
    {
        RegistrationUnknownDateOfStroke = false;
    }

    private void Restart()
    {
        foreach (RegistrationStepObject item in ListOfRegistrationStepObject)
        {
            SetCanvasGroupVisibility(item.RegistrationCanvas, true);
        }
        currentConcentTextIndex = 0;
        noOnsetDate = false;
    }

    public void SetConcentText()
    {
        if (currentConcentTextIndex == ListOfConcentText.Count)
        {
            return;
        }

        DisableConcentNextButtonInteractable();
        ConcentAcceptCurrent.isOn = false;

        ConcentText.text = ListOfConcentText[currentConcentTextIndex];

        currentConcentTextIndex++;
    }


    public void SetConcentNextButtonInteractable()
    {
        Color temp = concentNext.color;
        temp.a = concentToggle.isOn ? 1f : 0.2f;
        concentNext.color = temp;
        concentNext.transform.parent.GetComponent<Image>().enabled = concentToggle.isOn;
    }

    public void DisableConcentNextButtonInteractable()
    {
        Color temp = concentNext.color;
        temp.a = 0.2f;
        concentNext.color = temp;
        concentNext.transform.parent.GetComponent<Image>().enabled = false;
    }

    private void SetCurrentPanel(RegistrationStep phase, bool isVisible)
    {
        Debug.Log("SET:" + phase.ToString() + " to " + isVisible);
        RegistrationStepObject currentStep = ListOfRegistrationStepObject.Where(type => type.RegistrationStep == phase).SingleOrDefault();
        SetCanvasGroupVisibility(currentStep.RegistrationCanvas, isVisible);
        CurrentShit.text = currentStep.RegistrationStep.ToString();

        if (!isVisible) return;

        if (currentStep.OnActive != null)
        {
            currentStep.OnActive.Invoke();
        }
    }

    private void SetCanvasGroupVisibility(CanvasGroup panel, bool isVisible)
    {
        panel.blocksRaycasts = isVisible;
        panel.interactable = isVisible;
        panel.alpha = isVisible ? 1 : 0;
    }
}

[Serializable]
public class RegistrationStepObject
{
    public RegistrationStep RegistrationStep;
    public CanvasGroup RegistrationCanvas;
    public UnityEvent OnActive;
}

