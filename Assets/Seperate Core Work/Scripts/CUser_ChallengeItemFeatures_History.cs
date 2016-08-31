using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Assets.Seperate_Core_Work.Scripts
//{
class CUser_ChallengeItemFeatures_History
{
    public int m_intChallengeItemFeaturesIdx = -1;
    public List<CPresentHistory> m_lsPresentHistory = new List<CPresentHistory>();

    //----------------------------------------------------------------------------------------------------
    // CChallengeItemFeatures_History
    //----------------------------------------------------------------------------------------------------
    public CUser_ChallengeItemFeatures_History()
    {
    }

}

//************************************************************************************************************************
// CLASS- CPresentHistory
//************************************************************************************************************************
public class CPresentHistory
{
    public int m_intRoundIdx;
    public int m_intSessionIdx;
    public int m_intBlockIdx;
    public int m_intAccuracy;  // 0 = incorrect, 1 = correct on first attempt

    //----------------------------------------------------------------------------------------------------
    // CPresentHistory
    //----------------------------------------------------------------------------------------------------
    public CPresentHistory()
    {
        m_intRoundIdx = -1;
        m_intSessionIdx = -1;
        m_intBlockIdx = -1;
        m_intAccuracy = -1;
    }
}
//}
