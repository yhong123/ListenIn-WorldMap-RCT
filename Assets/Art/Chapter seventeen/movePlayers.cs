using UnityEngine;
using System.Collections;

public class movePlayers : MonoBehaviour {
	//public GameObject player;

	private bool moveDown;
	private bool moveSide;
	private float speedY;
	private float speedX;
	private Vector2 translationX;
	private Vector2 translationY;
	private Vector3 rotation;
	public float maxWidthL = 20.0f;
    public float maxWidthR = 20.0f;
    public float maxHeightU = 10.0f;
    public float maxHeightD = 10.0f;
    public float minSpeedX = 3.0f;
    public float maxSpeedX = 8.0f;
    public float minSpeedY = 2.0f;
    public float maxSpeedY = 8.0f;
    private bool moving;
	private float time = 1.0f;
	private Vector2 smoothVelocity;
	public float timerThreshold;
	private bool waiting = false;

	Vector3 velocityRef = Vector3.zero;
	Vector3 translatioMove = Vector3.zero;

	private Rigidbody2D body;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	IEnumerator SpeedChange()
	{
		float t = 0.0f;
		while (t<timerThreshold) {
			t+=Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		waiting = false;
	}

	void MovePlayer()
	{
		if (moveSide) {
			translationX = speedX * new Vector3(-1.0f, 0.0f, 0.0f);
		}
		else {
			translationX = speedX * new Vector3(1.0f, 0.0f, 0.0f);
		}
		
		if (transform.position.x >= maxWidthR) 
		{
			moveSide = true;
		}
		if (transform.position.x <= maxWidthL *(-1)) 
		{
			moveSide = false;
		}
	}

	void MoveUpPlayer()
	{

		if (moveDown) {

			translationY = new Vector3(0.0f,-1.0f,0.0f) * speedY;
		}
		else {
			translationY = new Vector3(0.0f,1.0f,0.0f) * speedY;
		}

		if (transform.position.y >= maxHeightU) 
		{
			moveDown = true;
		}
		if (transform.position.y <= -1*maxHeightD)
		{
			moveDown = false;
		}
	}


    void FixedUpdate()
    {
        translatioMove = Vector3.SmoothDamp(translatioMove, new Vector3(translationX.x, translationY.y, 0), ref velocityRef, 0.15f);
        body.MovePosition(transform.position + translatioMove * Time.fixedDeltaTime);
    }

    void Update () {

		MovePlayer ();
		MoveUpPlayer ();
		if (!waiting)
		{
			waiting = true;
			speedX = Random.Range(minSpeedX,maxSpeedX);
			speedY = Random.Range(minSpeedY, maxSpeedY);
			StartCoroutine(SpeedChange());
		}


	}

}
