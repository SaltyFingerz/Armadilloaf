using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManagerScript;

public partial class PlayerManagerScript : MonoBehaviour
{
    enum ArmadilloState { walk, launching};
    ArmadilloState m_state = ArmadilloState.walk;
    bool m_isFreeFlying = false;

    public GameObject M_launchingPlayer, M_walkingPlayer, M_freeFlyingPlayer;
    public CinemachineFreeLook M_launchingBaseCamera, M_freeMovementCamera;
    public CinemachineVirtualCamera M_walkingBaseCamera;

    const int m_launchingCameraMaxPriority = 8;
    const int m_walkingCameraMaxPriority = 9;
    const int m_freeMovementCameraMaxPriority = 10;
    public enum SizeState { small = 0, normal = 1, big = 2 };
    public float[] M_sizes = { 0.5f, 1.0f, 2.0f };
    public int M_sizeState = (int)SizeState.normal;

    // Start is called before the first frame update
    void Start()
    {
        StartWalking();
    }

    // Update is called once per frame
    void Update()
    {
        // grow
        if (Input.GetKeyDown(KeyCode.T))
        {
            Grow();
        }
        // shrink
        if (Input.GetKeyDown(KeyCode.I))
        {
            Shrink();
        }

        // state changing
        if (Input.GetKeyUp(KeyCode.Q) && !m_isFreeFlying)
        {
            StateCheck();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (m_isFreeFlying)
            {
                m_isFreeFlying = false;
                StateCheck();
            }
            else
            {
                StartFlying();
            }
        }
        // prototype restart, to do: have this be automatic upon failure
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Reset");
            M_launchingPlayer.GetComponent<PlayerLaunchScript>().Reset();
            M_launchingPlayer.SetActive(false);
            M_walkingPlayer.SetActive(false);
            M_launchingPlayer.transform.position = new Vector3(0.0f, 2.0f, 0.0f);
            M_walkingPlayer.transform.position = new Vector3(0.0f, 2.0f, 0.0f);
            StartWalking();
        }
    }

    void StateCheck()
    {
        switch (m_state)
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

    void StartWalking()
    {
        // change camera
        M_launchingBaseCamera.Priority = 0;
        M_freeMovementCamera.Priority = 0;
        M_walkingBaseCamera.Priority = m_walkingCameraMaxPriority;

        m_state = ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().Reset();
        M_walkingPlayer.SetActive(true);
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetSize(M_sizes[M_sizeState]);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
    }

    void StartLaunching()
    {
        // change camera
        M_walkingBaseCamera.Priority = 0;
        M_freeMovementCamera.Priority = 0;
        M_launchingBaseCamera.Priority = m_launchingCameraMaxPriority;

        M_launchingPlayer.transform.position = M_walkingPlayer.transform.position;
        m_state = ArmadilloState.launching;
        
        M_launchingPlayer.SetActive(true);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetSize(M_sizes[M_sizeState]);
        M_walkingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
    }

    void StartFlying()
    {

        // change camera
        M_walkingBaseCamera.Priority = 0;
        M_launchingBaseCamera.Priority = 0;
        M_freeMovementCamera.Priority = m_freeMovementCameraMaxPriority;

        M_walkingPlayer.SetActive(false);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(true);

        m_isFreeFlying = true;
    }

    public void Grow()
    {
        M_sizeState = (M_sizeState + 1) % 3;
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetSize(M_sizes[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetSize(M_sizes[M_sizeState]);
    }

    public void Shrink()
    {
        M_sizeState--;
        if (M_sizeState < 0)
        {
            M_sizeState = 0;
        }
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetSize(M_sizes[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetSize(M_sizes[M_sizeState]);
    }
}
