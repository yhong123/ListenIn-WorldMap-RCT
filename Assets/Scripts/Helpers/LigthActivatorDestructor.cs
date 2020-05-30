using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LigthActivatorDestructor : MonoBehaviour {

  
    public List<Transform> Targets;
    private bool activateLaser = false;

    LineRenderer line;
    private Light innerlight;

    public string soundFolderPath;
    public string[] ActivateSounds;

    public float lineLength = 2.0f;
    public Vector3 finalPosition;

    private void PlaySound()
    {
        AudioClipInfo aci;
        aci.delayAtStart = 0.0f;
        aci.isLoop = false;
        aci.useDefaultDBLevel = true;
        aci.clipTag = string.Empty;

        string strAudio = soundFolderPath + "/" + ActivateSounds[Random.Range(0, ActivateSounds.Length)].ToString();

        Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects, aci);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawLine(Targets[0].position, Targets[0].position + transform.up * lineLength);
    }

    void Start()
    {
        //GameObject go = GameObject.FindGameObjectWithTag("DiamondController");
        //if (go == null) Debug.LogError("Diamond Controller not found");
        //activator = go.GetComponent<DiamondActivator>();
        innerlight = GetComponentInChildren<Light>();
        innerlight.enabled = false;
        line = GetComponentInChildren<LineRenderer>();
        line.SetVertexCount(1 + Targets.Count);
        line.SetWidth(0.2f, 0.2f);

        finalPosition = Targets[0].position + transform.up * lineLength;

    }

    void Update()
    {
        if (activateLaser)
        {
            CheckCollidingCoins();
        }
    }

    public void SetActivationState(bool state)
    {
        //Inversting the state
        state = !state;
        if (activateLaser == state)
            return;

        activateLaser = state;

        line.enabled = state;
        innerlight.enabled = state;
        if (state)
        {
            PlaySound();
            line.SetPosition(0, transform.position);
            line.SetPosition(1, finalPosition);
        }
    }

    void CheckCollidingCoins()
    {
        Vector2 start = Vector2.zero;
        Vector2 raydirection = Vector2.zero;
        start.x = Targets[0].position.x;
        start.y = Targets[0].position.y;

        raydirection.x = transform.up.x;
        raydirection.y = transform.up.y;

        RaycastHit2D[] hitted = Physics2D.RaycastAll(start, raydirection, lineLength, LayerMask.GetMask("Coin"));
        if (hitted != null && hitted.Length > 0)
        {
            for (int i = 0; i < hitted.Length; i++)
            {
                if (hitted[i].transform.gameObject.tag == "Coin")
                {
                    hitted[i].transform.gameObject.GetComponent<CoinMono>().ImmediateDestroy(0.0f);
                }
            }
        }
    }

}
