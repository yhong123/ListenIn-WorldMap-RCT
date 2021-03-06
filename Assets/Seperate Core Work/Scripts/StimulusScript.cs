﻿using UnityEngine;
using System.Collections;

public class StimulusScript : MonoBehaviour {

	GameObject m_goImage;
	GameObject m_goRectangle;
	Animator m_anim;

	enum StimulusState {Idle, Throwing, Disappearing}

	StimulusState currState;

	public Vector3 throwPosition;
	public Vector3 endPosition;
	float throwingSpeed = 1.5f;

    private bool initialize = false;
	//----------------------------------------------------------------------------------------------------
	// Use this for initialization
	//----------------------------------------------------------------------------------------------------
	void Start () 
	{
		m_goImage = transform.FindChild("PictureFrame").gameObject;
		//m_goRectangle = transform.FindChild("Rectangle").gameObject;
		m_anim = GetComponent<Animator> ();

		currState = StimulusState.Idle;
	}

	IEnumerator ThrowCards()
	{
		float percent = 0;
		while(percent <= 1.0f)
		{
			percent += Time.deltaTime * throwingSpeed;

			float interpolation = Mathf.Sqrt(percent);
			transform.position = Vector3.Lerp(throwPosition, endPosition, interpolation);
			yield return null;
		}
	}

	//----------------------------------------------------------------------------------------------------
	// Update is called once per frame
	//----------------------------------------------------------------------------------------------------
	void Update () 
	{
		if(currState == StimulusState.Throwing)
		{
			currState = StimulusState.Idle;
			StartCoroutine(ThrowCards());
		}
	}

    void LateUpdate()
    {
        if (!initialize)
        {
            //ResetPosition();
            //ResetScale();
            initialize = true;
        }
    }

	public void SetThrowPosition(Vector3 pos)
	{
		throwPosition = pos;
	}

	public void SetFinalPosition(Vector3 pos)
	{
		endPosition = pos;
	}

    private void ResetScale()
    {
        this.transform.localScale = new Vector3(0.2f, 0.2f);
    }

	public void ResetPosition()
	{
		transform.position = throwPosition;
	}

	//----------------------------------------------------------------------------------------------------
	// SetStimulusImage: set stimulus's image
	//----------------------------------------------------------------------------------------------------
	public void SetStimulusImage (string strImage) 
	{
		if (!m_goImage)
			m_goImage = transform.FindChild("PictureFrame").gameObject;

		// retrieve image from the Resource folder
		m_goImage.GetComponent<SpriteRenderer> ().sprite = Resources.Load(strImage, typeof(Sprite)) as Sprite;

		// scale sprite to smaller size
		Vector3 scale = new Vector3(2, 2, 1);   // new Vector3(0.65f, 0.65f, 1);
		m_goImage.transform.localScale = scale;

	}

	//----------------------------------------------------------------------------------------------------
	// ShowStimulus: show / hide stimulus using animation
	//----------------------------------------------------------------------------------------------------
	public void ShowStimulus (bool bShow) 
	{
		if (!m_anim)
		{
			m_anim = GetComponent<Animator> ();
		}

		m_anim.SetBool ("bSpawn", bShow);

		if(bShow && currState == StimulusState.Idle)
		{
			currState = StimulusState.Throwing;
		}
	}
}
