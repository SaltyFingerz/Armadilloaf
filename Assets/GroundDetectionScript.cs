using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectionScript : MonoBehaviour
{
    public static bool M_IsGrounded;


    private void OnTriggerStay(Collider other)
    {
        if(!other.isTrigger)
        M_IsGrounded = true;
    }


    private void OnTriggerExit(Collider other)
    {
        M_IsGrounded = false;
    }
}
