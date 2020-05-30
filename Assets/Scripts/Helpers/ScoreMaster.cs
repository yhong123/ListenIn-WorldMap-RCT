using UnityEngine;
using System.Collections;

//simple class to persist data across scenes and act as a singleton controller for the score
public static class ScoreMaster {

    private static int coins = 0;
    private static int questions = 0;
    private static int correctAnswers = 0;

    public static void AddCoin(int amount)
    {
        coins += amount;
    }

    public static int GetCoinsEarned()
    {
        return coins;
    }

    public static void SetQuestionAmount(int amount)
    {
        questions = amount;
    }

    public static int GetQuestionAmount()
    {
        return questions;
    }

    public static void CorrectAnswer()
    {
        correctAnswers++;
    }

    public static int GetCorrectAnswers()
    {
        return correctAnswers;
    }

	public static void Reset()
	{
		coins = 0;
		questions = 0;
		correctAnswers = 0;
	}

}
