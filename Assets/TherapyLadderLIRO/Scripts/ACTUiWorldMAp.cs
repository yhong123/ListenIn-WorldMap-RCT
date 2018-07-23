using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MadLevelManager;

public class ACTUiWorldMAp : MonoBehaviour {

    public Text m_baseText;
    public GameObject m_startButton;

    private string text_format = "Loading... {0}%";

    private int currAmount = 0;
    private int previousAmount = 0;

    public void ActivateUI()
    {
        this.gameObject.SetActive(true);
    }

    public void UpdateText(int amount)
    {
        StopCoroutine(UpdateProgressInUI());
        currAmount = amount;
        StartCoroutine(UpdateProgressInUI());
    }

    public void StartACT()
    {
        MadLevel.LoadLevelByName("ACT");
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
            m_baseText.text = "Press the button to proceed";
            if (m_startButton != null)
                m_startButton.SetActive(true);
        }

    }

}
