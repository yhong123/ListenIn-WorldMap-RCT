using UnityEngine;
using System.Collections;

public class CoinDestroyer : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        if (this.enabled && other.gameObject.tag == "Coin")
        {

            other.gameObject.GetComponent<CoinMono>().ImmediateDestroy(0.8f);
            //GameObject.DestroyObject(other.gameObject, 0.2f);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
