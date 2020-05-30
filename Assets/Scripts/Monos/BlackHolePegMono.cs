using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BlackHolePegMono : MonoBehaviour {

    private List<GameObject> m_teleportCoins = new List<GameObject>(); 
    
	public GameObject Teleporter;
	public bool teleporting = false;
    public bool destroyCoin = false;

    public string soundFolderPath;

	private Vector3 originalScale;

	IEnumerator Expel(GameObject teleportCoin)
	{
		teleportCoin.transform.transform.position = Teleporter.transform.transform.position;
		teleportCoin.GetComponent<Rigidbody2D>().gravityScale = 0.6f;

		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		string strAudio = soundFolderPath + "/" + "BlackholeOut";
		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);

		while(teleportCoin.transform.localScale.x < originalScale.x)
		{
			teleportCoin.transform.localScale += new Vector3(0.04f,0.04f);
			//teleportCoin.transform.transform.position = Vector2.MoveTowards ( teleportCoin.transform.position , Teleporter.transform.position - Vector3.up * 0.3f , 0.05f );
			yield return null;
		}
		yield return new WaitForSeconds(0.45f);
		teleporting = false;
		teleportCoin.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
		teleportCoin.GetComponent<Collider2D>().enabled = true;
	}

	IEnumerator Swallow(GameObject teleportCoin)
	{
		originalScale = teleportCoin.transform.localScale;

		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = true;
		aci.clipTag = string.Empty;
		string strAudio = soundFolderPath + "/" + "BlackholeIn";

		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(strAudio) as AudioClip), ChannelType.LevelEffects,aci);

		while(teleportCoin.transform.localScale.x > 0.0f)
		{
			teleportCoin.transform.localScale -= new Vector3(0.02f,0.02f);
			teleportCoin.transform.transform.position = Vector2.MoveTowards ( teleportCoin.transform.position , transform.position , 0.2f );
			yield return null;
		}
        if (!destroyCoin)
        {
            StartCoroutine(Expel(teleportCoin));
        }
        else
        {
            DestroyImmediate(teleportCoin);
            teleporting = false;
        }
	}

    void OnTriggerEnter2D ( Collider2D collider )
    {
		if ( !m_teleportCoins.Contains ( collider.gameObject ) && !teleporting)
        {
            collider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
			collider.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
            m_teleportCoins.Add ( collider.gameObject );
            if(!destroyCoin)
			    teleporting = true;
            collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    void Update()
    {
		foreach ( var teleportCoin in m_teleportCoins.ToList () )
        {
			m_teleportCoins.Remove(teleportCoin);
			StartCoroutine(Swallow(teleportCoin));
        }
        
    }
}
