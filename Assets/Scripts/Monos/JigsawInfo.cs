using UnityEngine;
using System.Collections;

public class JigsawInfo : MonoBehaviour {

	public int BucketIndex;
	public int Index;

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
	}

    public void BurstParticle()
    {
        ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
        ps.Emit(500);
    }

	public void UnpauseAndActivateEffectGame()
	{
        ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
        ps.Emit(1);
        ps.Play();
		Time.timeScale = 1.0f;
	}

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                UnpauseAndActivateEffectGame();
            }
        }
    }
}
