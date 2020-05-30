using UnityEngine;
using System.Collections;

public class CoinSpawnerB1_Final : MonoBehaviour {

    public GameObject coin;
    public GameObject spawningEffect;

    private int coinsSpawned = 0;

    private float m_Countdown = 0f;
    private float m_Reset = .10f;

    void Update()
    {
        //check with our score master and see if we've spawned enough coins
        if (StateChallenge.Instance.GetCoinsEarned() > coinsSpawned)
        {
            if (m_Countdown <= 0f)
            {
                SpawnCoin();
                m_Countdown = m_Reset;
            }
            else
            {
                m_Countdown -= Time.deltaTime;
            }
        }
    }

    //method to spawn a coin
    void SpawnCoin()
    {
        GameObject coinGO = GameObject.Instantiate(coin, transform.position, Quaternion.identity) as GameObject;
        SpriteRenderer renderer = coinGO.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = 1;
        coinGO.transform.SetParent(this.transform,true);
		coinGO.transform.localScale = new Vector3(0.8f, 0.8f);

        CircleCollider2D collider = coinGO.GetComponent<CircleCollider2D>();
        collider.radius = 0.47f;

        GameObject effect = GameObject.Instantiate(spawningEffect, transform.position, Quaternion.identity) as GameObject;
        effect.SetActive(true);

        coinsSpawned++;
    }
}
