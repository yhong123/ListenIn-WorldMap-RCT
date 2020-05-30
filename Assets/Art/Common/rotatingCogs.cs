using UnityEngine;
using System.Collections;

public class rotatingCogs : MonoBehaviour {
    public GameObject cog;
    public float speed = 2.0f;
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    { 
      cog.transform.Rotate(new Vector3(0, 0, 2.0f * Time.deltaTime * speed)); 
    }
}
