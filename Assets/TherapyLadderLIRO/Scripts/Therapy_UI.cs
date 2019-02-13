using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Therapy_UI : MonoBehaviour {

    private string CycleFormat = "Cycle #{0}";
    private string TherapyFormat = "Total Therapy Time: {0}h {1}m";
    private string GameFormat = "Total Game Time: {0}h {1}m";
    private string PercentageFormat = "Completed: {0}%";
    public Text cycleText;
    public Text therapyText;
    public Text gameText;
    public Text percentageText;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateUserStats(UserProfileManager profile)
    {
        cycleText.text = string.Format(CycleFormat, profile.m_userProfile.m_cycleNumber);

        int hours, mins;
        hours = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalTherapyMinutes / 60;
        mins = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalTherapyMinutes % 60;
        therapyText.text = string.Format(TherapyFormat, hours, mins);

        hours = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalGameMinutes / 60;
        mins = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalGameMinutes % 60;
        gameText.text = string.Format(GameFormat, hours, mins);

        //Calculating percentage
        float perc = (float)(profile.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock - 1) / (float)profile.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks * 100.0f;
        if (perc > 100.0f || perc == -Mathf.Infinity || profile.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock > profile.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks)
            perc = 0.0f;

        percentageText.text = string.Format(PercentageFormat, perc.ToString("f2"));

    }
}
