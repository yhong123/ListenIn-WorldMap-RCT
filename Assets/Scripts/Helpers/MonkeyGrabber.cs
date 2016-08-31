using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class MonkeyGrabber : MonoBehaviour {

	private float currTime = 0f;
	private GameObject grabbedCoin = null;
	private bool grabbed = false;
	private bool canRelease = false;

	public float minReleaseTime = 1.0f;
	public float maxReleaseTime = 5.0f;

	public float minX = 0.0f;
	public float maxX = 20.0f;
	public float minY = 0.0f;
	public float maxY = 20.0f;

	public float maxTorque = 5.0f;

	public string soundFolderPath;

	public string[] MonkeySounds;

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = soundFolderPath + "/" + MonkeySounds[Random.Range(0,MonkeySounds.Length)].ToString();
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Coin" && grabbedCoin == null)
		{
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
			other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			other.gameObject.transform.SetParent(transform,true);
			other.gameObject.transform.localPosition = Vector2.zero;
			other.gameObject.transform.localRotation = Quaternion.identity;
			//other.gameObject.transform.position = transform.position;
			grabbedCoin = other.gameObject;
			PlaySound();
			grabbed = true;
		}
	}

	IEnumerator ReactivateTrigger()
	{
		yield return new WaitForSeconds(0.5f);
		gameObject.GetComponent<CircleCollider2D>().enabled = true;
		grabbedCoin = null;
	}

	IEnumerator ReleaseCoin(float restime)
	{
		while (currTime < restime)
		{
			grabbedCoin.gameObject.transform.localPosition = Vector2.zero;
			grabbedCoin.gameObject.transform.localRotation = Quaternion.identity;
			currTime += Time.deltaTime;
			yield return null;
		}

		PlaySound();
		canRelease = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(grabbed)
		{
			grabbed = false;
			float releaseTime = Random.Range(minReleaseTime,maxReleaseTime);
			StartCoroutine(ReleaseCoin(releaseTime));
		}
		else if(canRelease && grabbedCoin != null)
		{
			canRelease = false;
			grabbedCoin.GetComponent<Rigidbody2D>().isKinematic = false;
			grabbedCoin.transform.SetParent(null);
			grabbedCoin.gameObject.GetComponent<CircleCollider2D>().enabled = true;
			Vector2 appliedForce = new Vector2(Random.Range(minX,maxX), Random.Range(minY,maxY));
			grabbedCoin.GetComponent<Rigidbody2D>().AddForce(appliedForce,ForceMode2D.Impulse);
			grabbedCoin.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0f,maxTorque),ForceMode2D.Impulse);
			currTime = 0f;
			StartCoroutine(ReactivateTrigger());
		}

	}
}
