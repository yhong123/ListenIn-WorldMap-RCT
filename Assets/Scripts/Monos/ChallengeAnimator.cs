using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChallengeAnimator : MonoBehaviour {

	public bool activateOutro;
	public bool margineSlide;
	public float xStrenght;
	public float yStrength;
	public float movementSpeed;
	public float waitTimeAfterLastCoin;

    public Vector3[] positions;
	public GameObject[] coinSpawners;
	public GameObject[] margins;
	public int currMargin = 0;

    public Action outroAnimationCompleted;

	public void ActivateOutroAnimation()
	{
		activateOutro = true;
	}

	public void ActivateMarginSlinding()
	{
		margineSlide = true;
	}

	IEnumerator OutroAnimation()
	{
		yield return new WaitForSeconds(1.5f);
		List<Component> components = new List<Component>(coinSpawners[currMargin].GetComponentsInChildren(typeof(Transform)));
		List<Transform> transforms = components.ConvertAll(c => (Transform)c);

		transforms.Remove(coinSpawners[currMargin].transform);
        transforms.Sort(
            delegate (Transform t1, Transform t2)
            {
                return (t1.transform.localPosition.y.CompareTo(t2.transform.localPosition.y));
            }
        );
        for (int j = transforms.Count - 1; j > -1; j--)
		{

            transforms[j].gameObject.GetComponent<CoinLoader>().strenghtXComponent = xStrenght;
            transforms[j].gameObject.GetComponent<CoinLoader>().strenghtYComponent = yStrength;

            transforms[j].gameObject.GetComponent<CoinLoader>().ChargeIntoCannon(positions);
            yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(waitTimeAfterLastCoin);
		ActivateMarginSlinding();
	}

	IEnumerator MarginAnimation()
	{
		Vector3 initialPosition = margins[currMargin].transform.position;
		Vector3 finalposition = initialPosition + Vector3.left * 8;

		float percentage = 0;

		while(percentage <= 1.0f + movementSpeed)
		{
			margins[currMargin].transform.position = Vector3.Lerp(initialPosition, finalposition, percentage);
			percentage += movementSpeed;
			yield return null;
		}

        //OutroAnimation will be used also in challegetutorial. We can proceed only if we are not in that case
        if (!gameObject.name.Contains("Tutorial"))
            StateInitializePinball.Instance.StartPinball();
        else if (outroAnimationCompleted != null)
        {
            outroAnimationCompleted();
        }
        
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(activateOutro)
		{
			activateOutro = false;
			StartCoroutine(OutroAnimation());
		}
		else if(margineSlide)
		{
			margineSlide = false;
			StartCoroutine(MarginAnimation());
		}
	}
}
