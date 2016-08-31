using UnityEngine;
using System.Collections;

public class LevelTwentyOneManager : ILevel {

	private GameObject[] bouncers;

	public override void InitLevel()
	{
		base.InitLevel();

		for (int i = 0; i < bouncers.Length; i++) {
			bouncers[i].SetActive(true);
		}

		SplineController sc = null;
		for (int i = 0; i < bouncers.Length; i++) {
			sc = bouncers[i].GetComponent<SplineController>() as SplineController;
			sc.AutoStart = true;
			sc.UseRigidBody = true;
			sc.ExecuteMotion();
		}
	}

	public override void EndLevel()
	{
		base.EndLevel();

		for (int i = 0; i < bouncers.Length; i++) {
			bouncers[i].SetActive(false);
		}
	}

	protected override void Start()
	{
        base.Start();
        bouncers = GameObject.FindGameObjectsWithTag("BouncerPeg");
		for (int i = 0; i < bouncers.Length; i++) {
			bouncers[i].SetActive(false);
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
