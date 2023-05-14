using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangSizeScript : MonoBehaviour
{
    public PlayerManagerScript M_PlManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(M_PlManager.M_sizeState ==0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        
        else if (M_PlManager.M_sizeState == 1)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else if (M_PlManager.M_sizeState == 1)
        {
            transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }
}

