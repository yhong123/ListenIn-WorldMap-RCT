using UnityEngine;
using System.Collections;

public class LevelThreeManager : ILevel {

	private GameObject[] bouncers;

	public string resourceToLoop;
	private ObjectPooling pool;

	public override void InitLevel()
	{
		base.InitLevel();

		SplineController sc = null;
		for (int i = 0; i < bouncers.Length; i++) {
			sc = bouncers[i].GetComponent<SplineController>() as SplineController;
			sc.AutoStart = true;
			sc.UseRigidBody = true;
			sc.ExecuteMotion();
		}

		pool = gameObject.AddComponent<ObjectPooling>() as ObjectPooling;
		pool.objToInstantiate = resourceToLoop;
		pool.minSpawnTime = 1.5f;
		pool.maxSpawnTime = 3.5f;
		pool.size = 4;
	}

	public override void EndLevel()
	{
		base.EndLevel();
		for (int i = 0; i < bouncers.Length; i++) {
			bouncers[i].SetActive(false);
		}

		pool.Erase();
	}

	protected override void Start()
	{
        base.Start();
		bouncers = GameObject.FindGameObjectsWithTag("MusicalPegHigh");

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
