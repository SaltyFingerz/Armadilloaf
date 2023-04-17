using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionSFX : MonoBehaviour
{
    [SerializeField] AudioClip[] PCollided;
    public AudioSource PHit;
    private bool m_canHit = false;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            m_canHit = false;
        }
        if (!m_canHit)
        {
            StartCoroutine(waitForP());
        }
    }

    IEnumerator waitForP()
    {
        yield return new WaitForSeconds(1);
        m_canHit = true;

    }

    public void PHitSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = PCollided[UnityEngine.Random.Range(0, PCollided.Length)];
        PHit.PlayOneShot(clip);
        m_canHit = false;
        StartCoroutine(waitForP());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_canHit)
        {
            PHitSound();
        }
    }
}
