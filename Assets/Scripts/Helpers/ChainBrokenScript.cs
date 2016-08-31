using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainBrokenScript : MonoBehaviour {

	private List<GameObject> ropeSections;
	private bool brokeTriggered = false;

	// Use this for initialization
	void Start () {
		ropeSections = new List<GameObject>();
		List<Component> components = new List<Component>(gameObject.GetComponentsInChildren(typeof(RopeSectionBroken)));
		ropeSections = components.ConvertAll(c => (GameObject)c.gameObject);
	}

	private bool CheckBrockeCondition()
	{
		if(ropeSections.Count == 0) return false;
		bool ret = true;
		foreach (GameObject item in ropeSections) {
			if(!item.GetComponent<RopeSectionBroken>().CanBreak){
				ret = false;
				break;
			}
		}
		return ret;
	}

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Levels/GreatCanyon/RopeBreak";
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx,aci);
	}

	// Update is called once per frame
	void Update () {
	
		if(CheckBrockeCondition() && !brokeTriggered)
		{
			brokeTriggered = true;
			PlaySound();
			ropeSections[Random.Range(0,ropeSections.Count)].GetComponent<RopeSectionBroken>().BrokeJoint();
		}

	}
}
