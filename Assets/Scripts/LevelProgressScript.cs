using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressScript : MonoBehaviour
{
    public GameObject M_FinishUI;
    public PlayerLaunchScript M_playerLaunchScript;
    public RenderingScript M_renderScript;
    public RenderingScript M_RenderSets;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            M_RenderSets.BlurBackground();
            Time.timeScale = 0.0f;
            M_FinishUI.SetActive(true);
            M_FinishUI.GetComponent<LevelFinishScript>().UpdateScores();
            M_FinishUI.GetComponent<LevelFinishScript>().UpdateStars();
        }
    }
}
