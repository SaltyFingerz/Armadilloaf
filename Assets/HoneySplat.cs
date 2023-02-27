using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneySplat : MonoBehaviour
{
    public GameObject HoneyImpact;
    public GameObject HoneyTrail;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall")) //also check if player not on ground
        {
            print("splat!");
            HoneyImpact.SetActive(true);
            HoneyTrail.SetActive(true);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("wall"))
        {
            print("splat!");
            HoneyImpact.SetActive(true);
            HoneyTrail.SetActive(true);
        }
    }



}
