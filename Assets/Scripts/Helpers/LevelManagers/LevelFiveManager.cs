using UnityEngine;
using System.Collections;

public class LevelFiveManager : ILevel {
	
	public GameObject[] platforms;
    public GameObject[] objectToDisable;
	public string resourceToLoop;
	private ObjectPooling pool;
		
	public override void InitLevel()
	{
		base.InitLevel();
        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetActive(true);
        }
		

		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		
		string strAudio = "Levels/GreatCanyon/RopeCreak";

		pool = gameObject.AddComponent<ObjectPooling>() as ObjectPooling;
		pool.objToInstantiate = resourceToLoop;
		pool.minSpawnTime = 0.01f;
		pool.maxSpawnTime = 0.05f;
		pool.size = 3;
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.SoundFx, aci);
	}
	
	public override void EndLevel()
	{
		base.EndLevel();

        for (int i = 0; i < platforms.Length; i++)
        {
            platforms[i].SetActive(false);
        }

        for (int i = 0; i < objectToDisable.Length; i++)
        {
            objectToDisable[i].SetActive(false);
        }

        pool.Erase();
	}
	
	protected override void Start()
	{
        base.Start();
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
