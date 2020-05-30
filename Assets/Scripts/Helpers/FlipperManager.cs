using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HingeJoint2D))]

public class FlipperManager : MonoBehaviour {

	public float releaseSpeed = -50.0f;
	public float activeSpeed = 500.0f;
	public float releaseTime = 5.0f;
	public float activetime = 2.0f;
	public float minAngle = 0.0f;
	public float maxAngle = 60.0f;

	private HingeJoint2D _hinge;
	private JointMotor2D _jointmotors;
	private bool _movingUp;
	private bool _trigger;

	// Use this for initialization
	void Start () {
		_hinge = GetComponent<HingeJoint2D>();
		_jointmotors.motorSpeed = releaseSpeed;
		_jointmotors.maxMotorTorque = 10000.0f;
		_hinge.motor = _jointmotors;
		_hinge.useMotor = true;
		_hinge.useLimits = true;
		JointAngleLimits2D jl = new JointAngleLimits2D();
		jl.min = minAngle;
		jl.max = maxAngle;
		_hinge.limits = jl; 
		_movingUp = true;
		_trigger = true;
	}

	IEnumerator MoveFlipper(float restTime, float speed)
	{
		_jointmotors.motorSpeed = speed;
		_hinge.motor = _jointmotors;
		yield return new WaitForSeconds(restTime);
		_trigger = true;
	}

	//Update is called once per frame
	void Update () {	
		if(_trigger)
		{
			_trigger = false;
			if(_movingUp)
				StartCoroutine(MoveFlipper(releaseTime, releaseSpeed));
			else
				StartCoroutine(MoveFlipper(activetime, activeSpeed));
			_movingUp = !_movingUp;
		}
	}
}
