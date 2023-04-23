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
    [SerializeField] AudioClip m_pickupSoundClip;
    public AudioSource M_pickupSound;

    [SerializeField] AudioClip[] m_biteSounds;
    public AudioSource M_BiteSound;

    [SerializeField] AudioClip[] m_Burps;
    public AudioSource M_Burp;
    public static int M_FruitCollected = 0;
    public PlayerManagerScript M_PlayerManager;

    private void Start()
    {
        initPos = transform.position;

    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * freqHor) * ampHor + initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, Mathf.Sin(Time.time * freqDep) * ampDep + initPos.z);
        
        if(M_RotateOnAllAxes)
        {
            if (transform.GetChild(1) != null)
            {
                transform.GetChild(1).gameObject.transform.Rotate(new Vector3(0.2f, 0.8f, 0.2f));
            }
        }
        else if(M_RotateOnYAxis)
        {
            if (transform.GetChild(1) != null)
            {
                transform.GetChild(1).gameObject.transform.Rotate(new Vector3(0, 0.8f, 0));
            }
        }

       
        
    }

    public void PlayPickupSound()
    {
        M_pickupSound.PlayOneShot(m_pickupSoundClip);
        GetComponent<SphereCollider>().enabled = false;
        // AudioClip clip = m_biteSounds[UnityEngine.Random.Range(0, m_biteSounds.Length)];
        M_BiteSound.Play();
        StartCoroutine(waitToBurp());
        M_PlayerManager.M_FruitCollected ++;
    }

    IEnumerator waitToBurp()
    {
        yield return new WaitForSeconds(1);
        AudioClip clip = m_Burps[UnityEngine.Random.Range(0, m_Burps.Length)];
        M_Burp.PlayOneShot(clip);
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
