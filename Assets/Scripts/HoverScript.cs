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
    public bool M_RotateOnAllAxes = false;
    public bool M_RotateOnYAxis = false;
    private void Start()
    {
        initPos = transform.position;

    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * freqHor) * ampHor + initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, Mathf.Sin(Time.time * freqDep) * ampDep + initPos.z);
        
        if(M_RotateOnAllAxes)
        {
            transform.GetChild(1).gameObject.transform.Rotate(new Vector3(0.2f, 0.8f, 0.2f));
        }
        else if(M_RotateOnYAxis)
        {
            transform.GetChild(1).gameObject.transform.Rotate(new Vector3(0, 0.8f, 0));
        }
    }

    public void StopParticles()
    {
        GlowBobbles.Stop();
        GetComponentInParent<MeshRenderer>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(DisableCollecible());
    }

    IEnumerator DisableCollecible()
    {
        yield return new WaitForSeconds(2f);
       gameObject.SetActive(false);
    }
   
}
