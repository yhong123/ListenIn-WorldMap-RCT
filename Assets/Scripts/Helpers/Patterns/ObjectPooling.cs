using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

	public int size;
	public string objToInstantiate;

	//Pool managing
	public float minSpawnTime = 0.5f;
	public float maxSpawnTime = 1.0f;
	private float currSpawnTime = 0.0f;
	float spawnTimer = 0.0f;

	List<GameObject> pool;

	void Start () {
		pool = new List<GameObject>();
		for (int i = 0; i < size; i++) {
			GameObject go = Instantiate(Resources.Load(objToInstantiate, typeof(GameObject)), new Vector3(-10.0f,-10.0f),Quaternion.identity) as GameObject;
			go.GetComponent<ObjectController>().pool = this;
			go.GetComponent<ObjectController>().index = i;
			pool.Add(go);
		}	
	}

	public void ReturnToPool(int idx)
	{
		if(idx < pool.Count)
		{
			pool[idx].transform.position = new Vector3(-10,-10,0);
		}
		else
		{
			Debug.LogError("Wrong index to return at the pool");
		}
	}

	public void Erase()
	{
		if(pool != null && pool.Count != 0)
		{
			for (int i = 0; i < pool.Count; i++) {
				DestroyObject(pool[i]);
			}
		}

		pool.Clear();
	}

	private bool Spawn()
	{
		if(pool != null && pool.Count!=0)
		{
			foreach (var item in pool)
			{
				if(!item.GetComponent<ObjectController>().isActive)
				{
					item.GetComponent<ObjectController>().Spawn();
					return true;
				}
			}
		}
		return false;
	}

	void Update () {
		spawnTimer += Time.deltaTime;
		if(spawnTimer > currSpawnTime)
		{
			
			if(Spawn())
			{
				currSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
				spawnTimer = 0.0f;
			}
		}
	}
}
