using UnityEngine;
using System.Collections;

public class UserProfile
{

    private TherapyLadderStep m_LIROTherapyStep;
    public TherapyLadderStep LIROStep {
        set {
            m_LIROTherapyStep = value;
            m_TherapyLadderStepID = (int)m_LIROTherapyStep;
        } get { return m_LIROTherapyStep; }
    }

    private int m_TherapyLadderStepID;
    public int m_currIDUser;

    public void SaveToPrefs()
    {
        PlayerPrefs.SetInt("CurrentUser", m_currIDUser);
        PlayerPrefs.SetString("TherapyLadderStep", m_LIROTherapyStep.ToString());
        PlayerPrefs.SetInt("TherapyLadderStep", (int)m_LIROTherapyStep);
    }

    public void GetFromPrefs()
    {
        
    }

}
