using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(HingeJoint2D))]

public class LeverActivator : MonoBehaviour {

	public bool isOpened;
	public bool Open {
		get { return isOpened; }
		set { isOpened = value; }
	}
	public bool clockwise;
	public float _absolute_speed = 500f;
	public float initialMinAngle = 0.0f;
	public float initalMaxAngle = 90.0f;

	private Rigidbody2D _rigidbody;
	private HingeJoint2D _hinge;

	private float openedSpeed;
	private float closeSpeed;

	private JointMotor2D motors;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){

		isOpened = false;
		_rigidbody = gameObject.GetComponent<Rigidbody2D>();
		_rigidbody.gravityScale = 0.0f;
		_hinge = GetComponent<HingeJoint2D>();
		JointAngleLimits2D anglelLimits = new JointAngleLimits2D();
		anglelLimits.min = initialMinAngle;
		anglelLimits.max = initalMaxAngle;
		motors = new JointMotor2D();
		motors.motorSpeed = 0;
		motors.maxMotorTorque = 10000;

		if(clockwise)
		{
			openedSpeed = _absolute_speed;
			closeSpeed = -1f * _absolute_speed;
		}
		else
		{
			openedSpeed = -1f * _absolute_speed;
			closeSpeed = _absolute_speed;
		}

		_hinge.limits = anglelLimits;
	}

	// Update is called once per frame
	void Update () {
	
		if(isOpened)
		{
			motors.motorSpeed = openedSpeed;
		}
		else
		{
			motors.motorSpeed = closeSpeed;
		}

		_hinge.motor = motors;
				
	}
}
