using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys
//{
class CLexicalItem
{
    public int m_intId = 0;
    public string m_strName = "";
    //public int m_intFrequency = 0;
    //public int m_intConcreteness = 0;
    //public int m_intSyllableNum = 0;
    public List<int> m_lsChallengeItemIdx = new List<int>();

    //----------------------------------------------------------------------------------------------------
    // CLexicalItem
    //----------------------------------------------------------------------------------------------------
    public CLexicalItem(CLexicalItem item)
    {
        m_intId = item.m_intId;
        m_strName = item.m_strName;
        //m_intFrequency = item.m_intFrequency;
        //m_intConcreteness = item.m_intConcreteness;
        //m_intSyllableNum = item.m_intSyllableNum;
        for (var i = 0; i < item.m_lsChallengeItemIdx.Count; i++)
        {
            m_lsChallengeItemIdx.Add(item.m_lsChallengeItemIdx[i]);
        }
    }

    //----------------------------------------------------------------------------------------------------
    // CLexicalItem
    //----------------------------------------------------------------------------------------------------
    public CLexicalItem()
    {
    }

    //----------------------------------------------------------------------------------------------------
    // initItem
    //----------------------------------------------------------------------------------------------------
    public void initItem(int id, string name, int freq, int conc, int syll, List<int> lsChallengeItemIdx)
    {
        m_intId = id;
        m_strName = name;
        //m_intFrequency = freq;
        //m_intConcreteness = conc;
        //m_intSyllableNum = syll;
        for (var i = 0; i < lsChallengeItemIdx.Count; i++)
        {
            m_lsChallengeItemIdx.Add(lsChallengeItemIdx[i]);
        }
    }

    //----------------------------------------------------------------------------------------------------
    // updateChallengeItemIdx
    //----------------------------------------------------------------------------------------------------
    public void updateChallengeItemIdx(List<int> lsChallengeItemIdx)
    {
        for (var i = 0; i < lsChallengeItemIdx.Count; i++)
            m_lsChallengeItemIdx.Add(lsChallengeItemIdx[i]);
    }
}
//}
