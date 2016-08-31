using UnityEngine;
using System.Collections;

public class MovingParts : MonoBehaviour {
    public float scrollSpeed;
    //public Texture clouds;
    public Renderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float offset = Time.time * scrollSpeed;
        //rend.material.mainTextureOffset = new Vector2(-offset,0);
        rend.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
       
    }
}
