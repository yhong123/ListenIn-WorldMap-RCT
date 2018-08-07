using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using MadLevelManager;

public class AppControllerMainHUB : MonoBehaviour {

    public Button m_continueButton;
    public Text m_currentSectionText;
    public ACT_UI m_ACT_ui;
    public Therapy_UI m_Therapy_ui;

    private TherapyLadderStep currLadderStep;
    private string currLevelToLoad = "";

    void Awake()
    {
        //AndreaLIRO: goes into the main hub. This get the feedback from the therapyLIROManager to understand what to do in the current section
        //The first event is used when detecting switches
        TherapyLIROManager.Instance.m_OnUpdatingCurrentSection += UpdateCurrentSection;
        TherapyLIROManager.Instance.m_OnSwitchingSection += SwitchingSection;
        TherapyLIROManager.Instance.m_OnEndingSection += OnEndingSection;
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
        if (TherapyLIROManager.Instance != null)
        {
            TherapyLIROManager.Instance.m_OnUpdatingCurrentSection -= UpdateCurrentSection;
            TherapyLIROManager.Instance.m_OnSwitchingSection -= SwitchingSection;
            TherapyLIROManager.Instance.m_OnEndingSection -= OnEndingSection;
        }
    }

    #region Button Events
    public void mainButtonClicked()
    {
        MadLevel.LoadLevelByName(currLevelToLoad);
    }
    #endregion

    #region DelegateMethods
    void UpdateCurrentSection(UserProfileManager currProfile, int amount)
    {
        switch (currProfile.LIROStep)
        {
            case TherapyLadderStep.CORE:
                m_currentSectionText.text = "Therapy cycle";
                m_Therapy_ui.gameObject.SetActive(true);
                m_continueButton.enabled = true;
                currLevelToLoad = "World Map Select";
                m_continueButton.interactable = true;
                break;
            case TherapyLadderStep.ACT:
                m_currentSectionText.text = "ACT";
                m_ACT_ui.gameObject.SetActive(true);
                m_ACT_ui.UpdateIcon(2,currProfile.m_userProfile.m_ACTLiroUserProfile.m_currentBlock);
                currLevelToLoad = "ACT";
                m_continueButton.interactable = true;
                break;
            default:
                break;
        }
    }

    void SwitchingSection(UserProfileManager currProfile, int amount)
    {
        if (amount == 0)
        {
            m_ACT_ui.gameObject.SetActive(false);
            m_Therapy_ui.gameObject.SetActive(false);
            //Activate the current UI
            switch (currProfile.LIROStep)
            {
                case TherapyLadderStep.CORE:
                    m_currentSectionText.text = "Therapy cycle";
                    m_Therapy_ui.gameObject.SetActive(true);
                    currLevelToLoad = "Basket Selection";
                    m_continueButton.interactable = true;
                    break;
                case TherapyLadderStep.ACT:
                    m_currentSectionText.text = "ACT";
                    m_ACT_ui.gameObject.SetActive(true);
                    currLevelToLoad = "ACT";
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (currProfile.LIROStep)
            {
                case TherapyLadderStep.CORE:
                    break;
                case TherapyLadderStep.ACT:
                    m_ACT_ui.UpdateText(amount);
                    break;
                default:
                    break;
            }
        }
    }

    void OnEndingSection(UserProfileManager currProfile)
    {
        switch (currProfile.LIROStep)
        {
            case TherapyLadderStep.CORE:
                m_currentSectionText.text = "Therapy cycle";
                m_Therapy_ui.gameObject.SetActive(true);
                StartCoroutine(EndTherapy(currProfile));
                break;
            case TherapyLadderStep.ACT:
                m_currentSectionText.text = "ACT";
                m_ACT_ui.gameObject.SetActive(true);
                StartCoroutine(EndACT(currProfile));
                break;
            default:
                break;
        }
    }

    private IEnumerator EndTherapy(UserProfileManager currProfile)
    {
        yield return new WaitForSeconds(2);
        TherapyLIROManager.Instance.GoToNextSection();
    }
    private IEnumerator EndACT(UserProfileManager currProfile)
    {
        m_ACT_ui.UpdateIcon(2, currProfile.m_userProfile.m_ACTLiroUserProfile.m_currentBlock);
        yield return new WaitForSeconds(2);

        //AndreaLIRO: add other animations here... then back to the Therapy Manager to change section

        TherapyLIROManager.Instance.GoToNextSection();

    }
    #endregion

}
