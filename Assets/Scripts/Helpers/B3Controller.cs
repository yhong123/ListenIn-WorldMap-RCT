using UnityEngine;
using System.Collections;

public class B3Controller : MonoBehaviour {

    //quick way of setting up an end screen grabbing data from the score master
    public TextMesh questions, coins;

    private float countRate = 0.2f;
    private float nextCount;
    private int correctQuestionCount = 0;
    private int totalCoins = 0;

    void Update()
    {
        CountCorrectQuestions();
        CountCoins();
    }

    //display the questions and coins in a counting manner
    void CountCorrectQuestions()
    {
        while (correctQuestionCount < ScoreMaster.GetCorrectAnswers() && Time.time > nextCount)
        {
            nextCount = Time.time + countRate;
            correctQuestionCount++;
        }

        questions.text = "Words " + correctQuestionCount + "/" + ScoreMaster.GetQuestionAmount();
    }

    void CountCoins()
    {
        while (totalCoins < ScoreMaster.GetCoinsEarned() && Time.time > nextCount)
        {
            nextCount = Time.time + countRate/2f;
            totalCoins++;
        }

        coins.text = "Coins " + totalCoins;
    }

	void OnGUI () 
	{
		// set up font size based on screen resolution
		float fDefaultResolutionHeight = 800; 
		float fDefaultFontSize = 22; 
		int intFontSize = (int)(Screen.height / fDefaultResolutionHeight  * fDefaultFontSize);
		
		// button style
		GUIStyle styleBtn = new GUIStyle(GUI.skin.button);
		styleBtn.fontSize = intFontSize; 
		
		// set up button width & height relative to screen size
		float fBtnWidth = Screen.width / 12;
		float fBtnHeight = Screen.height / 20;
		
		// show restart button at the end of game
		{
			Rect rectBtn = new Rect(Screen.width - Screen.width/10, Screen.height/30, fBtnWidth, fBtnHeight);
			if (GUI.Button (rectBtn, "Restart", styleBtn)) {
				ScoreMaster.Reset();
                //Application.LoadLevel("B1_Final");
                GameController.Instance.ChangeState(GameController.States.StatePinball);
			}
		}
	}
	
}
