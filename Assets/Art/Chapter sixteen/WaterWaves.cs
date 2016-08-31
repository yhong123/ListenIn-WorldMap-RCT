using UnityEngine;
using System.Collections;

public class WaterWaves : MonoBehaviour {

	//an absolute value
	public float maxDistance;
	private int direction = 1;

	public float translationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float currXpos = transform.position.x;
		if( Mathf.Abs(currXpos) > maxDistance)
		{
			direction *= -1;
		}

		transform.position += Vector3.right * direction * translationSpeed * Time.deltaTime;

	}
}
