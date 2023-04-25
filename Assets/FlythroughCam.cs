using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlythroughCam : MonoBehaviour
{
    public GameObject M_WalkCam;
    public GameObject M_HUD;
    public GameObject M_cutsceneSkip;

    // Start is called before the first frame update
    void Start()
    {
        M_cutsceneSkip.SetActive(true);
        M_WalkCam.SetActive(false);
        M_HUD.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwitchCams();
        }
    }
    public void SwitchCams()
    {
        M_WalkCam.SetActive(true);
        M_cutsceneSkip.SetActive(false);
        gameObject.SetActive(false);
        M_HUD.SetActive(true);
    }

}
