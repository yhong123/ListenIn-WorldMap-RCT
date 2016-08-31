using UnityEngine;
using System.Collections;

public class AutoParticleDestroyer : MonoBehaviour {

    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if (!ps.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
