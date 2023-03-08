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

    public void GolfScene()
    {
        SceneManager.LoadScene("Launching");
    }
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit()
    {
        Debug.Log("Game exit");
        Application.Quit();
    }

    public void KitchenScene()
    {
        SceneManager.LoadScene("KitchenAndLivingRoom");
    }
}
