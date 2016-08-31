using UnityEngine;
using System.Collections;

public class TestMouseCoinSpawner : MonoBehaviour {
	
	private bool spawn;
	private Vector3 worldposition;
	
	public GameObject coinprefab;
	
	void OnMouseDown(){
		worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		spawn = true;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldposition.z = 0.0f;

			GameObject coin = GameObject.Instantiate(coinprefab,worldposition,Quaternion.identity) as GameObject;
			coin.transform.localScale = Vector3.one * 0.6f;
		}
	}
}
