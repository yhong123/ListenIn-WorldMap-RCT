using UnityEngine;
using System.Collections;

public class PotAnimatorController : MonoBehaviour {

    public AnimationClip AnimationInfo;
    private Animator _anim;

    private bool enTrigger = true;

    IEnumerator TriggerAnimation()
    {
        enTrigger = false;
        _anim.SetTrigger("ActivateFade");

        yield return new WaitForSeconds(AnimationInfo.length);
        enTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin" && enTrigger == true)
        {
            StartCoroutine(TriggerAnimation());            
        }
    }

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
