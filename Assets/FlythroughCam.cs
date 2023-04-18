using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlythroughCam : MonoBehaviour
{
    public GameObject M_WalkCam;


    // Start is called before the first frame update
    void Start()
    {
        M_WalkCam.SetActive(false);
  
    }
    public void SwitchCams()
    {
        M_WalkCam.SetActive(true);
        gameObject.SetActive(false);
    }

}
