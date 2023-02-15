using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{
    enum ArmadilloState { walk, launching};
    ArmadilloState m_state = ArmadilloState.walk;

    public GameObject M_launchingPlayer;
    public GameObject M_walkingPlayer;

    public CinemachineFreeLook M_launchingBaseCamera;
    public CinemachineVirtualCamera M_walkingBaseCamera;

    const int m_launchingCameraMaxPriority = 9;
    const int m_walkingCameraMaxPriority = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartWalking();
        M_walkingBaseCamera.Priority = m_walkingCameraMaxPriority;
        M_launchingBaseCamera.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // state changing
        if (Input.GetKeyUp(KeyCode.Q))
        {
            switch(m_state)
            {
                case ArmadilloState.walk:
                    StartLaunching();
                    break;

                case ArmadilloState.launching:
                    StartWalking();
                    break;

                default:
                    break;
            }
        }
    }

    void StartWalking()
    {
        // change camera
        M_walkingBaseCamera.Priority = m_walkingCameraMaxPriority;
        M_launchingBaseCamera.Priority = 0;

        m_state= ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
        M_launchingPlayer.SetActive(false);
    }

    void StartLaunching()
    {
        // change camera
        M_launchingBaseCamera.Priority = m_launchingCameraMaxPriority;
        M_walkingBaseCamera.Priority = 0;

        m_state = ArmadilloState.launching;

        M_launchingPlayer.transform.position = M_walkingPlayer.transform.position;
        M_launchingPlayer.SetActive(true);
        M_walkingPlayer.SetActive(false);
    }
}
