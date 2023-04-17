using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAudioScript : MonoBehaviour
{
    [SerializeField] AudioClip[] CanCollided;
    AudioSource CanClash;
    // Start is called before the first frame update
    void Start()
    {
        CanClash = GetComponent<AudioSource>();
    }

    public void CanClashSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = CanCollided[UnityEngine.Random.Range(0, CanCollided.Length)];
        CanClash.PlayOneShot(clip);
    }


    private void OnCollisionEnter(Collision collision)
    {
        CanClashSound();
    }

   
}
