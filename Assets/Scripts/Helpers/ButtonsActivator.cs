using UnityEngine;
using System.Collections;

public class ButtonsActivator : MonoBehaviour {

	public Color InactiveColor;
	public Color ActiveColor;

    private Light innerLight;
    
	private bool isActive;
	public bool Actived {
		get{return isActive;}
	}

	private void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Levels/FancyIsland/FlipperAppear";

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Coin")
		{
			isActive = !isActive;
			if(isActive	)
			{
				PlaySound();
			}
		}
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            isActive = !isActive;
            if (isActive)
            {                
                PlaySound();
            }
            if (innerLight != null)
                innerLight.enabled = !isActive;
        }
    }

    void Awake()
	{
		isActive = false;
        innerLight = GetComponentInChildren<Light>();

    }

	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().color = isActive ? ActiveColor : InactiveColor;
	}
}
