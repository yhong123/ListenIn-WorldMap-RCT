using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts
//{
class CChallengeItemFeatures
{
    public int m_intChallengeItemIdx = 0;

    public string m_strName = "";

    public int m_intFrequency = 0;
    public int m_intConcreteness = 0;
    public int m_intDistractorNum = 0;
    public int m_intLinguisticCategory = 0; // refer CConstant.g_LinguisticCategory - noun / pronoun / adj etc
    public int m_intLinguisticType = 0; // refer CConstant.g_LinguisticType - word / easy sentence / hard sentence
    public int m_intNoiseLevel = 0; // 0 = no noise, 1 = phn voice, 2 = 5% noise, 3 = 10% noise, 4 = 15% noise, 5 = 20% noise  

    public double m_dComplexity_Frequency = 0;
    public double m_dComplexity_Concreteness = 0;
    public double m_dComplexity_DistractorNum = 0;
    public double m_dComplexity_LinguisticType = 0;
    public double m_dComplexity_NoiseLevel = 0;
    public double m_dComplexity_Overall = 0;

    //----------------------------------------------------------------------------------------------------
    // CChallengeItemFeatures
    //----------------------------------------------------------------------------------------------------
    public CChallengeItemFeatures(CChallengeItemFeatures features)
    {
        m_intChallengeItemIdx = features.m_intChallengeItemIdx;
        m_strName = features.m_strName;

        m_intFrequency = features.m_intFrequency;
        m_intConcreteness = features.m_intConcreteness;
        m_intDistractorNum = features.m_intDistractorNum;
        m_intLinguisticCategory = features.m_intLinguisticCategory;
        m_intLinguisticType = features.m_intLinguisticType;
        m_intNoiseLevel = features.m_intNoiseLevel;

        m_dComplexity_Frequency = features.m_dComplexity_Frequency;
        m_dComplexity_Concreteness = features.m_dComplexity_Concreteness;
        m_dComplexity_DistractorNum = features.m_dComplexity_DistractorNum;
        m_dComplexity_LinguisticType = features.m_dComplexity_LinguisticType;
        m_dComplexity_NoiseLevel = features.m_dComplexity_NoiseLevel;
        m_dComplexity_Overall = features.m_dComplexity_Overall;

    }

    //----------------------------------------------------------------------------------------------------
    // CChallengeItemFeatures
    //----------------------------------------------------------------------------------------------------
    public CChallengeItemFeatures()
    {

    }

}
//}
