using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PrototypePlayerMovement M_PPlayerMovment;


    public GameObject M_movePrompt;
    public GameObject M_jumpPrompt;
    public GameObject M_MyChildCanWalk;
    //public GameObject M_launchPrompt;
    public GameObject M_launchAimPrompt;
    public GameObject M_launchShoot;
    public GameObject M_launchShoot2;
    public GameObject M_walkPrompt;
    public GameObject M_FreeCamEntryPrompt;
    public GameObject M_FreeCamMousePrompt;
    public GameObject M_FreeCamHeightPrompt;
    public GameObject M_FreeCamWASDPrompt;
    public GameObject M_FreeCamExitPrompt;

 public GameObject M_BoostPrompt;
    public GameObject M_TiltPrompt;
    public GameObject M_shrinkPrompt;
    public GameObject M_curlWorldPrompt;
    public GameObject M_BananaPrompt;
    public GameObject M_goalArrow;
    public GameObject M_BallPlayer;
    public GameObject M_FreeMovePlayer;

    public GameObject M_Walker;
    public GameObject M_Ball;
    private int  m_initialState; // 0 walker and 1 ball
    private int m_currentState; // 0 walker and 1 ball
    private float m_timerSeconds = 0f;
    private bool m_stateChanged;

    bool m_Wpressed;
    bool m_Apressed;
    bool m_Spressed;
    bool m_Dpressed;

    bool m_Qpressed;
    bool m_Epressed;
    bool m_walkPromptShown;
    bool m_launched;

    void Start()
    {
       
    }

    public void CloseIntro()
    {
        
        print("yas");
        Cursor.lockState = CursorLockMode.Locked;
        M_movePrompt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(M_Walker.activeSelf)
        {
            print("walker active");
            M_launchAimPrompt.SetActive(false);
            M_launchShoot.SetActive(false);
            M_launchShoot2.SetActive(false);
          
        }

        
       
      if(M_curlWorldPrompt.activeSelf)
        {
           // M_launchAimPrompt.SetActive(false);
            M_launchShoot.SetActive(false);
            if(!M_Walker.activeSelf)
            {
                M_launchAimPrompt.SetActive(true);
               
                StartCoroutine(NextPrompt0(M_launchShoot, M_launchAimPrompt));
                //  M_curlWorldPrompt.SetActive(false);
            }
        }


      if(M_launchShoot.activeSelf)
        {
            M_curlWorldPrompt.SetActive(false );
            M_launchAimPrompt.SetActive(false);

            if (Input.GetMouseButton(0))
            {
                StopCoroutine(NextPrompt0(M_launchShoot, M_launchAimPrompt));

                StartCoroutine(NextPrompt2(M_launchShoot2, M_launchShoot));
                
            }
        }

      if(M_launchShoot2.activeSelf)
        {
            M_launchShoot.SetActive(false);
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
           
            M_movePrompt.SetActive(true);
        }


        if((m_Wpressed || m_Apressed || m_Spressed || m_Dpressed) && M_movePrompt.activeSelf)
        {
            M_movePrompt.SetActive(false);
            M_MyChildCanWalk.SetActive(true);
            print("hohoho");
           // M_jumpPrompt.SetActive(true);
     

        }

        if (M_MyChildCanWalk.activeSelf)
        {
            StartCoroutine(NextPrompt(M_BananaPrompt, M_MyChildCanWalk));
        }

        IEnumerator NextPrompt(GameObject gameObjOpen, GameObject gameObjClose)
        {
            if(M_Walker.activeSelf)
            {
                m_initialState = 0;
            }
            else if(M_Ball.activeSelf)
            {
                m_initialState = 1;
            }
            yield return new WaitForSeconds(1.5f);

            if (M_Walker.activeSelf)
            {
                m_currentState = 0;
            }
            else if (M_Ball.activeSelf)
            {
                m_currentState = 1;
            }

            if(m_initialState == m_currentState)
            {
                m_stateChanged = false;
            }
            else if (m_initialState != m_currentState)
            {
                m_stateChanged = true;
            }

            if (!m_stateChanged)
            {
                gameObjClose.SetActive(false);
                gameObjOpen.SetActive(true);
            }
            
        }

        IEnumerator NextPrompt0(GameObject gameObjOpen, GameObject gameObjClose)
        {
            M_curlWorldPrompt.SetActive(false);
            M_launchShoot.SetActive(false);
            if (M_Walker.activeSelf)
            {
                m_initialState = 0;
            }
            else if (M_Ball.activeSelf)
            {
                m_initialState = 1;
            }
            yield return new WaitForSeconds(2f);

            if (M_Walker.activeSelf)
            {
                m_currentState = 0;
            }
            else if (M_Ball.activeSelf)
            {
                m_currentState = 1;
            }

            if (m_initialState == m_currentState)
            {
                m_stateChanged = false;
            }
            else if (m_initialState != m_currentState)
            {
                m_stateChanged = true;
            }

            if (!m_stateChanged)
            {
                gameObjClose.SetActive(false);
                gameObjOpen.SetActive(true);
            }

        }


        IEnumerator NextPrompt2(GameObject gameObjOpen, GameObject gameObjClose)
        {
            if (M_Walker.activeSelf)
            {
                m_initialState = 0;
            }
            else if (M_Ball.activeSelf)
            {
                m_initialState = 1;
            }
            yield return new WaitForSeconds(1.5f);

            if (M_Walker.activeSelf)
            {
                m_currentState = 0;
            }
            else if (M_Ball.activeSelf)
            {
                m_currentState = 1;
            }

            if (m_initialState == m_currentState)
            {
                m_stateChanged = false;
            }
            else if (m_initialState != m_currentState)
            {
                m_stateChanged = true;
            }

            if (!m_stateChanged)
            {
                gameObjClose.SetActive(false);
                gameObjOpen.SetActive(true);
            }

        }



        if (!M_BallPlayer.activeSelf && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            m_timerSeconds = 0;

        }

        

        if (Input.GetKey(KeyCode.Space) && M_jumpPrompt.activeSelf)
        {
            M_jumpPrompt.SetActive(false);
          //  M_launchPrompt.SetActive(true);
        }

     /*   if ((Input.GetMouseButtonDown(1)) && PrototypePlayerMovement.M_InLaunchZone) // && M_launchPrompt.activeSelf)
        {
           
         //   M_launchPrompt.SetActive(false);
         M_curlWorldPrompt.SetActive(false);
            M_launchAimPrompt.SetActive(true);
            //StartCoroutine(NextPrompt(M_launchShoot, M_launchAimPrompt));
        }
     */

        if ((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space)) && M_launchAimPrompt.activeSelf)
        {
           
            M_launchAimPrompt.SetActive(false);
            StopCoroutine(NextPrompt0(M_launchShoot, M_launchAimPrompt));
            M_launchShoot.SetActive(true);

            //  StartCoroutine(ExitBall());
        }

       // IEnumerator ExitBall()
     //   {
         
           /* yield return new WaitForSeconds(6);
            if (!M_TiltPrompt.activeSelf)
            {
                M_walkPrompt.SetActive(true);
            }
           */
     //   }

        if((Input.GetMouseButtonDown(1) || !M_BallPlayer.activeSelf )&& M_walkPrompt.activeSelf)
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
     

       if(Input.GetKeyDown(KeyCode.C) && M_FreeCamEntryPrompt.activeSelf)
        {
            m_timerSeconds = 0;
            M_FreeCamMousePrompt.SetActive(true);
            if (M_FreeMovePlayer.activeSelf)
            {
                M_goalArrow.SetActive(true);
            }
            
            M_FreeCamEntryPrompt.SetActive(false);

            StartCoroutine(NextPrompt(M_FreeCamWASDPrompt, M_FreeCamMousePrompt));
        }

       if(M_FreeCamWASDPrompt.activeSelf && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) )
       {
            StartCoroutine(NextPrompt(M_FreeCamHeightPrompt, M_FreeCamWASDPrompt));
       }

       if(M_FreeCamHeightPrompt.activeSelf && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)))
            {
            StartCoroutine(NextPrompt(M_FreeCamExitPrompt, M_FreeCamHeightPrompt));
        }

        if (!M_FreeMovePlayer.activeSelf)
        {
            M_goalArrow.SetActive(false);
        }
        else
        {
            M_goalArrow.SetActive(true);
        }




       

      /*  if ((m_Qpressed || m_Epressed) && M_freeControl.activeSelf)
        {
            M_closePrompt.SetActive(true);
            M_freeControl.SetActive(false);
        }
      */


        if (Input.GetKeyDown(KeyCode.C) && M_FreeCamExitPrompt.activeSelf)
        {
            M_FreeCamExitPrompt.SetActive(false);
        }


        if(M_BoostPrompt.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(NextPrompt(M_TiltPrompt, M_BoostPrompt));
            }

        }

        if(M_TiltPrompt.activeSelf || M_BoostPrompt.activeSelf)
        {
            if(!M_BallPlayer.activeSelf)
            {
                M_TiltPrompt.SetActive(false);
                M_BoostPrompt.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && M_TiltPrompt.activeSelf)
            {
                StartCoroutine(DeactivateTiltPrompt());
            }
        }

        IEnumerator DeactivateTiltPrompt()
        {
            yield return new WaitForSeconds(2);
            M_TiltPrompt.SetActive(false);
            if(M_BallPlayer.activeSelf)
            {
                M_walkPrompt.SetActive(true);
            }
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
