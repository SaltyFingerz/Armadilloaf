using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManagerScript : UIManagerScript
{
    public Canvas M_canvas;
    public PlayerManagerScript M_playerManager;
    // Start is called before the first frame update
    void Start()
    {
        M_playerManager = FindObjectOfType<PlayerManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.1)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        M_canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        M_playerManager.Resume();
    }

    public void Pasued()
    {
        M_canvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
