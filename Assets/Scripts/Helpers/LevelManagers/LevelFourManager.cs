using UnityEngine;
using System.Collections;

public class LevelFourManager : ILevel {
	
	public GameObject[] BottomHoles;

	public string resourceToLoop;
	private ObjectPooling pool;
	
	public override void InitLevel()
	{
		base.InitLevel();

        for (int i = 0; i < BottomHoles.Length; i++)
        {
            BottomHoles[i].SetActive(true);
        }
        
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Levels/FancyIsland/FlipperAppear2";

		pool = gameObject.AddComponent<ObjectPooling>() as ObjectPooling;
		pool.objToInstantiate = resourceToLoop;
		pool.minSpawnTime = 10.0f;
		pool.maxSpawnTime = 3.5f;
		pool.size = 0;

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx, aci);
	}
	
	public override void EndLevel()
	{
		base.EndLevel();

        for (int i = 0; i < BottomHoles.Length; i++)
        {
            BottomHoles[i].SetActive(false);
        }

		pool.Erase();
	}
	
	protected override void Start()
	{
        base.Start();
        for (int i = 0; i < BottomHoles.Length; i++)
        {
            BottomHoles[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	public override void UpdateLevel() {
        		
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
