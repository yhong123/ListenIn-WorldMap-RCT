using UnityEngine;
using System.Collections;

public class ParticleRemitter : MonoBehaviour {

    private ParticleSystem glowParticle;
    private ParticleSystem lightParticle;
    private ParticleSystem ringParticle;  

    void Awake()
    {
        glowParticle = transform.FindChild("Glow").gameObject.GetComponent<ParticleSystem>();
        lightParticle = transform.FindChild("Light").gameObject.GetComponent<ParticleSystem>();
        ringParticle = transform.FindChild("Ring").gameObject.GetComponent<ParticleSystem>();
    }
    
    void OnEnable()
    {
        Invoke("InitializeParticles", 5.0f);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        glowParticle.Stop();
        lightParticle.Stop();
        ringParticle.Stop();
    }

    IEnumerator EmitParticles()
    {
        glowParticle.Play();
        lightParticle.Play();
        ringParticle.Play();
        yield return new WaitForSeconds(5);
        StartCoroutine(StopParticles());
    }

    IEnumerator StopParticles()
    {
        glowParticle.Stop();
        lightParticle.Stop();
        ringParticle.Stop();
        yield return new WaitForSeconds(8);
        StartCoroutine(EmitParticles());
    }

    void InitializeParticles()
    {
        StartCoroutine(EmitParticles());
    }
}
