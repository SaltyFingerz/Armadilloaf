using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLaunchTuteController : MonoBehaviour
{
    public enum TutorialState { hidden, uncurl, curl, aim, power, release }

    [SerializeField] private GameObject m_uncurlPrompt;
    [SerializeField] private GameObject m_curlPrompt;
    [SerializeField] private GameObject m_aimPrompt;
    [SerializeField] private GameObject m_powerPrompt;
    [SerializeField] private GameObject m_releasePrompt;
    
    private TutorialState m_currentState;

    IEnumerator AimToPower()
    {
        yield return new WaitForSeconds(2);
       ChangeState(FirstLaunchTuteController.TutorialState.power);
    }

    public void ChangeState(TutorialState newState) 
    {
        if (newState == m_currentState)
            return;

        switch (newState)
        {
            case TutorialState.hidden:
            {
                m_uncurlPrompt.SetActive(false);
                m_curlPrompt.SetActive(false);
                m_aimPrompt.SetActive(false);
                m_powerPrompt.SetActive(false);
                m_releasePrompt.SetActive(false);
                    StopCoroutine(AimToPower());
                    m_currentState = TutorialState.hidden;
                break;
            }
            case TutorialState.uncurl:
            {
                    //if (m_currentState != TutorialState.hidden && m_currentState != TutorialState.curl)
                    //{
                    //    Debug.LogError($"{nameof(FirstLaunchTuteController)}: Unable to advance to uncul to state from {m_currentState}");
                    //    break;
                    //}
                   // m_uncurlPrompt.SetActive(false);
                    m_aimPrompt.SetActive(false);
                    m_powerPrompt.SetActive(false);
                    m_releasePrompt.SetActive(false);
                    m_uncurlPrompt.SetActive(true);
                    m_currentState = TutorialState.uncurl;
                    StopCoroutine(AimToPower());
                    break;
            }

            case TutorialState.curl:
                {
                    //if (m_currentState != TutorialState.hidden && m_currentState != TutorialState.uncurl)
                    //{
                    //    Debug.LogError($"{nameof(FirstLaunchTuteController)}: Unable to advance to curl to state from {m_currentState}");
                    //    break;
                    //}
                    m_curlPrompt.SetActive(true);
                    m_uncurlPrompt.SetActive(false);

                   
                    m_aimPrompt.SetActive(false);
                    m_powerPrompt.SetActive(false);
                    m_releasePrompt.SetActive(false);
                    m_currentState = TutorialState.curl;
                    StopCoroutine(AimToPower());
                    break;
                }

            case TutorialState.aim:

                {
                    if (m_currentState != TutorialState.hidden && m_currentState != TutorialState.curl && m_currentState != TutorialState.uncurl && m_currentState != TutorialState.aim)
                    {
                       
                        break;
                    }





                    m_powerPrompt.SetActive(false);
                    m_releasePrompt.SetActive(false);
                    m_curlPrompt.SetActive(false);
                    m_uncurlPrompt.SetActive(false);
                    m_aimPrompt.SetActive(true);
                    m_currentState = TutorialState.aim;
                    StartCoroutine(AimToPower());
                    break;
                }

            case TutorialState.power:
                {

                    if (m_currentState != TutorialState.aim && m_currentState != TutorialState.power)
                    {
                       
                        break;
                    }

                    m_curlPrompt.SetActive(false);
                    m_uncurlPrompt.SetActive(false);
                    m_aimPrompt.SetActive(false);
                    m_releasePrompt.SetActive(false);
                    m_powerPrompt.SetActive(true);
                    m_currentState = TutorialState.power;
                    break;

                   
                }

            case TutorialState.release:
                {
                    //if(m_currentState != TutorialState.power)
                    //{
                    //    Debug.LogError($"{nameof(FirstLaunchTuteController)}: Unable to advance to release to state from {m_currentState}");
                    //    break;
                    //}
                    m_curlPrompt.SetActive(false);
                    m_uncurlPrompt.SetActive(false);
                    m_aimPrompt.SetActive(false);
                    m_powerPrompt.SetActive(false);
                    m_releasePrompt.SetActive(true);
                    m_currentState = TutorialState.release;
                    StopCoroutine(AimToPower());
                    break;
                }



        }
    }
}
