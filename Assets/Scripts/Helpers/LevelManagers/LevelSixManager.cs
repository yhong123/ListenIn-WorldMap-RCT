using UnityEngine;
using System.Collections;

public class LevelSixManager : ILevel {
	
	public GameObject jellyFish;

	private float currTimer = 0.0f;

	public string soundFolderPath;
	public string[] BubbleSounds;
	private bool playBubbleSound = false; 

	private void PlayBubbleSound()
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = soundFolderPath + "/" + BubbleSounds[Random.Range(0,BubbleSounds.Length)].ToString();
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);
	}

	public override void InitLevel()
	{
		base.InitLevel();

		Physics2D.gravity = new Vector2(0.0f, -4f);

		jellyFish.SetActive(true);

		SplineController sc = null;
		
		sc = jellyFish.GetComponent<SplineController>() as SplineController;
		sc.AutoStart = true;
		sc.UseRigidBody = false;
		sc.UseScaling = false;
		sc.ExecuteMotion();

		currTimer = 0.0f;
		playBubbleSound = true;
	}
	
	public override void EndLevel()
	{
		base.EndLevel();

		Physics2D.gravity = new Vector2(0.0f, -9.81f);
		jellyFish.SetActive(false);
		playBubbleSound = false;
	}
	
	protected override void Start()
	{
        base.Start();
		jellyFish.SetActive(false);
		playBubbleSound = false;
	}
	
	// Update is called once per frame
	public override void UpdateLevel() {

		base.UpdateLevel();
		currTimer += Time.deltaTime;
		if (currTimer > 1.0f && playBubbleSound)
		{
			PlayBubbleSound();
			currTimer = 0.0f;
		}
	}

    #region Difficulty setting
    protected override void PrepareEasyLevel()
    {
        base.PrepareEasyLevel();
    }
    protected override void PrepareMediumLevel()
    {
        base.PrepareMediumLevel();
    }
    protected override void PrepareHardLevel()
    {
        base.PrepareHardLevel();
    }
    #endregion Difficulty setting
}
