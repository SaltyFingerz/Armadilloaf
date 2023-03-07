using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject M_introPrompt;
    public GameObject M_jumpPrompt;
    public GameObject M_launchPrompt;
    public GameObject M_launchAimPrompt;
    public GameObject M_walkPrompt;
    public GameObject M_freeCamPrompt;

    public GameObject M_goalArrow;

    bool m_Wpressed;
    bool m_Apressed;
    bool m_Spressed;
    bool m_Dpressed;
  

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            m_Wpressed = true;
        if (Input.GetKey(KeyCode.A))
            m_Apressed = true;
        if (Input.GetKey(KeyCode.S))
            m_Spressed = true;
        if (Input.GetKey(KeyCode.D))
            m_Dpressed = true;

        if(m_Wpressed && m_Apressed && m_Spressed && m_Dpressed && M_introPrompt.activeSelf)
        {
            M_introPrompt.SetActive(false);
            M_jumpPrompt.SetActive(true);
          

        }

        if(Input.GetKey(KeyCode.Space) && M_jumpPrompt.activeSelf)
        {
            M_jumpPrompt.SetActive(false);
            M_launchPrompt.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Q) && M_launchPrompt.activeSelf)
        {
           
            M_launchPrompt.SetActive(false);
            M_launchAimPrompt.SetActive(true);
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && M_launchAimPrompt.activeSelf)
        {

            StartCoroutine(ExitBall());
        }

        IEnumerator ExitBall()
        {
            
            M_launchAimPrompt.SetActive(false);
            yield return new WaitForSeconds(2);
            M_walkPrompt.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Q) && M_walkPrompt.activeSelf)
        {
            M_walkPrompt.SetActive(false);
        }


    }
}
