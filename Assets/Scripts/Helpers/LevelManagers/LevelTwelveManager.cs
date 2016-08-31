using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTwelveManager : ILevel {

	public List<GameObject> pool;
	public float minSpawnTime = 0.5f;
	public float maxSpawnTime = 2.0f;
	public float currSpawnTime = 0.0f;
	private bool init = true;
	float spawnTimer = 0.0f;

	public void ReturnToPool(int index)
	{
		if(index < pool.Count)
		{
			pool[index].transform.position = new Vector3(-10,-10,0);
		}
		else
		{
			Debug.LogError("Wrong index to return at the pool");
		}
	}

	public override void InitLevel()
	{
		base.InitLevel();
		currSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
		init = false;
	}

	public override void EndLevel()
	{
		base.EndLevel();
		init = true;
		foreach (var item in pool) {
			item.SetActive(false);
		}
	}

	public bool Spawn()
	{
		foreach (var item in pool) {
			if(!item.GetComponent<JapaneseGirlController>().isActive)
			{
				item.GetComponent<JapaneseGirlController>().SpawnRandomPosition();
				return true;
			}
		}
		return false;
	}

	protected override void Start()
	{
		init = true;
		GameObject[] jap = GameObject.FindGameObjectsWithTag("BouncerPeg");
		for (int i = 0; i < jap.Length; i++) {
			jap[i].GetComponent<JapaneseGirlController>().index = i;
			pool.Add(jap[i]);
		}
        base.Start();
	}

	// Update is called once per frame
	public override void UpdateLevel() {
		//Cloud managing
		spawnTimer += Time.deltaTime;
		if(spawnTimer > currSpawnTime && !init)
		{			
			if(Spawn())
			{
				currSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
				spawnTimer = 0.0f;
			}
		}
	}

    #region Difficulty setting
    protected override void PrepareEasyLevel()
    {
        base.PrepareEasyLevel();
        JapaneseGirlController jgc;
        foreach (var item in pool)
        {
            jgc = item.GetComponent<JapaneseGirlController>();
            jgc.minSpeed = 300;
            jgc.maxSpeed = 370;
            jgc.minJumpStrengh = 200;
            jgc.maxJumpStrengh = 300;
            jgc.minjumptimer = 0.6f;
            jgc.maxjumptimer = 1.6f;
        }
        minSpawnTime = 1.5f;
        maxSpawnTime = 3.0f;
    }

    protected override void PrepareMediumLevel()
    {
        base.PrepareMediumLevel();
        JapaneseGirlController jgc;
        foreach (var item in pool)
        {
            jgc = item.GetComponent<JapaneseGirlController>();
            jgc.minSpeed = 350;
            jgc.maxSpeed = 420;
            jgc.minJumpStrengh = 250;
            jgc.maxJumpStrengh = 380;
            jgc.minjumptimer = 0.3f;
            jgc.maxjumptimer = 1.3f;
        }
    }

    protected override void PrepareHardLevel()
    {
        base.PrepareHardLevel();
        JapaneseGirlController jgc;
        foreach (var item in pool)
        {
            jgc = item.GetComponent<JapaneseGirlController>();
            jgc.minSpeed = 380;
            jgc.maxSpeed = 400;
            jgc.minJumpStrengh = 300;
            jgc.maxJumpStrengh = 350;
            jgc.minjumptimer = 0.3f;
            jgc.maxjumptimer = 1.3f;
        }
    }
    #endregion Difficulty setting

}
