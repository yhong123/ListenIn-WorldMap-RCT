using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using MadLevelManager;

public class AppControllerMainHUB : MonoBehaviour {

    public Button m_continueButton;
    public Text m_currentSectionText;
    public ACTUiWorldMAp m_feedbackACT;

    private TherapyLadderStep currLadderStep;
    private string currLevelToLoad = "World Map Select";

    void Awake()
    {
        //AndreaLIRO: goes into the main hub. This get the feedback from the therapyLIROManager to understand what to do in the current section
        TherapyLIROManager.Instance.m_onAdvancingTherapy += PrepareNewSection;
        TherapyLIROManager.Instance.m_onFinishingSetupCurrentSection += UpdatingCurrentScenePreparation;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level Loaded calling TherapyLiroManager");
        TherapyLIROManager.Instance.CheckCurrentSection();
    }

    #region Button Events
    public void mainButtonClicked()
    {
        MadLevel.LoadLevelByName(currLevelToLoad);
    }
    #endregion

    #region Methods for communinating with main therapy
    public IEnumerator ChangeScene(TherapyLadderStep currAdvanceStep)
    {
        currLadderStep = currAdvanceStep;
        //Just updating the UI
        switch (currAdvanceStep)
        {
            case TherapyLadderStep.CORE:
                m_currentSectionText.text = "Therapy cycle";
                break;
            case TherapyLadderStep.ACT:
                m_currentSectionText.text = "ACT time!";
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(3);

        switch (currAdvanceStep)
        {
            case TherapyLadderStep.CORE:
                currLevelToLoad = "Basket Selection";
                m_continueButton.interactable = true;
                break;
            case TherapyLadderStep.ACT:
                currLevelToLoad = "ACT";
                break;
            default:
                break;
        }
    }

    void PrepareNewSection(TherapyLadderStep currAdvancedStep)
    {
        currLadderStep = currAdvancedStep;
        switch (currAdvancedStep)
        {
            case TherapyLadderStep.CORE:
                StartCoroutine(ChangeScene(currAdvancedStep));
                break;
            case TherapyLadderStep.ACT:
                m_currentSectionText.text = "ACT time!";
                break;
            default:
                break;
        }
    }

    void UpdatingCurrentScenePreparation(TherapyLadderStep currStep, int amount)
    {
        currLadderStep = currStep;
        switch (currStep)
        {
            case TherapyLadderStep.CORE:
                currLevelToLoad = "World Map Select";
                m_continueButton.interactable = true;
                break;
            case TherapyLadderStep.ACT:
                m_feedbackACT.UpdateText(amount);
                if (amount == 100)
                {
                    currLevelToLoad = "ACT";
                    m_continueButton.interactable = true;
                }                    
                break;
            default:
                break;
        }
    }

    #endregion

}
