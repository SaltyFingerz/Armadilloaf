using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlythroughCam : MonoBehaviour
{
    public GameObject M_WalkCam;
    public GameObject M_HUD;

    // Start is called before the first frame update
    void Start()
    {
        M_WalkCam.SetActive(false);
  M_HUD.SetActive(false);
    }
    public void SwitchCams()
    {
        M_WalkCam.SetActive(true);
        gameObject.SetActive(false);
        M_HUD.SetActive(true);
    }

}
