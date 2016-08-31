using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

	public string soundFolderPath;
	public float threshold = 3.0f;
	public bool defaultDBLevel = true;
	public float DBLevel = 10.0f;
	public string[] HitSounds;

	public bool useRotatingSounds;
	public string[] RotatingSounds;
	private float initialRotZ;

	public void PlaySoundOnce(string resource, float volume)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = false;
		aci.clipTag = string.Empty;
		Camera.main.GetComponent<SoundManager>().SetChannelLevel(ChannelType.LevelEffects, volume);
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(resource) as AudioClip), ChannelType.LevelEffects,aci);
	}

	public void PlaySound(string resource, bool _defaultDBLevel)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = _defaultDBLevel;
		aci.clipTag = string.Empty;
		Camera.main.GetComponent<SoundManager>().SetChannelLevel(ChannelType.LevelEffects, DBLevel);
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(resource) as AudioClip), ChannelType.LevelEffects,aci);
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Coin" && collider.relativeVelocity.magnitude > threshold)
		{
			string strAudio = soundFolderPath + "/" + HitSounds[Random.Range(0,HitSounds.Length)].ToString();
			PlaySound(strAudio, defaultDBLevel);
		}
	}

	void Start () {
		initialRotZ = transform.eulerAngles.z;
	}
	
	void Update () {
		if(useRotatingSounds && Mathf.Abs(transform.eulerAngles.z - initialRotZ) < 0.1f)
		{
			string strAudio = soundFolderPath + "/" + RotatingSounds[Random.Range(0,RotatingSounds.Length)].ToString();
			PlaySound(strAudio, true);
		}
	}
}
