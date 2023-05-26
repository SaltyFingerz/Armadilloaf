using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CutSceneScript : MonoBehaviour
{
    float m_timer;
    float m_videoTime = 31.0f;
    float M_dilloMove = 359.7f;
    float M_dilloStartPos = -176.1f;
    bool isLoading = false;
    public Slider M_slider;
    public GameObject M_loadingStuff;
    public GameObject M_Loadillo;
    public GameObject M_videoPlayer;
    public TextMeshProUGUI M_tip;

    void Start()
    {
        m_timer = 0.0f;
        Time.timeScale = 1.0f;
        M_slider.maxValue = 1.0f;
        M_slider.value = 0.0f;
        M_tip.text = RandomTip(Random.Range(1, 6));
        M_loadingStuff.SetActive(false);
    }

    public void LoadLevel(int a_sceneIndex)
    {
        StartCoroutine(LoadAsync(a_sceneIndex));
    }

    IEnumerator LoadAsync(int a_sceneIndex)
    {
        AsyncOperation l_operation = SceneManager.LoadSceneAsync(a_sceneIndex);
        //l_operation.allowSceneActivation = false;

        while (!l_operation.isDone)
        {
            float l_progress = Mathf.Clamp01(l_operation.progress / .9f);
            M_slider.value = l_progress;
            M_Loadillo.transform.localPosition = new Vector3(l_progress * (M_dilloMove + M_dilloStartPos), -205.7f, 0f);
            yield return null;
        }
    }

    private void Update()
    {
        HandleInput();
        if (!isLoading)
        {
            m_timer += Time.deltaTime;

        if(m_timer > m_videoTime )
        {
                LoadLevel(2);
                M_loadingStuff.SetActive(true);
            isLoading = true;
        }

        }

    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            M_videoPlayer.SetActive(false);
            LoadLevel(2);
            M_loadingStuff.SetActive(true);
            isLoading = true;
        }
    }

    public string RandomTip(int a_number)
    {
        string l_string;
        switch(a_number)
        {
            case 1:
                { 
                    l_string = "The dust bunnies only pick on those that are smaller than themselves.";
                    break;
                }
            case 2:
                {
                    l_string = "The smaller Rollio is, the faster he can go!";
                    break;
                }
            case 3:
                {
                    l_string = "Whatever you do, don't fall on the floor!";
                    break;
                }
            case 4:
                {
                    l_string = "Unless you have to navigate with precision, rolling is almost always a good idea!";
                    break;
                }
            case 5:
                {
                    l_string = "Did you know that you can launch in mid air?";
                    break;
                }
            default:
                {
                    l_string = "To kill for yourself is murder. To kill for your country is heroic. To kill for entertainment is harmless.";
            break;
                }
        }
        return l_string.ToUpper();
    }
}
