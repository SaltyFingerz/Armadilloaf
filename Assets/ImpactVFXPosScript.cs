using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXPosScript : MonoBehaviour
{
    public Transform M_BallPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(M_BallPosition.position.x, transform.position.y, M_BallPosition.position.z);
    }
}
