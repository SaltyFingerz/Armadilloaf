using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionSFX : MonoBehaviour
{
    [SerializeField] AudioClip[] PCollided;
    [SerializeField] AudioClip[] M_Bounces;
    public GameObject M_PManager;
    public AudioSource PHit;
    private bool m_canHit = true;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    private void Update()
    {
        
      
    }

    IEnumerator waitForP()
    {
        yield return new WaitForSeconds(1f);
        m_canHit = true;

    }

    public void PHitSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = PCollided[UnityEngine.Random.Range(0, PCollided.Length)];
        PHit.PlayOneShot(clip);
        m_canHit = false;
        StartCoroutine(waitForP());
    }

    public void BounceSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = M_Bounces[UnityEngine.Random.Range(0, M_Bounces.Length)];
        PHit.PlayOneShot(clip);
        m_canHit = false;
        StartCoroutine(waitForP());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_canHit && M_PManager.GetComponent<PlayerManagerScript>().M_sizeState <2)
        {
            PHitSound();
        }

        else if (m_canHit && M_PManager.GetComponent<PlayerManagerScript>().M_sizeState == 2)
        {
            BounceSound();

        }

        else if (!m_canHit)
        {
            StartCoroutine(waitForP());
        }
    }
}
