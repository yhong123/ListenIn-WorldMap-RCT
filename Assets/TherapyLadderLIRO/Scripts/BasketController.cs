using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasketController : MonoBehaviour {

    public int m_basketNumber;
    public bool m_hardMode = false;

    public Color m_NormalColor;
    public Color m_HighlightColor;

    public Image m_Image;

    public Image m_imageEasy;
    public Button m_easyButton;

    public Image m_imageHard;
    public Button m_hardButton;

    // Use this for initialization
    void Start () {
        if (m_Image == null)
        {
            Debug.LogWarning("Need to add the highlight image to the image controller");
        }
    }

    public void SetHighlightColor(bool highlight)
    {
        if (highlight)
        {
            m_Image.enabled = true;
        }
        else
        {
            m_Image.enabled = false;
        }
    }

    public void SetHardMode(bool isEasy)
    {
        if (isEasy)
        {
            m_hardMode = false;
            m_hardButton.interactable = true;
            m_easyButton.interactable = false;
        }
        else
        {
            m_hardMode = true;
            m_hardButton.interactable = false;
            m_easyButton.interactable = true;
        }
    }

}
