using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasketController : MonoBehaviour {

    public int m_basketNumber;
    public bool m_hardMode;

    public Color m_NormalColor;
    public Color m_HighlightColor;

    public Image m_Image;

	// Use this for initialization
	void Start () {
        m_Image = GetComponent<Image>();
    }

    public void SetHighlightColor(bool highlight)
    {
        if (highlight)
        {
            m_Image.color = m_HighlightColor;
        }
        else
        {
            m_Image.color = m_NormalColor;
        }
    }

}
