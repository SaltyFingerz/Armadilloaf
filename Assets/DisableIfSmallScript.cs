using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfSmallScript : MonoBehaviour
{
    public PlayerManagerScript M_PlMan;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {if(M_PlMan.M_sizeState ==0)
        {
            gameObject.SetActive(false);
        }
        
    }
}
