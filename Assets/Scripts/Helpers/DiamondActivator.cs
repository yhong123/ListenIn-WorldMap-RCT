using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiamondActivator : MonoBehaviour {

	public Light li;
    public int activeLights = 4;
	public List<bool> activeStones;
	public float lightSpeed;
	private bool coroutineRunning = false;
	private bool runningFinalBlinking = false;
	private bool finalAnimation, destruction = false;
	private bool fullPower;

	public bool testBool;

	private void PlaySound(string resource,bool loop)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = loop;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(resource) as AudioClip), ChannelType.LevelEffects,aci);

	}

	private void StopSound(ChannelType channeltype)
	{
		Camera.main.GetComponent<SoundManager>().Stop(channeltype);
	}

	public void SetActivation(bool act, int index)
	{
		if(index < activeStones.Count)
		{
			activeStones[index] = act;
		}
		else
		{
			Debug.LogError("Wrong index!");
		}
	}

	// Use this for initialization
	void Start () {
		li = GetComponentInChildren<Light>();
		li.intensity = 0f;
		activeStones = new List<bool>();
        for (int i = 0; i < activeLights; i++)
        {
            activeStones.Add(false);
        }
	}

	IEnumerator AdjustLightIntensity(float start, float end)
	{
		coroutineRunning = true;
		float t = 0.0f;
		while(t < 1.0f)
		{
			t += Time.deltaTime * lightSpeed;
			float currInt = Mathf.Lerp(start, end, t);
			li.intensity = currInt;
			yield return new WaitForEndOfFrame();
		}

		//li.intensity = end;

       	coroutineRunning = false;
	}

	private float CheckFullPower()
	{
		fullPower = true;
		float nextIntensity = 0.0f;
		for (int i = 0; i < activeStones.Count; i++) {
			
			nextIntensity += activeStones[i] ? 1.0f : 0.0f;
			
			if(!activeStones[i])
			{
				fullPower = false;
			}
		}
		return nextIntensity;
	}

	IEnumerator FinalBlinking()
	{
		runningFinalBlinking = true;
		PlaySound("Levels/TrickyLights/LI_SFX_Cave_BeamCharge_Loop", true);
		float start = li.intensity;
		float end = li.intensity + 4f;
		for (int i = 0; i < 8; i++) {

			float t = 0.0f;
			while(t < 1.0f)
			{
				CheckFullPower();
				if(!fullPower) break;
				t += Time.deltaTime * lightSpeed * (1 + i * 2);
				float intensity = Mathf.Lerp(start,end,t);
				li.intensity = intensity;
				yield return new WaitForEndOfFrame();
			}

			float temp = start;
			start = end;
			end = temp;
			if(!fullPower) break;
			yield return new WaitForEndOfFrame();
		}

		if(fullPower)
		{
			destruction = true;
			StopSound(ChannelType.LevelEffects);
		}

		runningFinalBlinking = false;
	}

	private void CheckStones()
	{

		float startingIntensity = li.intensity;

		fullPower = true;

		float nextIntensity = CheckFullPower();

		if(!Mathf.Approximately(startingIntensity,nextIntensity) && !coroutineRunning)
		{
			StartCoroutine(AdjustLightIntensity(startingIntensity, nextIntensity));
		}

	}

	IEnumerator DestroyWalls()
	{
		PlaySound("Levels/TrickyLights/LI_SFX_Cave_BeamRumble_Loop", false);
		StartCoroutine(Camera.main.LinearShake(1.5f,0.2f,CameraExtension.CameraAxis.XY));
		yield return new WaitForSeconds(1.5f);

		StopSound(ChannelType.LevelEffects);

		GameObject[] walls = GameObject.FindGameObjectsWithTag("WallRoot");
		if(walls == null) Debug.LogError("Walls game tag not found");

		for (int i = 0; i < walls.Length; i++) {
			walls[i].GetComponent<Rigidbody2D>().isKinematic = false;
		}

		PlaySound("Levels/TrickyLights/LI_SFX_Cave_ChargeFinish", false);

	}

	// Update is called once per frame
	void Update () {

		if(!destruction)
		{
			if(!fullPower)
				CheckStones();
			else if(!coroutineRunning)
			{
				if(!runningFinalBlinking)
				{
					StartCoroutine(FinalBlinking());
				}
			}
		}
		else if (!finalAnimation)
		{
			finalAnimation = true;
			li.enabled = false;
			StartCoroutine(DestroyWalls());
		}

	}
}
