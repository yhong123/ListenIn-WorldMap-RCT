using UnityEngine;
using System.Collections;

public class FireButton : MonoBehaviour {


	public GameObject Coin;
	public Sprite[] buttons;

	private SpriteRenderer spriteRenderer;
	private CoinSpawnerB2_Final spawner = null;

	void PlaySound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Sounds/Pinball/RedButtonPress";
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelMusic,aci);

	}

	void OnMouseDown()
	{
		if(spawner != null)
		{
			PlaySound();
			spriteRenderer.sprite = buttons[1];
			if(spawner.stopDropper)
			{
				spawner.stopDropper = false;
			}
			spawner.SetFiringCannon();
		}
	}

	void OnMouseUp()
	{
		spriteRenderer.sprite = buttons[0];
	}

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
        spawner = GameObject.FindGameObjectWithTag("PinballPrefab").GetComponentInChildren<CoinSpawnerB2_Final>() as CoinSpawnerB2_Final;
		Physics2D.IgnoreCollision(Coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
