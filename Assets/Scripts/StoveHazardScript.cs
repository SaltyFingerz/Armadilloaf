using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveHazardScript : MonoBehaviour
{
    public List <ParticleSystem> M_Fire;
    public float M_waitTime;
    bool m_OnOff;
    bool m_canStart = false;
    public float M_startTime;
    private AudioSource m_fireSound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay(M_startTime));
        m_fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       while(!m_OnOff && m_canStart) 
            StartCoroutine(OnOffStove(M_waitTime));
    }

    IEnumerator OnOffStove(float waitTime)
    { m_OnOff = true;
        
        yield return new WaitForSeconds(waitTime);
        GetComponent<SphereCollider>().enabled = true;

        for (int i = 0; i<M_Fire.Count; i++)
        {
            M_Fire[i].Play();
            m_fireSound.Play();
        }

        yield return new WaitForSeconds(waitTime);
        GetComponent<SphereCollider>().enabled = false;
        for (int i = 0; i < M_Fire.Count; i++)
        {
            M_Fire[i].Stop();
            m_fireSound.Stop();
        }
        m_OnOff = false;
    }

    IEnumerator Delay(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        m_canStart = true;

    }
}
