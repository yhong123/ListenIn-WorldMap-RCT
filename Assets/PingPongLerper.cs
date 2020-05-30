using UnityEngine;
using System.Collections;

public class PingPongLerper : MonoBehaviour {

    public enum EaseType {Linear, Quad}

    public EaseType easetype;

    float currLerpTime;
    public float lerpingTime =  2.0f;
    private delegate float PingPong(float x);
    private PingPong pingpong;

    public float tolerance = 0.001f;

    public Vector3 startPosition, endPosition;

    // Use this for initialization
    void Awake()
    {
        pingpong = x => x;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        currLerpTime += Time.deltaTime;

        //keeping percentange between 0 and maxlerping time
        float percentage = Mathf.Clamp(currLerpTime, 0.0f, lerpingTime);

        //normalizing to one for lerping
        percentage = percentage / lerpingTime;

        switch (easetype)
        {
            case EaseType.Linear:
                //do nothing basically
                break;
            case EaseType.Quad:
                percentage = Mathf.Pow(percentage,2);
                break;
            default:
                break;
        }

        percentage = pingpong(percentage);
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, percentage);

        //Ping pong
        if (isApproximate(percentage, 1.0f, tolerance))
        {
            //reset time and calculate inverse percentage
            currLerpTime = 0.0f;
            pingpong = x => 1 - x;
        }
        else if (isApproximate(percentage, 0.0f, tolerance))
        {
            currLerpTime = 0.0f;
            pingpong = x => x;
        }
	}



    private bool isApproximate(float a, float b, float tol)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}
