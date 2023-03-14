using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectScriptMixerattatchment : MonoBehaviour
{
    public float M_rotationSpeed =1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3( 0, 0, M_rotationSpeed));
    }
}
