using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasketManager : MonoBehaviour {

    private List<int> m_selectedBasket;
    public int max_number_of_selected_basket = 2;

    public int currentSelectedBaskets;

	// Use this for initialization
	void Start () {
        m_selectedBasket = new List<int>();
    }

    void Update()
    {
        currentSelectedBaskets = m_selectedBasket.Count;
    }

    public void RegisterBasket(BasketController bc)
    {
        int indexInList = m_selectedBasket.IndexOf(bc.m_basketNumber);
        if (indexInList != -1)
        {
            //Remove
            bc.SetHighlightColor(false);
            m_selectedBasket.RemoveAt(indexInList);
        }
        else
        {
            if (m_selectedBasket.Count < max_number_of_selected_basket)
            {
                bc.SetHighlightColor(true);
                m_selectedBasket.Add(bc.m_basketNumber);
            }                    
        }
    }

}
