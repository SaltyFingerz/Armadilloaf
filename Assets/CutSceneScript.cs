using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneScript : MonoBehaviour
{
    void Start()
    {
        StopAllCoroutines();
        if (SceneManager.GetSceneByName("Level_01").isLoaded)
        {
            SceneManager.LoadScene("Level_01");
           
        }
        else
        {
            StartCoroutine(LoadLevel1());
        }
    }

    IEnumerator LoadLevel1()
    {
      
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
       
    }
}
