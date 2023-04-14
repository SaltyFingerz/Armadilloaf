using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLevel1());
    }

    IEnumerator LoadLevel1()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Level_01");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
