using UnityEngine;
using System.Collections;

public class CoinMono : MonoBehaviour {

    public bool isDeleting = false;

    public string soundFolderPath;
    public string[] CoinBucketSounds;

    public string toneFolderPath;
    public float toneHitThreshold = 1.0f;
    public string[] MusicalPegSoundsLow;
    public string[] MusicalPegSoundsMedium;
    public string[] MusicalPegSoundsHigh;


    private enum MusicalSounds { Low, Medium, High };

    private int musicalCounterLow;
    private int musicalCounterMedium;
    private int musicalCounterHigh;

    void Start()
    {
        musicalCounterLow = 0;
        musicalCounterMedium = 0;
        musicalCounterHigh = 0;
    }

    private void AdjustMusicalSound(MusicalSounds soundType, string soundToPlay)
    {

        switch (soundType) {
            case MusicalSounds.Low:
                PlaySound(soundToPlay, false, 18.0f);
                musicalCounterLow++;
                if (musicalCounterLow == MusicalPegSoundsLow.Length)
                {
                    musicalCounterLow = 0;
                }
                break;
            case MusicalSounds.Medium:
                PlaySound(soundToPlay, false, 6.0f);
                musicalCounterMedium++;
                if (musicalCounterMedium == MusicalPegSoundsMedium.Length)
                {
                    musicalCounterMedium = 0;
                }
                break;
            case MusicalSounds.High:
                PlaySound(soundToPlay, false, 10.0f);
                musicalCounterHigh++;
                if (musicalCounterHigh == MusicalPegSoundsHigh.Length)
                {
                    musicalCounterHigh = 0;
                }
                break;
            default:
                break;
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.relativeVelocity.magnitude > toneHitThreshold)
        {
            if (coll.gameObject.tag == "MusicalPegLow" && MusicalPegSoundsLow.Length != 0)
            {
                string strAudio = toneFolderPath + "/" + MusicalPegSoundsLow[musicalCounterLow].ToString();
                AdjustMusicalSound(MusicalSounds.Low, strAudio);

            }
            else if (coll.gameObject.tag == "MusicalPegMedium" && MusicalPegSoundsMedium.Length != 0)
            {
                string strAudio = toneFolderPath + "/" + MusicalPegSoundsMedium[musicalCounterMedium].ToString();
                AdjustMusicalSound(MusicalSounds.Medium, strAudio);
            }
            else if (coll.gameObject.tag == "MusicalPegHigh" && MusicalPegSoundsHigh.Length != 0)
            {
                string strAudio = toneFolderPath + "/" + MusicalPegSoundsHigh[musicalCounterHigh].ToString();
                AdjustMusicalSound(MusicalSounds.High, strAudio);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 view = Camera.main.WorldToViewportPoint(this.transform.position);
        if (view.y <= -1.0f)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
        if (isDeleting && tag != "DestroyedCoin")
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                UnityEngine.Object.Destroy(this.gameObject, 1.5f);
            }
        }
    }

    public void ImmediateDestroy(float timer)
    {
        if (!isDeleting)
        {
            isDeleting = true;
            //this.GetComponent<SpriteRenderer>().enabled = false;
            //this.GetComponent<Collider2D>().enabled = false;
            //this.GetComponent<Rigidbody2D>().isKinematic = true;
            //this.gameObject.tag = "DestroyedCoin";
            if (timer > 1.5f)
            {
                timer = 1.5f;
            }
            GameObject.Destroy(this.gameObject, timer);
        }
    }

    public void BeginDestroy()
    {
            isDeleting = true;
            string strAudio = soundFolderPath + "/" + CoinBucketSounds[Random.Range(0, CoinBucketSounds.Length)].ToString();
            PlaySound(strAudio, false, 0.0f, ChannelType.SoundFx);
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<Rigidbody2D>().isKinematic = true;
     
    }

    void OnDestroy()
    {
        //this.gameObject.tag = "DestroyedCoin";
    }

    private void PlaySound(string soundResource, bool useDefault = true, float DBValue = 12.0f, ChannelType chType = ChannelType.CoinEffects)
	{
		AudioClipInfo aci;
		aci.delayAtStart = 0.0f;
		aci.isLoop = false;
		aci.useDefaultDBLevel = useDefault;
		aci.clipTag = string.Empty;
		
		Camera.main.GetComponent<SoundManager>().SetChannelLevel(chType, DBValue);		
		Camera.main.GetComponent<SoundManager>().Play((Resources.Load(soundResource) as AudioClip), chType,aci);
		
	}
}
