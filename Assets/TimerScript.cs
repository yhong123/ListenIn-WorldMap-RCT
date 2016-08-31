using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimerScript : MonoBehaviour {

    public Text _textTotal;
    public Text _textToday;
    
	// Use this for initialization
	void Start () {

        if (_textToday != null && _textToday != null)
        {
            string time = StateChallenge.Instance.GetTotalTherapyTime();
            _textTotal.text = time;
            time = StateChallenge.Instance.GetTodayTherapyTime();
            _textToday.text = time;
        }
        else { Debug.LogError("Please assign therapy texts to the inspector"); }
    }
}
