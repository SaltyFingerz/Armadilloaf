using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    public float amp;
    public float freq;
    public float ampHor;
    public float freqHor;
    public ParticleSystem GlowBobbles;
    public float ampDep;
    public float freqDep;
    Vector3 initPos;
    private void Start()
    {
        initPos = transform.position;

    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * freqHor) * ampHor + initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, Mathf.Sin(Time.time * freqDep) * ampDep + initPos.z);

    }

    public void StopParticles()
    {
        GlowBobbles.Stop();
        GetComponentInParent<MeshRenderer>().enabled = false;
        StartCoroutine(DisableCollecible());
    }

    IEnumerator DisableCollecible()
    {
        yield return new WaitForSeconds(2f);
       gameObject.SetActive(false);
    }
   
}
