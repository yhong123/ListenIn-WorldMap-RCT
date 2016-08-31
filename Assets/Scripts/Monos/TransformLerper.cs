using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformLerper : MonoBehaviour {

	public GameObject ObjectToLerp;

	public List<Vector3> Positions;
	public List<Vector3> Scalers;
	public List<Vector3> LookAts;

	bool settedRot = false;
	Vector3 currDirection = Vector3.zero;
	float angle = 0.0f;
	Quaternion targetRotQuat = Quaternion.identity;

	public float singleStepDuration;

	public bool usingRectTransform;
	public bool isLocal;
	public bool slerpingIsActive;
	public bool animationEnded = false;

	private bool startAnimation;
	private int currentStep;

	public void AddPosition(Vector3 tr)
	{
		if(Positions != null)
		{
			Positions.Add(tr);
		}
	}

	public void AddScaler(Vector3 tr)
	{
		if(Scalers != null)
		{
			Scalers.Add(tr);
		}
	}

	public void AddLookingTarget(Vector3 toLookAt)
	{
		if(LookAts != null)
		{
			LookAts.Add(toLookAt);
		}
	}

	//Must be forced since we create at runtime
	public void Init()
	{
		Positions = new List<Vector3>();
		Scalers   = new List<Vector3>();
		LookAts   = new List<Vector3>();
	}

	public bool GetStartAnimation()
	{
		return startAnimation;
	}

	public void ResetCounter()
	{
		currentStep = 0;
		animationEnded = false;
		startAnimation = false;
	}

	public void ResetAll()
	{
		currentStep = 0;
		Positions = new List<Vector3>();
		Scalers   = new List<Vector3>();
		LookAts   = new List<Vector3>();

		//This does not properly work. Should check it
		if(!animationEnded)
		{
			if(slerpingIsActive)
			{
				StopCoroutine(AnimateSlerp());
			}				
			else
			{
				StopCoroutine(AnimateLerp());
			}
		}
		animationEnded = false;
		startAnimation = false;
	}

	public void StartAnimation()
	{
		currentStep = 0;
		animationEnded = false;
		startAnimation = true;
	}

	private void SetFinalPosition()
	{
		if(Positions.Count != 0 && currentStep < Positions.Count - 1)
		{
			if(usingRectTransform)
			{
				if(!isLocal)
					ObjectToLerp.GetComponent<RectTransform>().position = Positions[currentStep + 1];
				else
					ObjectToLerp.GetComponent<RectTransform>().localPosition = Positions[currentStep + 1];
			}
			else
			{
				if(!isLocal)
					ObjectToLerp.transform.position = Positions[currentStep + 1];
				else
					ObjectToLerp.transform.localPosition = Positions[currentStep + 1];
				
			}
		}
		
		if(Scalers.Count != 0 && currentStep < Scalers.Count - 1)
		{
			if(usingRectTransform)
				ObjectToLerp.GetComponent<RectTransform>().localScale = Scalers[currentStep + 1];
			else
				ObjectToLerp.transform.localScale = Scalers[currentStep + 1];
			
		}

		if(LookAts.Count != 0 && currentStep < LookAts.Count)
		{
			ObjectToLerp.transform.rotation = targetRotQuat;
		}

	}

	IEnumerator AnimateSlerp()
	{
		var rate = 1.0f/singleStepDuration;
		Quaternion initialRotation = Quaternion.identity;

		//this is dangerous
		if(LookAts.Count != 0 && currentStep < LookAts.Count && ObjectToLerp != null)
		{
			currDirection = (LookAts[currentStep] - ObjectToLerp.transform.position).normalized;
			angle = Mathf.Atan2(currDirection.y,currDirection.x) * Mathf.Rad2Deg + 180;
			targetRotQuat = Quaternion.AngleAxis(angle,Vector3.forward);
			initialRotation = ObjectToLerp.transform.rotation;
			settedRot = true;
		}
		else 
		{
			settedRot = false;
		}

		float f = 0;
		while(f < 1.0f && ObjectToLerp != null)
		{
			f += Time.deltaTime * rate;

			if(Positions.Count != 0 && currentStep < Positions.Count - 1)
			{
				if(usingRectTransform)
				{
					if(!isLocal)
						ObjectToLerp.GetComponent<RectTransform>().position = Vector3.Slerp(Positions[currentStep],Positions[currentStep + 1],f);
					else
						ObjectToLerp.GetComponent<RectTransform>().localPosition = Vector3.Slerp(Positions[currentStep],Positions[currentStep + 1],f);
				}					
				else
				{
					if(!isLocal)
						ObjectToLerp.transform.position = Vector3.Slerp(Positions[currentStep],Positions[currentStep + 1],f);
					else
						ObjectToLerp.transform.localPosition = Vector3.Slerp(Positions[currentStep],Positions[currentStep + 1],f);
					
				}
				
			}
			
			if(Scalers.Count != 0 && currentStep < Scalers.Count - 1)
			{
				if(usingRectTransform)
					ObjectToLerp.GetComponent<RectTransform>().localScale = Vector3.Slerp(Scalers[currentStep],Scalers[currentStep + 1],f);
				else
					ObjectToLerp.transform.localScale = Vector3.Slerp(Scalers[currentStep],Scalers[currentStep + 1],f);
				
			}
			
			if(settedRot)
			{
				ObjectToLerp.transform.rotation = Quaternion.Slerp(initialRotation, targetRotQuat, f);
			}

			yield return 0;
		}

		if(ObjectToLerp != null)
			SetFinalPosition();

		//TODO this condition is not enoough
		if((Positions.Count != 0 && currentStep < Positions.Count - 1) ||
		   (Scalers.Count != 0 && currentStep < Scalers.Count - 1) ||
		   (LookAts.Count != 0  && currentStep < LookAts.Count)
		   && ObjectToLerp != null
		   )
		{
			currentStep++;
			StartCoroutine(AnimateSlerp());
		}
		else
		{
			animationEnded = true;
		}
	}

	IEnumerator AnimateLerp()
	{
		var rate = 1.0f/singleStepDuration;
		Quaternion initialRotation = Quaternion.identity;

		if(LookAts.Count != 0 && currentStep < LookAts.Count && ObjectToLerp != null)
		{
			currDirection = (LookAts[currentStep] - ObjectToLerp.transform.position).normalized;
			angle = Mathf.Atan2(currDirection.y,currDirection.x) * Mathf.Rad2Deg + 180;
			targetRotQuat = Quaternion.AngleAxis(angle,Vector3.forward);
			initialRotation = ObjectToLerp.transform.rotation;
			settedRot = true;
		}
		else 
		{
			settedRot = false;
		}

		float f = 0f;
		while(f < 1.0f && ObjectToLerp != null)
		{
			f += Time.fixedDeltaTime * rate;
			if(Positions.Count != 0 && currentStep < Positions.Count - 1)
			{
				if(usingRectTransform)
				{
					if(!isLocal)
						ObjectToLerp.GetComponent<RectTransform>().position = Vector3.Lerp(Positions[currentStep],Positions[currentStep + 1],f);
					else
						ObjectToLerp.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Positions[currentStep],Positions[currentStep + 1],f);
				}					
				else
				{
					if(!isLocal)
						ObjectToLerp.transform.position = Vector3.Lerp(Positions[currentStep],Positions[currentStep + 1],f);
					else
						ObjectToLerp.transform.localPosition = Vector3.Lerp(Positions[currentStep],Positions[currentStep + 1],f);

				}
					
			}
			if(Scalers.Count != 0 && currentStep < Scalers.Count - 1)
			{
				if(usingRectTransform)
					ObjectToLerp.GetComponent<RectTransform>().localScale = Vector3.Lerp(Scalers[currentStep],Scalers[currentStep + 1],f);
				else
					ObjectToLerp.transform.localScale = Vector3.Lerp(Scalers[currentStep],Scalers[currentStep + 1],f);
				
			}

			if(LookAts.Count != 0 && currentStep < LookAts.Count - 1)
			{
				Vector3 currpointingpos_local = gameObject.transform.InverseTransformPoint(LookAts[currentStep]);
				float angle = Mathf.Atan2(currpointingpos_local.y,currpointingpos_local.x) * Mathf.Rad2Deg;
				//Debug.Log("Found angle: " + angle);
				
				gameObject.transform.Rotate(Vector3.forward, angle + 180, Space.Self);
			}

			if(settedRot)
			{
				ObjectToLerp.transform.rotation = Quaternion.Slerp(initialRotation, targetRotQuat, f);
			}

			yield return 0;
		}

		if(ObjectToLerp != null)
			SetFinalPosition();

		if( 
		   (Positions.Count != 0 && currentStep < Positions.Count - 1) ||
		   (Scalers.Count != 0 && currentStep < Scalers.Count - 1) ||
			(LookAts.Count != 0  && currentStep < LookAts.Count) 
		   && ObjectToLerp != null
		   )
		{
			currentStep++;
			StartCoroutine(AnimateLerp());
		}
		else
		{
			animationEnded = true;
		}


	}

	// Update is called once per frame
	void Update () {
		if(startAnimation)
		{
			startAnimation = false;
			if(ObjectToLerp != null)
			{
				if(!slerpingIsActive)
					StartCoroutine(AnimateLerp ());
				else
					StartCoroutine(AnimateSlerp());
			}
			else
			{
				Debug.LogError("No object to lerp, but startAnimation is true");
			}
		}
	}
}
