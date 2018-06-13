using UnityEngine;
using System.Collections;

public class BridgeController : MonoBehaviour {

    HingeJoint2D jointController;
    public float motorSpeed;

	// Use this for initialization
	void Start () {
        jointController = GetComponent<HingeJoint2D>();
        jointController.useMotor = false;

    }

    void ForceDisableMotor()
    {
        jointController.useMotor = false;
    }

    public void SetBridgeOpenState(bool state)
    {
        JointMotor2D motor = jointController.motor;
        motor.motorSpeed = state ? motorSpeed : -1 * motorSpeed;
        jointController.useMotor = true;
        jointController.motor = motor;
        Invoke("ForceDisableMotor", 1.5f);
    }

}
