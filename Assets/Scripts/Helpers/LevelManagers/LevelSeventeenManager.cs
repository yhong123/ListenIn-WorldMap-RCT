using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSeventeenManager : ILevel
{
    public List<GameObject> pool;
    public GameObject Bridge;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 2.0f;
    public float currSpawnTime = 0.0f;
    private bool init = true;
    float spawnTimer = 0.0f;

    public void ReturnToPool(int index)
    {
        if (index < pool.Count)
        {
            pool[index].transform.position = new Vector3(-10, -10, 0);
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
        Bridge.SetActive(true);
    }

    public override void EndLevel()
    {
        base.EndLevel();
        init = true;
        foreach (var item in pool)
        {
            item.SetActive(false);
        }
        Bridge.SetActive(false);
    }

    public bool Spawn()
    {
        foreach (var item in pool)
        {
            if (!item.GetComponent<CarController>().isActive)
            {
                item.GetComponent<CarController>().SpawnCar();
                return true;
            }
        }
        return false;
    }

    protected override void Start()
    {
        init = true;
        GameObject[] cars = GameObject.FindGameObjectsWithTag("BouncerPeg");
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].GetComponent<CarController>().index = i;
            pool.Add(cars[i]);
        }
        base.Start();
        Bridge.SetActive(false);
    }

    // Update is called once per frame
    public override void UpdateLevel()
    {

        //Cloud managing
        spawnTimer += Time.deltaTime;
        if (spawnTimer > currSpawnTime && !init)
        {
            if (Spawn())
            {
                currSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                spawnTimer = 0.0f;
            }
        }
    }
}
