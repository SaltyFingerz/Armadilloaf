using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveHazard : MonoBehaviour
{
    public ParticleSystem Fire;
    public float waitTime;
    bool OnOff;
    bool canStart = false;
    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay(startTime));
    }

    // Update is called once per frame
    void Update()
    {
       while(!OnOff && canStart) 
            StartCoroutine(OnOffStove(waitTime));
    }

    IEnumerator OnOffStove(float waitTime)
    { OnOff = true;
        
        yield return new WaitForSeconds(waitTime);
        GetComponent<SphereCollider>().enabled = true;
        Fire.Play();
        yield return new WaitForSeconds(waitTime);
        GetComponent<SphereCollider>().enabled = false;
        Fire.Stop();
        print("op");
        OnOff = false;
    }

    IEnumerator Delay(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        canStart = true;

    }
}
