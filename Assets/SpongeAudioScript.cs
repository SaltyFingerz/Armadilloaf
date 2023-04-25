using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeAudioScript : MonoBehaviour
{
    [SerializeField] AudioClip[] BoxCollided;
    AudioSource BoxHit;
    private bool m_canHit;
    // Start is called before the first frame update
    void Start()
    {
        BoxHit = GetComponent<AudioSource>();
        StartCoroutine(waitForBox());
    }

    IEnumerator waitForBox()
    {
        yield return new WaitForSeconds(0.5f);
        m_canHit = true;

    }

    public void BoxHitSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = BoxCollided[UnityEngine.Random.Range(0, BoxCollided.Length)];
        BoxHit.PlayOneShot(clip);
        m_canHit = false;
        StartCoroutine(waitForBox());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_canHit)
        {
            BoxHitSound();
        }
    }
}
