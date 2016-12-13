using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts
//{
class CUser_TherapyBlock
{
    //public string m_strStartTime = "";
    //public string m_strEndTime = "";

    public DateTime m_dtStartTime = new DateTime();
    public DateTime m_dtEndTime = new DateTime();

    public int m_intRoundIdx = 0;

    //public double m_dLinguisticCateogryComplexity = 0; // 0 = single word/phrase, 1 = sentence
    public int m_intLinguisticType = 0; // 0 = single word/phrase, 1 = easy sentence, 2 = hard sentence
                                        //public double m_dNoiseLevelComplexity = 0;  // 0 = no noise, 1 = phn voice, 2 = 5% noise, 3 = 10% noise, 4 = 15% noise, 5 = 20% noise
    public int m_intNoiseLevel = 0;  // 0 = no noise, 1 = phn voice, 2 = 5% noise, 3 = 10% noise, 4 = 15% noise, 5 = 20% noise
    public int m_intDiversityNum = 0;
    public int m_intBlockType = 0;   // 0 = normal block, 1 = forced block

    public List<int> m_lsChallengeItemFeaturesIdx = new List<int>();
    public List<double> m_lsChallengeItemFeatures_Complexity = new List<double>();
    public List<int> m_lsIsDiversity = new List<int>();
    public List<int> m_lsResponseAccuracy = new List<int>();
    public List<double> m_lsResponseRtSec = new List<double>();  // 2016-12-06
    public double m_dAccuracyRate = 0;
    public double m_dBlockComplexity = 0;
    public double m_dUserAbility_Accumulated = 0;  // accumulated user ability
                                                   // 2016-07-23
    public double m_dUserAbility_ThisBlockAvg = 0;  // average user ability in this block based on the complexity of correct responses
    public double m_dTrainingStep = 0.001;  // fixed - 0.01 / -0.01
    public double m_dNextBlock_DiversityThresholdUpper = 0.01;  // based on dataset's std deviation
    public double m_dNextBlock_DiversityThresholdLower = 0.01;

    public double m_dMean_Frequency = 0;
    public double m_dMean_Concreteness = 0;
    public double m_dMean_DistractorNum = 0;

    public double m_dStdDeviation_Frequency = 0;
    public double m_dStdDeviation_Concreteness = 0;
    public double m_dStdDeviation_DistractorNum = 0;

    //----------------------------------------------------------------------------------------------------
    // CTherapyBlock
    //----------------------------------------------------------------------------------------------------
    public CUser_TherapyBlock()
    {
    }
}
//}
