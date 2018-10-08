using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ACT_UI : MonoBehaviour {

    public Text m_baseText;
    public GameObject m_Icon;
    public GameObject standardAct;
    public GameObject scoreAct;
    public Text currScoreText;
    public Text prevScoreText;
    public List<RectTransform> m_iconTransformList = new List<RectTransform>();
    public Button m_continueButton;
    public Button m_closeScoreButton;

    private string text_format = "Loading... {0}%";

    private int currAmount = 0;
    private int previousAmount = 0;

    public IEnumerator HideBoard()
    {
        m_closeScoreButton.interactable = false;
        yield return new WaitForSeconds(1);
        standardAct.SetActive(false);
        scoreAct.SetActive(false);
        yield return new WaitForSeconds(2);
    }

    public IEnumerator SetScore(int currScore, int previousScore)
    {
        m_closeScoreButton.interactable = false;
        standardAct.SetActive(false);
        scoreAct.SetActive(true);
        currScoreText.text = currScore.ToString();
        prevScoreText.text = previousScore.ToString();

        yield return new WaitForSeconds(3);

        m_closeScoreButton.interactable = true;
    }

    public void UpdateText(int amount)
    {
        StopCoroutine(UpdateProgressInUI());
        currAmount = amount;
        StartCoroutine(UpdateProgressInUI());
    }

    public void UpdateIcon(float duration, int currStep)
    {
        m_Icon.transform.localPosition = m_iconTransformList[currStep - 1].localPosition;
        if (currStep == 1)
            return;

        iTween.Init(m_Icon);
        iTween.MoveTo(m_Icon, iTween.Hash("position", m_iconTransformList[currStep].localPosition, "islocal",  true, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
    }

    public void SetIcon(int currStep)
    {
        if (currStep > 0)
        {
            m_Icon.transform.localPosition = m_iconTransformList[currStep].position;
        }
    }

    IEnumerator UpdateProgressInUI()
    {
        int amount = previousAmount;
        while (amount <= currAmount)
        {
            m_baseText.text = string.Format(text_format, amount);
            amount++;
            previousAmount = amount;
            yield return new WaitForSeconds(0.02f);
        }
        previousAmount = currAmount;

        if (previousAmount == 100)
        {
            m_baseText.text = "Press the button";
            m_continueButton.interactable = true;
        }

    }

}
