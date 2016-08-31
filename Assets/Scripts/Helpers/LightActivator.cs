using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]

public class LightActivator : MonoBehaviour {

	public float rotationAngle;
	public int direction;
	public float precision;

	public int index;

	private Quaternion targetQuat;
	public float speed;
	private bool coroutineRunning = false;

	public List<Transform> Targets;
	private bool activateLaser = false;
	private bool triggerLaser = false;
	private bool freezeRotation =  false;
	private DiamondActivator activator;

	LineRenderer line;

	public string soundFolderPath;
	public string[] ActivateSounds;

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = soundFolderPath + "/" + ActivateSounds[Random.Range(0,ActivateSounds.Length)].ToString();
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);

	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 1, 0, 1);
		Gizmos.DrawLine(Targets[0].position, Targets[1].position);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Coin" && other.relativeVelocity.magnitude > 2f && !freezeRotation)
		{
			if(!coroutineRunning)
			{
				StartCoroutine(RotateSmooth());
			}
		}
	}

	IEnumerator RotateSmooth()
	{
		coroutineRunning = true;
		float t = 0;
		targetQuat = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + rotationAngle * direction, Vector3.forward);
		Quaternion initialQuat = transform.rotation;
		while(t < 1.0f)
		{
			t += Time.deltaTime * speed;
			transform.rotation = Quaternion.Slerp(initialQuat,targetQuat,t);
			yield return null;
		}
		coroutineRunning = false;
	}

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("DiamondController");
		if(go == null) Debug.LogError("Diamond Controller not found");
		activator = go.GetComponent<DiamondActivator>();
		line = GetComponent<LineRenderer>();
		line.SetVertexCount(1 + Targets.Count);
		line.SetWidth(0.2f, 0.2f);
	}

	private void CheckLaserActivation()
	{
		// 1 is lasertip
		Vector3 pointingDirection = (Targets[0].position - transform.position).normalized;
		Vector3 targetDirection = (Targets[1].position - transform.position).normalized;
		activateLaser = Vector3.Dot(targetDirection,pointingDirection) > precision;
		if(activateLaser)
		{
			freezeRotation = true;
		}
	}

	void Update () {

		if(!freezeRotation)
		{
			CheckLaserActivation();
		}

		if(activateLaser)
		{
			line.enabled = true;
			if(!triggerLaser)
			{
				triggerLaser = true;
				PlaySound();
			}
			line.SetPosition(0, transform.position);
			for (int i = 0; i < Targets.Count; i++) {
				line.SetPosition(i + 1,Targets[i].position);
			}
		}
		else
		{
			triggerLaser = false;
			line.enabled = false;
		}

		activator.SetActivation(activateLaser,index);

	}
}
