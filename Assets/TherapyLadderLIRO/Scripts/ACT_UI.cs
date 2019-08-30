using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ACT_UI : MonoBehaviour {

    public Text m_baseText;
    //public GameObject m_Icon;
    public GameObject standardAct;
    public GameObject scoreAct;
    public Text currScoreText;
    public Text prevScoreText;
    //public List<RectTransform> m_iconTransformList = new List<RectTransform>();
    public Button m_continueButton;
    public Button m_closeScoreButton;

    public Image m_circularProgression;
    public Image[] m_filledPieces;
    public Text m_scoreText;

    private string text_format = "Loading... {0}%";

    private int currAmount = 0;
    private int previousAmount = 0;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void HideInteralUI()
    {
        standardAct.gameObject.SetActive(false);
        standardAct.gameObject.SetActive(false);
    }

    public void SetACTStardardBoard()
    {
        HideInteralUI();
        standardAct.gameObject.SetActive(true);

    }

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
        HideInteralUI();
        m_closeScoreButton.interactable = false;
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
        StartCoroutine(SetCircularBar(1.4f, currStep));

        if (currStep > 0)
        {
        }
        //    m_Icon.transform.localPosition = m_iconTransformList[currStep - 1].localPosition;
        //    if (currStep == 1)
        //        return;

        //    iTween.Init(m_Icon);
        //    iTween.MoveTo(m_Icon, iTween.Hash("position", m_iconTransformList[currStep].localPosition, "islocal",  true, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
    }

    private void SetIcon(int currStep)
    {
        Debug.LogError("Method not implemented");
        if (currStep > 0)
        {
            //m_Icon.transform.localPosition = m_iconTransformList[currStep].position;
        }
    }

    private IEnumerator SetCircularBar(float speed, int step)
    {
        yield return new WaitForEndOfFrame();
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * speed * 1.5f;
            m_circularProgression.color = Color.Lerp(startColor, endColor, t);
            yield return new WaitForEndOfFrame();
        }

        m_circularProgression.color = endColor;
        yield return new WaitForSeconds(0.4f);

        t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * speed * 1.5f;
            for (int i = 0; i < step - 1; i++)
            {
                m_filledPieces[i].color = Color.Lerp(startColor, endColor, t);
            }
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < step -1; i++)
        {
            m_filledPieces[i].color = endColor;
        }
        yield return new WaitForEndOfFrame();

        yield return new WaitForSeconds(0.5f);
        m_scoreText.text = string.Format("{0}/8", (step - 1).ToString());

        yield return new WaitForSeconds(1.0f);
        m_baseText.text = "Please, press play";
        yield return new WaitForEndOfFrame();

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
            StartCoroutine(SetCircularBar(1.3f,0));
            yield return new WaitForSeconds(1.5f);
            m_baseText.text = "Please, press play";
            m_continueButton.interactable = true;
        }

    }

}
