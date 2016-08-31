using UnityEngine;
using System.Collections;

// Script to rotate a platform with speed and direction input
public class PlatformSpinner : MonoBehaviour {

    public int speed;
	public int secondspeed;
    public int direction;

	public float minRandomSeconds;
	public float maxRandomSeconds;

	public bool useRigidBody = false;

	private float currRandomWaitSeconds = 0.0f;

	private float currTime;
	public int currSpeed;

	public bool randomize_speed;
	public bool randomize_direction;

	private Rigidbody2D _body;

	void Awake()
	{
		currTime = Time.time;
		currSpeed = speed;
	}
	
	void Update () {

	}

	void FixedUpdate()
	{
		if(randomize_speed && ((Time.time - currTime) > currRandomWaitSeconds))
		{
			if(randomize_direction)
			{
				direction = Random.Range(0,2) * 2 - 1;
			}

			currSpeed = Random.Range(speed, secondspeed + 1);

			//transform.Rotate(new Vector3(0,0,direction) * speed * Time.fixedDeltaTime);
			currRandomWaitSeconds = Random.Range(minRandomSeconds,maxRandomSeconds);
			currTime = Time.time;
		}

		if(useRigidBody)
		{
			gameObject.GetComponent<Rigidbody2D>().AddTorque(direction * currSpeed,ForceMode2D.Force);
		}
		else
		{
            transform.Rotate(new Vector3(0,0,direction) * currSpeed * Time.fixedDeltaTime);
		}

	}

}
