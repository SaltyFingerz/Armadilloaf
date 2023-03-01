using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveHazardScript : MonoBehaviour
{
    public ParticleSystem M_Fire;
    public float M_waitTime;
    bool m_OnOff;
    bool m_canStart = false;
    public float M_startTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay(M_startTime));
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
        M_Fire.Play();
        yield return new WaitForSeconds(waitTime);
        GetComponent<SphereCollider>().enabled = false;
        M_Fire.Stop();
        print("op");
        m_OnOff = false;
    }

    IEnumerator Delay(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        m_canStart = true;

    }
}
