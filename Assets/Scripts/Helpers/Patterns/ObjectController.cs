using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public bool isActive = false;
	public int index;
	
	public float minSpeed = 0.5f;
	public float maxSpeed = 1.5f;
	private float currSpeed = 0.0f;
	
	public float minScale = 0.3f;
	public float maxScale = 0.7f;

	public float minYPos = -2.0f;
	public float maxYPos = -1.3f;

	public float xHalfWidth;
	public float yHalfWidth;
	public float yOffset = 3.5f;
	public float xOffset = 2.0f;
	public float xOutOfBoundOffset = 6.0f;
	
	private int direction = 0;
	private Vector3 smoothVel;
	private Vector3 translation = Vector3.zero;

	public string soundFolderPath;
	public string[] sounds;
	private bool bPlay = true;

	public ObjectPooling pool;

	void Start () {
		Vector3 screenRes = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		xHalfWidth = screenRes.x;
		yHalfWidth = screenRes.y;
	}

	private void ReturnObjectToPool()
	{
		pool.ReturnToPool(index);
		bPlay = true;
		isActive = false;
	}

	public void Spawn()
	{
		//float yPos = Random.Range
		float xPos = 0.0f;
		float yPos = 0.0f;
		float scale = Random.Range(minScale,maxScale);
		currSpeed = Random.Range(minSpeed,maxSpeed);
		yPos =  Random.Range(minYPos, maxYPos);
		
		if(Random.Range(0.0f,1.0f) > 0.49f)
		{
			//spawnRight
			xPos = xHalfWidth + xOffset;
			direction = -1;
		}
		else
		{
			//spawnLeft
			xPos = -xHalfWidth - xOffset; 
			direction = 1;
		}
		
		gameObject.transform.localScale = new Vector3(scale * direction,scale,scale);
		translation = gameObject.transform.position = new Vector3(xPos,yPos,0);
		
		isActive = true;
	}

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;

		string strAudio = soundFolderPath + "/" + sounds[Random.Range(0,sounds.Length)].ToString();

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);
	}

	void Update () {

		if(isActive)
		{
			gameObject.transform.position += Vector3.right * direction * currSpeed * Time.deltaTime;

			if(sounds != null && sounds.Length != 0)
			{
				if(Mathf.Abs(gameObject.transform.position.x) < xHalfWidth && bPlay)
				{
					bPlay = false;
					PlaySound();
				}
			}
			else
			{
				Debug.LogWarning("No sound attached to moving object");
			}
		}
		
		if(direction == 1)
		{
			if(transform.position.x > xHalfWidth + xOutOfBoundOffset)
			{
				ReturnObjectToPool();
			}
		}
		else
		{
			if(transform.position.x < - xHalfWidth - xOutOfBoundOffset)
			{
				ReturnObjectToPool();
			}
		}

	}
}
