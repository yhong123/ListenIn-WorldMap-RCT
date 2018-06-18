using UnityEngine;
using System.Collections;

public class UserProfile
{
    #region User Properties
    public int m_currIDUser;
    private TherapyLadderStep m_LIROTherapyStep;
    private int m_TherapyLadderStepID;
    public int m_currentBlock = 0;
    public int m_current_Total_Blocks = 0;
    #endregion

    public TherapyLadderStep LIROStep
    {
        set
        {
            m_LIROTherapyStep = value;
            m_TherapyLadderStepID = (int)m_LIROTherapyStep;
        }
        get { return m_LIROTherapyStep; }
    }

    #region API
    /// <summary>
    /// PlayerPrefs are used as checkpoints, updated information are saved in the user_profile.xml
    /// </summary>
    public void SaveToPrefs()
    {
        PlayerPrefs.SetInt("CurrentUser", m_currIDUser);
        PlayerPrefs.SetString("TherapyLadderStep", m_LIROTherapyStep.ToString());
        PlayerPrefs.SetInt("TherapyLadderStepID", (int)m_LIROTherapyStep);
        PlayerPrefs.SetInt("CurrentBlockNumber", m_currentBlock);
        PlayerPrefs.SetInt("CurrentTotalBlockNumber", m_current_Total_Blocks);
    }
    public void GetFromPrefs()
    {
        if (PlayerPrefs.HasKey("CurrentUser"))
        {
            m_currIDUser = PlayerPrefs.GetInt("CurrentUser");
        }

        if (PlayerPrefs.HasKey("TherapyLadderStepID"))
        {
            m_TherapyLadderStepID = PlayerPrefs.GetInt("TherapyLadderStepID");
            m_LIROTherapyStep = (TherapyLadderStep)m_TherapyLadderStepID;
        }
    }

    public void SetToPrefs(string key, int number)
    {
        PlayerPrefs.SetInt(key, number);
    }
    public void GetFromPrefs(string key, string text)
    {

        PlayerPrefs.SetString(key, text);

    }
    public void GetFromPrefs(string key, ref string text)
    {
        if (PlayerPrefs.HasKey(key))
        {
            text = PlayerPrefs.GetString(key);
        }
    }
    #endregion

}
