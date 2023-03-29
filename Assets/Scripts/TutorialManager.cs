using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PrototypePlayerMovement M_PPlayerMovment;

    public GameObject M_introPrompt;
    public GameObject M_movePrompt;
    public GameObject M_jumpPrompt;
    //public GameObject M_launchPrompt;
    public GameObject M_launchAimPrompt;
    public GameObject M_walkPrompt;
    public GameObject M_freeCamPrompt;
    public GameObject M_freeControl;
    public GameObject M_closePrompt;
    public GameObject M_TiltPrompt;
    public GameObject M_shrinkPrompt;
    public GameObject M_curlWorldPrompt;
    public GameObject M_BananaPrompt;
    public GameObject M_goalArrow;
    public GameObject M_BallPlayer;
    public GameObject M_FreeMovePlayer;

    private float m_timerSeconds = 0f;


    private bool m_uncurled = false;

    bool m_Wpressed;
    bool m_Apressed;
    bool m_Spressed;
    bool m_Dpressed;

    bool m_Qpressed;
    bool m_Epressed;

    bool m_launched;

    void Start()
    {
       
    }

    public void CloseIntro()
    {
        M_introPrompt.SetActive(false);
        print("yas");
        Cursor.lockState = CursorLockMode.Locked;
        M_movePrompt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(M_introPrompt.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
      
       
        m_timerSeconds += Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            m_Wpressed = true;
        if (Input.GetKey(KeyCode.A))
            m_Apressed = true;
        if (Input.GetKey(KeyCode.S))
            m_Spressed = true;
        if (Input.GetKey(KeyCode.D))
            m_Dpressed = true;

        if(PlayerPrefs.GetInt("tute") == 1)
        {
            M_introPrompt.SetActive(false);
            M_movePrompt.SetActive(true);
        }


        if((m_Wpressed || m_Apressed || m_Spressed || m_Dpressed) && M_movePrompt.activeSelf)
        {
            M_movePrompt.SetActive(false);
            M_BananaPrompt.SetActive(true);
            print("hohoho");
           // M_jumpPrompt.SetActive(true);
     

        }

        if(Input.GetKey(KeyCode.Space) && M_jumpPrompt.activeSelf)
        {
            M_jumpPrompt.SetActive(false);
          //  M_launchPrompt.SetActive(true);
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && M_PPlayerMovment.M_InLaunchZone) // && M_launchPrompt.activeSelf)
        {
           
         //   M_launchPrompt.SetActive(false);
            M_launchAimPrompt.SetActive(true);
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space)) && M_launchAimPrompt.activeSelf)
        {

            StartCoroutine(ExitBall());
        }

        IEnumerator ExitBall()
        {
            m_launched = true;
            M_launchAimPrompt.SetActive(false);
            yield return new WaitForSeconds(6);
            if (!M_TiltPrompt.activeSelf && !m_uncurled)
            {
                M_walkPrompt.SetActive(true);
                m_uncurled = true;
}
        }

        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || !M_BallPlayer.activeSelf )&& M_walkPrompt.activeSelf)
        {
            M_walkPrompt.SetActive(false);
           // StartCoroutine(ShowFreeCamPrompt());
        }

       // IEnumerator ShowFreeCamPrompt()
        //{
        //    M_walkPrompt.SetActive(false);
           // yield return new WaitForSeconds(3f);
          //  if (!M_launchAimPrompt.activeSelf) //&& !M_launchPrompt.activeSelf && )
          //  {
          //      M_freeCamPrompt.SetActive(true);
          //  }
      //  }
     

       if(Input.GetKeyDown(KeyCode.C) && M_freeCamPrompt.activeSelf)
        {
            m_timerSeconds = 0;
            M_freeControl.SetActive(true);
            if (M_FreeMovePlayer.activeSelf)
            {
                M_goalArrow.SetActive(true);
            }
            
            M_freeCamPrompt.SetActive(false);

           
        }
        if (!M_FreeMovePlayer.activeSelf)
        {
            M_goalArrow.SetActive(false);
        }
        else
        {
            M_goalArrow.SetActive(true);
        }
        if(M_freeControl.activeSelf)
        {
            StartCoroutine(freeControlPromptTimer());
            if (Input.GetKey(KeyCode.Q))
                m_Qpressed = true;
            if (Input.GetKey(KeyCode.E))
                m_Epressed = true;
        }


        IEnumerator freeControlPromptTimer()
        {
            yield return new WaitForSeconds(5f);
            if (M_freeControl.activeSelf)
            {
                M_closePrompt.SetActive(true);
                M_freeControl.SetActive(false);
            }
        }

        if ((m_Qpressed || m_Epressed) && M_freeControl.activeSelf)
        {
            M_closePrompt.SetActive(true);
            M_freeControl.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.C) && M_closePrompt && m_timerSeconds > 1 )
        {
            M_closePrompt.SetActive(false);
        }


        if(M_TiltPrompt.activeSelf)
        {
            StartCoroutine(DeactivateTiltPrompt());
        }

        IEnumerator DeactivateTiltPrompt()
        {
            yield return new WaitForSeconds(10);
            M_TiltPrompt.SetActive(false);
        }

        if(M_shrinkPrompt.activeSelf)
        {
            if(Input.GetKey(KeyCode.E))
            {
                M_shrinkPrompt.SetActive(false);
            }
        }

    }
}
