using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour {

	public bool roaming = true;
	public bool chasing = false;
	public bool waitingForCoin = false;
	public bool wandering= false;
	public bool searching = false;

	private float timerRoaming = 0f;
	private float timerStaring = 0.0f;
	public float maxRoamingTime;
	public float maxStaringTime;

	public float mindirectionChangeInterval;
	public float maxdirectionChangeInterval;
	private float directionChangeInterval =0f;
	public float changeAmount;
	private float heading;
	private Quaternion targetRotation;
	private bool headingCorooutineRunning = false;
	private bool runningReverseHeading = false;

	private float roamSpeed = 0.0f;

	public float maxRotSpeed;
	public float linDamp;
	public float minLinSpeedRoam;
	public float maxLinSpeedRoam;
	public float maxLinSpeed;
	//public float distanceThreshold = 2.0f;

	public GameObject target = null;
	public GameObject spawner = null;

	public BoxCollider2D boundings;

	private Rigidbody2D body;
	Vector3 currHeading = Vector3.zero;

	//Sounds
	private enum SoundType
	{
		Spotted,
		Movement,
		Eating
	}

	public string soundFolderPath;

	public string[] SpottedSounds;
	public string[] MovementSounds;
	public string[] EatingSounds;

	private void PlaySound(SoundType type)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = false;
		aci.clipTag = string.Empty;
		string strAudio = soundFolderPath + "/";

		switch (type) {
		case SoundType.Movement:
			if(MovementSounds != null && MovementSounds.Length != 0)
			{
				strAudio += MovementSounds[Random.Range(0,MovementSounds.Length)].ToString();
				Camera.main.GetComponent<SoundManager>().SetChannelLevel(ChannelType.LevelEffects, -5.0f);
			}
			break;
		case SoundType.Spotted:
			if(SpottedSounds != null && SpottedSounds.Length != 0)
			{
				strAudio += SpottedSounds[Random.Range(0,SpottedSounds.Length)].ToString();
				Camera.main.GetComponent<SoundManager>().SetChannelLevel(ChannelType.LevelEffects, 5.0f);
			}
			break;
		case SoundType.Eating:
			if(EatingSounds != null && EatingSounds.Length != 0)
			{
				strAudio += EatingSounds[Random.Range(0,EatingSounds.Length)].ToString();
				Camera.main.GetComponent<SoundManager>().SetChannelLevel(ChannelType.LevelEffects, 5.0f);
			}
			break;
		default:
			break;
		}

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);

	}

	private void CheckExistingCoin()
	{
		GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

		if(coins != null && coins.Length != 0 && target == null)
		{
			for (int i = 0; i < coins.Length; i++) {
				if(!coins[i].GetComponent<Rigidbody2D>().isKinematic)
				{
					target = coins[i];
					PlaySound(SoundType.Spotted);
					chasing = true;
					roaming = false;
					break;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		spawner = GameObject.FindGameObjectWithTag("Respawn");
		heading = Random.Range(0,360);
		transform.eulerAngles = new Vector3(0,0,heading - 90);
		body.freezeRotation = true;

		StartCoroutine(NewTargetWandering());

	}

	private void LookAtGunner()
	{
		Vector3 destDir = spawner.transform.position - this.transform.position;
		destDir.Normalize();
		
		float angle = Mathf.Atan2(destDir.y,destDir.x) * Mathf.Rad2Deg - 90;
		Quaternion targetRotQuat = Quaternion.AngleAxis(angle,Vector3.forward);
		
		float step = maxRotSpeed * Time.fixedDeltaTime;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotQuat, step);
	}

	IEnumerator BackSpeed(int direction)
	{
		runningReverseHeading = true;
		body.velocity = new Vector2(body.velocity.x, direction * maxLinSpeed);
		//Quaternion initialRotation = transform.rotation;
		targetRotation = Quaternion.AngleAxis(heading + 90, Vector3.forward);
		float timer = 0.0f;
		while (timer < 0.7f)
		{
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, timer * 0.015f);
			yield return null;
		}
		runningReverseHeading = false;
	}

	private void ApplyFrameConstriction()
	{
		//I can try to invert the heading
		if(transform.position.y > boundings.bounds.extents.y + boundings.offset.y)
		{
			if(!runningReverseHeading)
			{
				StartCoroutine(BackSpeed(-1));
			}
		}
		else if(transform.position.y < -boundings.bounds.extents.y + boundings.offset.y)
		{
			if(!runningReverseHeading)
			{
				transform.rotation = targetRotation = Quaternion.AngleAxis(heading + 180, Vector3.forward);
				StartCoroutine(BackSpeed(1));
			}
		}
	}

	private void Seek(Vector3 targetposition)
	{
		Vector3 dest = targetposition- this.transform.position;
		Vector3 destDir = dest.normalized;

		float angle = Mathf.Atan2(destDir.y,destDir.x) * Mathf.Rad2Deg - 90;
		Quaternion targetRotQuat = Quaternion.AngleAxis(angle,Vector3.forward);

		float step = maxRotSpeed * Time.fixedDeltaTime;

		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotQuat, step);

		currHeading = Vector3.Lerp(currHeading, dest, linDamp * Time.fixedDeltaTime);
		body.velocity = currHeading * maxLinSpeed;

		ApplyFrameConstriction();
 
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Coin")
		{
			DestroyObject(collision.gameObject);
			PlaySound(SoundType.Eating);
		}
	}

	void CheckTargetPosition()
	{
		if(target != null)
		{
			if(target.transform.position.x > boundings.bounds.extents.x || target.transform.position.x < -boundings.bounds.extents.x)
			{
				target = null;
			}			
			else if(target.transform.position.y < -boundings.bounds.extents.y)
			{
				target = null;
			}
		}
	}

	void OnDrawGizmosSelected() {
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawSphere(transform.position, 1);
	}
	
	IEnumerator NewTargetWandering()
	{
		PlaySound(SoundType.Movement);
		headingCorooutineRunning = true;
		float a = Mathf.Clamp(heading - changeAmount, 0, 360);
		float b = Mathf.Clamp(heading + changeAmount, 0, 360);
		heading = Random.Range(a,b);
		targetRotation = Quaternion.AngleAxis(heading-90, Vector3.forward);// new Vector3(0,0,heading - 90);
		roamSpeed = Random.Range(minLinSpeedRoam,maxLinSpeedRoam);
		directionChangeInterval = Random.Range(mindirectionChangeInterval,maxdirectionChangeInterval);
		yield return new WaitForSeconds(directionChangeInterval);
		headingCorooutineRunning = false;
	}

	void FixedUpdate()
	{
		if(roaming)
		{
			timerRoaming += Time.fixedDeltaTime;
			if(timerRoaming > maxRoamingTime)
			{
				timerStaring += Time.fixedDeltaTime;
				body.velocity = Vector3.Lerp(body.velocity,Vector3.zero, Time.fixedDeltaTime * 2.0f);
				LookAtGunner();
				CheckExistingCoin();
				
				if(timerStaring > maxStaringTime && roaming)
				{
					timerRoaming = 0f;
					timerStaring = 0.0f;
				}
			}
			else
			{
				if(!headingCorooutineRunning)
				{
					StartCoroutine(NewTargetWandering());
				}
				else
				{
					if(!runningReverseHeading)
					{
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * directionChangeInterval);
						Vector3 forward  = transform.TransformDirection(Vector3.up);
						body.velocity = forward * roamSpeed;
					}
				}
			}

			ApplyFrameConstriction();

		}
		else if(chasing)
		{
			CheckTargetPosition();
			if(target != null)
				Seek(target.transform.position);
			else
			{
				chasing = false;
				roaming = true;
				timerRoaming = 0f;
				timerStaring = 0.0f;
			}
		}		
	}
}
 