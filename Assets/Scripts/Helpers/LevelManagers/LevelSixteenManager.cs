using UnityEngine;
using System.Collections;

public class LevelSixteenManager : ILevel {

	public GameObject[] activators;
	public GameObject[] rotatingDuck;

	public override void InitLevel()
	{
		base.InitLevel();
        for (int i = 0; i < activators.Length; i++)
        {
            activators[i].SetActive(true);
        }

        for (int i = 0; i < rotatingDuck.Length; i++)
        {
            rotatingDuck[i].SetActive(true);
        }
		
	}

	public override void EndLevel()
	{
		base.EndLevel();
        for (int i = 0; i < activators.Length; i++)
        {
            activators[i].SetActive(false);
        }

        for (int i = 0; i < rotatingDuck.Length; i++)
        {
            rotatingDuck[i].SetActive(false);
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
