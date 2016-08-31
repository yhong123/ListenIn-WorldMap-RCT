using UnityEngine;
using System.Collections;

public class LevelSevenManager : ILevel {
	
	private GameObject[] iceblocks;
	
	public override void InitLevel()
	{
		base.InitLevel();

		SplineController sc = null;
		for (int i = 0; i < iceblocks.Length; i++) {
			sc = iceblocks[i].GetComponent<SplineController>() as SplineController;
			sc.AutoStart = true;
			sc.UseRigidBody = true;
			sc.ExecuteMotion();
		}
	}
	
	public override void EndLevel()
	{
		base.EndLevel();

		for (int i = 0; i < iceblocks.Length; i++) {
			iceblocks[i].SetActive(false);
		}
	}
	
	protected override void Start()
	{
        base.Start();
		iceblocks = GameObject.FindGameObjectsWithTag("BouncerPeg");
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
