using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts.TherapyItemRecSys
//{
class CRecCandidate
{
    public int m_intChallengeItemFeaturesIdx = 0;

    public int m_intNeighbourForCtr = 0;

    public double m_dSimilarityStrength = 0;        // calculate from items' similarities

    public double m_dCurComplexity = 0;   // dItemCurComplexity* dChallengeItemCurComplexity * dLexicalItemCurComplexity

    public double m_dComplexityWeight = 0;

    public double m_dNeighbourWeight = 0;

    public double m_dExposureWeight = 0;

    public double m_dRecommendationStrength = 0;  // m_dSimilarityStrength + m_dNeighbourWeight + m_dExposureWeight


    //----------------------------------------------------------------------------------------------------
    // CRecCandidate
    //----------------------------------------------------------------------------------------------------
    public CRecCandidate()
    {
    }

    //----------------------------------------------------------------------------------------------------
    // CRecCandidate
    //----------------------------------------------------------------------------------------------------
    public CRecCandidate(CRecCandidate item)
    {
        m_intChallengeItemFeaturesIdx = item.m_intChallengeItemFeaturesIdx;
        m_intNeighbourForCtr = item.m_intNeighbourForCtr;
        m_dSimilarityStrength = item.m_dSimilarityStrength;
        m_dCurComplexity = item.m_dCurComplexity;
        m_dComplexityWeight = item.m_dComplexityWeight;
        m_dNeighbourWeight = item.m_dNeighbourWeight;
        m_dExposureWeight = item.m_dExposureWeight;
        m_dRecommendationStrength = item.m_dRecommendationStrength;
    }
}
//}
