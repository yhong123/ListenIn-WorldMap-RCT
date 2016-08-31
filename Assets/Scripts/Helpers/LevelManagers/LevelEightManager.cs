using UnityEngine;
using System.Collections;

public class LevelEightManager : ILevel {

	public GameObject[] monkeys;

	public string resourceToLoop1;
	public string resourceToLoop2;
	private ObjectPooling pool1;
	private ObjectPooling pool2;

	public override void InitLevel()
	{
		base.InitLevel();

        for (int i = 0; i < monkeys.Length; i++)
        {
            monkeys[i].SetActive(true);
        }
		

		pool1 = gameObject.AddComponent<ObjectPooling>() as ObjectPooling;
		pool1.objToInstantiate = resourceToLoop1;
		pool1.minSpawnTime = 0.01f;
		pool1.maxSpawnTime = 0.5f;
		pool1.size = 4;

		pool2 = gameObject.AddComponent<ObjectPooling>() as ObjectPooling;
		pool2.objToInstantiate = resourceToLoop2;
		pool2.minSpawnTime = 0.01f;
		pool2.maxSpawnTime = 1.0f;
		pool2.size = 6;
		
	}

	public override void EndLevel()
	{
		base.EndLevel();

        for (int i = 0; i < monkeys.Length; i++)
        {
            monkeys[i].SetActive(false);
        }
		
		pool1.Erase();
		pool2.Erase();
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
