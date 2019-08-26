using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class Therapy_UI : MonoBehaviour {

    private string CycleFormat = "Therapy Cycle #{0}";
    private string TherapyFormat = "  {0}h  {1}m";
    //private string GameFormat = "Total Game Time: {0}h {1}m";
    private string PercentageFormat = "{0}%";
    public Text cycleText;
    public Image clockImage;
    public Text completedText; 
    public Text therapyText;
    //public Text gameText;
    public Image backgroundFill;
    public Image filledProgressBar;
    public Text percentageText;

    public float textSpeed = 0.1f;

    private char[] cycleTextS;
    private char[] therapyTextS;
    //private char[] gameTextS;
    private char[] percentageTextS;

    private float perc;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {

    }

    public void UpdateUserStats(UserProfileManager profile)
    {
        cycleTextS = (string.Format(CycleFormat, profile.m_userProfile.m_cycleNumber)).ToCharArray();

        int hours, mins;
        hours = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalTherapyMinutes / 60;
        mins = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalTherapyMinutes % 60;
        therapyTextS = (string.Format(TherapyFormat, hours, mins)).ToCharArray();

        hours = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalGameMinutes / 60;
        mins = profile.m_userProfile.m_TherapyLiroUserProfile.m_totalGameMinutes % 60;
        //gameTextS = (string.Format(GameFormat, hours, mins)).ToCharArray();

        //Calculating percentage
        perc = (float)(profile.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock - 1) / (float)profile.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks * 100.0f;
        if (perc > 100.0f || perc == -Mathf.Infinity || profile.m_userProfile.m_TherapyLiroUserProfile.m_currentBlock > profile.m_userProfile.m_TherapyLiroUserProfile.m_totalBlocks)
            perc = 0.0f;

        percentageTextS = (string.Format(PercentageFormat, perc.ToString("f2"))).ToCharArray();
        StartCoroutine(UpdateUI());
    }

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="profile"></param>
    public void PrepareBasketScreen(UserProfileManager profile)
    {
        if (profile.m_userProfile.m_cycleNumber == 0)
        {
            cycleTextS = ("Welcome to ListenIn").ToCharArray();
            therapyTextS = ("").ToCharArray();
            //gameTextS = ("").ToCharArray();
            percentageTextS = ("Press the play button to proceed").ToCharArray();
        }
        else
        {
            cycleTextS = ("Ready for the next iteration...").ToCharArray();
            therapyTextS = ("").ToCharArray();
            //gameTextS = ("").ToCharArray();
            percentageTextS = ("Press the play button to proceed").ToCharArray();
        }

        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        yield return StartCoroutine(PrintText(cycleText,cycleTextS));
        yield return StartCoroutine(PrintText(completedText, ("Completed:").ToCharArray()));
        yield return StartCoroutine(ShowImageAlpha(clockImage, 1.5f));
        yield return StartCoroutine(PrintText(therapyText, therapyTextS));
        yield return StartCoroutine(ShowImageAlpha(backgroundFill, 1.8f));
        yield return StartCoroutine(ShowProgressFill(filledProgressBar, 1.5f));
        //yield return StartCoroutine(PrintText(gameText, gameTextS));
        yield return StartCoroutine(PrintText(percentageText, percentageTextS));

    }

    private IEnumerator ShowImageAlpha(Image mImage, float speed)
    {
        Color currColor = mImage.color;
        Color targetColor = currColor;
        targetColor.a = 1.0f;
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime;
            mImage.color = Color.Lerp(currColor, targetColor, t * speed);
            yield return new WaitForEndOfFrame();
        }
        mImage.color = targetColor;
    }

    private IEnumerator ShowProgressFill(Image mImage, float speed)
    {
        
        float t = 0.0f;
        float finalPercentage = perc / 100.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime;
            mImage.fillAmount = Mathf.Lerp(0.0f, finalPercentage, t * speed);
            yield return new WaitForEndOfFrame();
        }
        mImage.fillAmount = finalPercentage;
    }

    private IEnumerator PrintText(Text targetText, char[] targetString)
    {
        targetText.text = string.Empty;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < targetString.Length; i++)
        {
            sb.Append(targetString[i]);
            targetText.text = sb.ToString();
            yield return new WaitForSeconds(textSpeed);
        }
    }

}
