using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{

    public PlayerManagerScript M_playerManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
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

    public void Level01()
    {
        
        SceneManager.LoadScene("Level_01");
        M_playerManagerScript.ResetAbilities();
        PlayerPrefs.SetInt("tute", 0);
        
    }

    public void Cutscene()
    {
        SceneManager.LoadScene("CutsceneIntro");
        PlayerPrefs.SetInt("tute", 0);
    }
}
