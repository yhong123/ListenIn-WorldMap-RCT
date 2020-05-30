using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class HorizontalScrollSnap : MonoBehaviour
{
    private bool /*This really*/ m_IsDragging;

    private ScrollRect m_ScrollRect;
    private GridLayoutGroup m_Grid;

    private RectTransform m_ContentRect;

    private float m_Intertia;

    void Start()
    {
        m_ScrollRect = this.GetComponent<ScrollRect>();

        m_Intertia = m_ScrollRect.decelerationRate;
        m_ScrollRect.inertia = false;

        m_Grid = m_ScrollRect.content.GetComponent<GridLayoutGroup>();
        m_ContentRect = m_ScrollRect.content.GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (m_IsDragging)
        {
            float dist = Mathf.Infinity;
            float width = m_Grid.cellSize.x;
            float snapTo = 0;

            for (int i = 0; i < m_Grid.transform.childCount; i++)
            {
                float gridSnap = ((float)width * i) - ((float)width * ((float)(m_Grid.transform.childCount - 1) * 0.5f));
                float curDist = Mathf.Abs(m_Grid.transform.localPosition.x - gridSnap);
                if (curDist < dist)
                {
                    dist = curDist;
                    snapTo = gridSnap;
                }
            }
            Vector2 newPos = new Vector2(snapTo, m_Grid.transform.localPosition.y);
            m_Grid.transform.localPosition = Vector2.Lerp(m_Grid.transform.localPosition, newPos, m_Intertia);
        }
    }


    public void Drag()
    {
    }
    public void DragEnd()
    {
    }
}