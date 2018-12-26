using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasketController : MonoBehaviour {

    public int m_basketNumber;
    public bool m_hardMode = false;

    public Color m_NormalColor;
    public Color m_HighlightColor;

    public Image m_Image;

    public GameObject m_slider;
    public Vector3 positionEasy;
    public Vector3 positionHard;

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

    public void SetHardMode()
    {
        if (!m_hardMode)
        {
            m_hardMode = true;
            m_slider.GetComponent<RectTransform>().localPosition = positionHard;
            
        }
        else
        {
            m_hardMode = false;
            m_slider.GetComponent<RectTransform>().localPosition = positionEasy;
        }
    }

}
