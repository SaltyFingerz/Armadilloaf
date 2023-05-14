using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeAudioScript : MonoBehaviour
{
    [SerializeField] AudioClip[] PipeCollided;
    AudioSource PipeHit;
    private bool m_canHit;
    // Start is called before the first frame update
    void Start()
    {
        PipeHit = GetComponent<AudioSource>();
        StartCoroutine(waitForPipes());
    }

    IEnumerator waitForPipes()
    {
        yield return new WaitForSeconds(2);
        m_canHit = true;

    }

    public void PipeHitSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = PipeCollided[UnityEngine.Random.Range(0, PipeCollided.Length)];
        PipeHit.PlayOneShot(clip);
        m_canHit = false;
        StartCoroutine(waitForPipes());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_canHit)
        {
            PipeHitSound();
        }
    }

}
