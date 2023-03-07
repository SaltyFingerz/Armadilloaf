using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManagerScript : UIManagerScript
{
    public Canvas M_canvas;
    public GameObject M_playerManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused
        if (Time.timeScale > 0.1)
        {
            return;
        }

        if (Input.GetButtonUp("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            Resume();
        }
    }

    public void Resume()
    {
        M_canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        M_playerManager.GetComponent<PlayerManagerScript>().Resume();
    }

    public void Paused()
    {
        M_canvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
