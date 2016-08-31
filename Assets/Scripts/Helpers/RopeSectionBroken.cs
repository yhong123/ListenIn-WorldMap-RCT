using UnityEngine;
using System.Collections;

public class RopeSectionBroken : MonoBehaviour {

	[SerializeField]
	private bool canBreak;
	public bool CanBreak {
		get{ return canBreak;}
		set{ canBreak = value;}
	}

	// Use this for initialization
	void Awake () {
		canBreak = false;
	}

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Levels/GreatCanyon/Ropereak2";
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx,aci);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Coin")
		{
			canBreak = true;
			PlaySound();
		}
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Coin")
		{
			canBreak = true;
		}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "Coin")
		{
			//canBreak = false;
		}
	}

	public void BrokeJoint()
	{
		gameObject.GetComponent<HingeJoint2D>().enabled = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
