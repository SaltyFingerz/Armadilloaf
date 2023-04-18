using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillbboardScript : MonoBehaviour
{
 
    public GameObject M_TargetCam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

                Vector3 targetPosition = new Vector3(M_TargetCam.transform.position.x, M_TargetCam.transform.position.y, M_TargetCam.transform.position.z);
                transform.LookAt(targetPosition);
        
    }
}