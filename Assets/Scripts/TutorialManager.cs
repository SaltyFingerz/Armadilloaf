using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstLaunchTuteController))]
public class TutorialManager : MonoBehaviour
{
    public PrototypePlayerMovement M_PPlayerMovment;
    public FirstLaunchTuteController M_FirstLaunchTute;

    public GameObject M_MyChildCanWalk;
    public GameObject M_movePrompt, M_jumpPrompt, M_BoostPrompt, M_TiltPrompt, M_shrinkPrompt,M_BananaPrompt, M_goalArrow;
    public GameObject M_BallPlayer, M_FreeMovePlayer;

    public GameObject M_Walker, M_Ball;
    private int  m_initialState, m_currentState;     // 0 walker and 1 ball
    private bool m_stateChanged;
    public static bool M_ShownTiltAndBoost;

    bool m_Wpressed, m_Apressed, m_Spressed, m_Dpressed;

    void Start()
    {
        M_FirstLaunchTute ??= GetComponent<FirstLaunchTuteController>();  
        M_FirstLaunchTute.ChangeState(FirstLaunchTuteController.TutorialState.hidden);
    }

    public void CloseIntro()
    {
        Cursor.lockState = CursorLockMode.Locked;
        M_movePrompt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
        }


        if(M_BoostPrompt.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(NextPrompt(M_TiltPrompt, M_BoostPrompt));
            }

        }

        if(M_TiltPrompt.activeSelf)
        {
            M_ShownTiltAndBoost = true;
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
