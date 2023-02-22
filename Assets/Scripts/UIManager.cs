using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    Canvas M_canvas;

    // Start is called before the first frame update
    void Start()
    {
        M_canvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartScene()
    {
        SceneManager.LoadScene("Launching");
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
