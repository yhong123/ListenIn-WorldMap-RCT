using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys
//{
class CChallengeItem
{
    public int m_intId = 0;
    public int m_intLexicalItemIdx = 0;
    public string m_strName = "";
    public int m_intFrequency = 0;
    public int m_intConcreteness = 0;
    public int m_intSyllableNum = 0;

    //public int m_intDistractorNum = 0;
    public int m_intLinguisticType = 0; // 0 = single word/phrase, 1 = sentence
    public string m_strLinguisticCategoryName = "";
    //public int m_intNoiseLevel = 0; // 0 = no noise, 1 = phn voice, 2 = 5% noise, 3 = 10% noise, 4 = 15% noise, 5 = 20% noise  

    //public string m_strAudioFile = "";   // audio of the target stimulus
    public List<string> m_lsAudioFile = new List<string>();
    public int m_intTargetIdx = 0;  // index of the target stimulus in lsPicture
    public List<CPictureChoice> m_lsPictureChoice = new List<CPictureChoice>();

    public List<int> m_lsChallengeItemFeaturesIdx = new List<int>();

    public int m_intForcedItem = 0; // 0 = non-forced item, 1 = forced item

    //----------------------------------------------------------------------------------------------------
    // CChallengeItem
    //----------------------------------------------------------------------------------------------------
    public CChallengeItem(CChallengeItem item)
    {
        m_intId = item.m_intId;
        m_intLexicalItemIdx = item.m_intLexicalItemIdx;
        m_strName = item.m_strName;
        m_intFrequency = item.m_intFrequency;
        m_intConcreteness = item.m_intConcreteness;
        m_intSyllableNum = item.m_intSyllableNum;

        //m_intDistractorNum = item.m_intDistractorNum;
        m_intLinguisticType = item.m_intLinguisticType;
        m_strLinguisticCategoryName = item.m_strLinguisticCategoryName;
        //m_intNoiseLevel = item.m_intNoiseLevel;

        //m_strAudioFile = item.m_strAudioFile;   // audio of the target stimulus
        for (var i = 0; i < item.m_lsAudioFile.Count; i++)
            m_lsAudioFile.Add(item.m_lsAudioFile[i]);

        m_intTargetIdx = item.m_intTargetIdx;  // index of the target stimulus in lsStimulus
        for (var i = 0; i < item.m_lsPictureChoice.Count; i++)
        {
            CPictureChoice picChoice = new CPictureChoice();
            picChoice.m_strName = item.m_lsPictureChoice[i].m_strName;
            picChoice.m_strImageFile = item.m_lsPictureChoice[i].m_strImageFile;
            picChoice.m_strType = item.m_lsPictureChoice[i].m_strType;
            m_lsPictureChoice.Add(picChoice);
        }

        for (var i = 0; i < item.m_lsChallengeItemFeaturesIdx.Count; i++)
        {
            m_lsChallengeItemFeaturesIdx.Add(item.m_lsChallengeItemFeaturesIdx[i]);
        }
    }


    //----------------------------------------------------------------------------------------------------
    // CChallengeItem
    //----------------------------------------------------------------------------------------------------
    public CChallengeItem()
    {
    }

    //----------------------------------------------------------------------------------------------------
    // initItem
    //----------------------------------------------------------------------------------------------------
    public void initItem(int id, string target, int freq, int conc, int syll)
    {
        /*m_intId = id;
        m_strName = target;
        m_strLinguisticCategory = "";
        m_intFrequency = freq;
        m_intConcreteness = conc;
        m_intSyllable = syll;
        m_intDistractorNum = 0;
        m_strAudioFile = "";   // audio of the target stimulus
        m_intTargetIdx = 0;*/
    }

    //----------------------------------------------------------------------------------------------------
    // updateChallengeItemFeaturesIdx
    //----------------------------------------------------------------------------------------------------
    public void updateChallengeItemFeaturesIdx(List<int> lsChallengeItemFeaturesIdx)
    {
        for (var i = 0; i < lsChallengeItemFeaturesIdx.Count; i++)
            m_lsChallengeItemFeaturesIdx.Add(lsChallengeItemFeaturesIdx[i]);
    }
}

//************************************************************************************************************************
// CLASS- CPictureChoice
//************************************************************************************************************************
public class CPictureChoice
{
    // each picture choice has a associated name and image file
    public string m_strName;
    public string m_strImageFile;
    public string m_strType;  // target / phonological / semantic / unrelated
    public string m_strPType;  // target / distant / close / mp / assoc / un

    //----------------------------------------------------------------------------------------------------
    // CPictureChoice
    //----------------------------------------------------------------------------------------------------
    public CPictureChoice()
    {
        m_strName = "";
        m_strImageFile = "";
        m_strType = "";
        m_strPType = "";
    }
}

//}
