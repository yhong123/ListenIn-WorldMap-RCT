using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MadLevelManager;

public class BasketManager : MonoBehaviour {

    private List<int> m_selectedBasket;
    public int max_number_of_selected_basket = 2;

    [SerializeField]
    private Button m_startButton;
    [SerializeField]
    private GameObject m_progressScreen;
    [SerializeField]
    private Text m_progressTherapy;

    private string m_stringProgressBarFormat = "Loading Therapy... {0}%";

	// Use this for initialization
	void Start () {
        
        m_selectedBasket = new List<int>();
        if (m_startButton == null || m_progressScreen == null || m_progressTherapy == null)
            Debug.LogError("Please attach the components to BasketManager");
        m_startButton.interactable = false;
        m_progressScreen.SetActive(false);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_selectedBasket.Clear();
            m_selectedBasket.Add(1);
            m_selectedBasket.Add(2);
            PrepareTherapy();
        }
#endif
    }

    void OnDestroy()
    {
        if (TherapyLIROManager.Instance != null)
            TherapyLIROManager.Instance.m_onUpdateProgress -= UpdateProgressBar;
    }

    public void RegisterBasket(BasketController bc)
    {
        int indexInList = m_selectedBasket.IndexOf(bc.m_basketNumber);
        if (indexInList != -1)
        {
            //Remove
            bc.SetHighlightColor(false);
            m_selectedBasket.RemoveAt(indexInList);
            m_startButton.interactable = false;
        }
        else
        {
            if (m_selectedBasket.Count < max_number_of_selected_basket)
            {
                bc.SetHighlightColor(true);
                m_selectedBasket.Add(bc.m_basketNumber);
                if(m_selectedBasket.Count == max_number_of_selected_basket)
                    m_startButton.interactable = true;
            }                    
        }
    }

    public void PrepareTherapy()
    {
        m_progressScreen.SetActive(true);
        TherapyLIROManager.Instance.m_onUpdateProgress += UpdateProgressBar;
        TherapyLIROManager.Instance.StartCoreTherapy(m_selectedBasket);        
    }

    private void UpdateProgressBar(int amount)
    {
        m_progressTherapy.text = string.Format(m_stringProgressBarFormat, amount);
        if (amount == 100)
        {
            //Escaping to the world map select
            StartCoroutine(BackToWorldMap());
        }
    }

    private IEnumerator BackToWorldMap()
    {
        yield return StartCoroutine(TherapyLIROManager.Instance.SaveBasketCompletedInfo());
        yield return new WaitForSeconds(2);
        MadLevel.LoadLevelByName("MainHUB");
    }

}
