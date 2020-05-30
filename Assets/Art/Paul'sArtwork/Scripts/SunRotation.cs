using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour {
    public GameObject Sun = null;
    public float rotation = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Sun.transform.Rotate(new Vector3(0, 0, -rotation));	
	}
}
