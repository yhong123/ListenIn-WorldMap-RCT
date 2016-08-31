using UnityEngine;
using System.Collections;

public class UIStats : MonoBehaviour {

    public CoinSpawnerB2_Final spawner;

    public TextMesh coinsLeft;

	void Update () 
    {
        ShowCoinsLeft();
	}

    void ShowCoinsLeft()
    {
        //coinsLeft.gameObject.SetActive(true);
        //coin.gameObject.SetActive(true);
        coinsLeft.text = spawner.ReturnCoinsLeft().ToString();
    }

}
