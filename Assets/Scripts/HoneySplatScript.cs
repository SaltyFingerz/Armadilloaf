using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneySplatScript : MonoBehaviour
{
    public GameObject M_HoneyImpact;
    public GameObject M_HoneyTrail;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall")) //also check if player not on ground
        {
            M_HoneyImpact.SetActive(true);
            M_HoneyTrail.SetActive(true);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("wall"))
        {
            M_HoneyImpact.SetActive(true);
            M_HoneyTrail.SetActive(true);
        }
    }



}
