using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
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

    public void KitchenScene()
    {
        SceneManager.LoadScene("KitchenAndLivingRoom");
    }
}
