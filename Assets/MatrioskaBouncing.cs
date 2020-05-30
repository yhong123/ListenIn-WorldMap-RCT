using UnityEngine;
using System.Collections;
using MadLevelManager;

public class MatrioskaBouncing : MonoBehaviour {

    public Vector3[] Positions;
    public float completitionTime = 3.0f;
    public iTween.EaseType EasyType;

    private Transform CoinSpawnerTransform;

	// Use this for initialization
	void Start () {
        CoinSpawnerTransform = GameObject.FindGameObjectWithTag("CoinSpawner").transform;
        if(Positions.Length > 1)
            iTween.MoveTo(this.gameObject, iTween.Hash("path", Positions, "time", completitionTime, "easetype", EasyType, "looptype", iTween.LoopType.pingPong));
        else
            iTween.MoveTo(this.gameObject, iTween.Hash("position", Positions[0], "time", completitionTime, "easetype", EasyType, "looptype", iTween.LoopType.pingPong));
    }
	
	// Update is called once per frame
	void LateUpdate () {

	}
}
