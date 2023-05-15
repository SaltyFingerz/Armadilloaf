using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneScript : MonoBehaviour
{
    float m_timer;
    float m_videoTime = 3.0f;

    void Start()
    {
        m_timer = 0.0f;
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer > m_videoTime )
        {
            SceneManager.LoadScene("Level_01");
        }

    }
}
