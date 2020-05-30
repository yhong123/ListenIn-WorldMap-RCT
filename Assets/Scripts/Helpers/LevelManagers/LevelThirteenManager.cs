using UnityEngine;
using System.Collections;

public class LevelThirteenManager : ILevel {

	public GameObject[] russianToys;

	public override void InitLevel()
	{
		base.InitLevel();
        for (int i = 0; i < russianToys.Length; i++)
        {
            russianToys[i].SetActive(true);
        }
	}

	public override void EndLevel()
	{
		base.EndLevel();
        for (int i = 0; i < russianToys.Length; i++)
        {
            russianToys[i].SetActive(false);
        }
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
