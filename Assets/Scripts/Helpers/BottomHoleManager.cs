using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BottomHoleManager : MonoBehaviour {

	public GameObject LeversRoot;
	public GameObject ActivatorsRoot;

	private LeverActivator[] levers;
	private ButtonsActivator[] activators;

	private bool activatorsOn;
	private bool previousOn;

	// Use this for initialization
	void Start () {
		levers = LeversRoot.GetComponentsInChildren<LeverActivator>();
		activators = ActivatorsRoot.GetComponentsInChildren<ButtonsActivator>();
		activatorsOn = false;
	}

	public void PlaySound(bool active)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;

		string strAudio = active ? "Levels/FancyIsland/FlipperOpen" : "Levels/FancyIsland/FlipperClose" ;

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx, aci);
	}
	
	// Update is called once per frame
	void Update () {
	
		previousOn = activatorsOn;

    	activatorsOn = false; 
		if(activators != null && activators.Length != 0)
		{
			for (int i = 0; i < activators.Length; i++) {
				if(activators[i].Actived)
				{
					activatorsOn = true;
				}
				else
				{
					activatorsOn = false;
					break;
				}
			}
		}

		if(levers != null && levers.Length != 0)
		{
			for (int i = 0; i < levers.Length; i++) {
				levers[i].Open = activatorsOn;
			}
		}

		if(previousOn != activatorsOn)
		{
			PlaySound(activatorsOn);
		}
	}
}
 