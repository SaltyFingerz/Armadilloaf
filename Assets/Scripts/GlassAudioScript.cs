using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassAudioScript : MonoBehaviour
{
    AudioSource m_glassBoink;
    bool m_canBoink = true;
    void Start()
    {
        m_glassBoink = GetComponent<AudioSource>();
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        m_glassBoink.Play();
        m_canBoink = false;
    }

    IEnumerator resetSound()
    {
        yield return new WaitForSeconds(1f);
        m_canBoink = true;
    }
 
}
