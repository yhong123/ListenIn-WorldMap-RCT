using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {


	public bool isActive = false;
	public int index;

	public float minSpeed = 0.5f;
	public float maxSpeed = 1.5f;
	private float currSpeed = 0.0f;

	public float minScale = 0.3f;
	public float maxScale = 0.7f;

	public float xHalfWidth;
	public float yHalfWidth;
	public float yOffset = 3.5f;
	public float xOffset = 2.0f;
	public float xOutOfBoundOffset = 6.0f;

	private int direction = 0;
	private Vector3 smoothVel;
	private Vector3 translation = Vector3.zero;

	private LevelFifteenManager controller;
	
	public void SpawnRandomPosition()
	{
		//float yPos = Random.Range
		float xPos = 0.0f;
		float yPos = 0.0f;
		float scale = Random.Range(minScale,maxScale);
		currSpeed = Random.Range(minSpeed,maxSpeed);
		yPos =  Random.Range(-yHalfWidth + yOffset, yHalfWidth - yOffset);

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

		gameObject.transform.localScale = new Vector3(scale,scale,scale);
		translation = gameObject.transform.position = new Vector3(xPos,yPos,0);

		isActive = true;
	}

	// Use this for initialization
	void Start () {
		Vector3 screenRes = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		xHalfWidth = screenRes.x;
		yHalfWidth = screenRes.y;

		controller = gameObject.transform.parent.gameObject.GetComponent<LevelFifteenManager>();
	}

	private void ReturnObjectToPool()
	{
		controller.ReturnToPool(index);
		isActive = false;
	}

	// Update is called once per frame
	void Update () {

		if(isActive)
		{
			gameObject.transform.position += Vector3.right * direction * currSpeed * Time.deltaTime;
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
