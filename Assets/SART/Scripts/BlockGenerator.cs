using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockGenerator : MonoBehaviour {

	private  int maxGo = 24;
	private int maxNoGo = 3;
	public int maxBlocks;

	[HideInInspector]
	public List<bool> generatedBlock;

	public List<List<bool>> allBlocks;

	void Awake(){
        allBlocks = new List<List<bool>>();
        StartCoroutine(PrepareBlocks());
    }

    public IEnumerator PrepareBlocks()
    {
        for (int i = 0; i < maxBlocks; i++)
        {
            List<bool> singleBlock = GenerateSingleBlock();
            allBlocks.Add(singleBlock);
            yield return null;
        }
    }

	public List<bool> GenerateSingleBlock(){

		List<bool> trials = new List<bool>(); //List where all trials are stored.
        int[] currPosNogo = new int[3];

        int randomPos = Random.Range(0, 8);
        currPosNogo[0] = randomPos;
        randomPos = Random.Range(9, 18);
        currPosNogo[1] = randomPos;
        randomPos = Random.Range(19, 27);
        currPosNogo[2] = randomPos;

        for (int i = 0; i < maxGo + maxNoGo; i++){	//generate all 'trues' and add to list.
			trials.Add (true);
		}

        trials[currPosNogo[0]] = false;
        trials[currPosNogo[1]] = false;
        trials[currPosNogo[2]] = false;

        return trials;
	}
}
