using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerManagerScript;

public partial class PlayerManagerScript : MonoBehaviour
{
    enum ArmadilloState { walk, launching};
    ArmadilloState m_state = ArmadilloState.walk;
    public bool M_isFreeFlying = false;

    public GameObject M_launchingPlayer, M_walkingPlayer, M_freeFlyingPlayer;
    public CinemachineFreeLook M_launchingBaseCamera;
    public CinemachineVirtualCamera M_walkingBaseCamera, M_freeMovementCamera;

    const int m_launchingCameraMaxPriority = 8;
    const int m_walkingCameraMaxPriority = 9;
    const int m_freeMovementCameraMaxPriority = 10;
    public enum SizeState { small = 0, normal = 1, big = 2 };
    public float[] M_sizes = { 0.25f, 0.5f, 1.0f };
    public int M_sizeState = (int)SizeState.normal;

    public PauseManagerScript M_UIManager;
    bool m_justUnpaused;

    // Start is called before the first frame update
    void Start()
    {
        m_justUnpaused = false;
        M_UIManager = FindObjectOfType<PauseManagerScript>();
        Cursor.lockState = CursorLockMode.Locked;

        // change camera
        M_launchingBaseCamera.Priority = 0;
        M_freeMovementCamera.Priority = 0;
        M_walkingBaseCamera.Priority = m_walkingCameraMaxPriority;

        m_state = ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
        M_sizeState = (int)SizeState.normal;
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetSize(M_sizes[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetSize(M_sizes[M_sizeState]);
        M_UIManager.Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_justUnpaused)
        {
            m_justUnpaused = false;
            return;
        }
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
        if (Input.GetKeyUp(KeyCode.Q) && !M_isFreeFlying)
        {
            StateCheck();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (M_isFreeFlying)
            {
                M_isFreeFlying = false;
                switch (m_state)
                {
                    case ArmadilloState.walk:
                        StartWalking();
                        break;
                    case ArmadilloState.launching:
                        StartLaunching();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                StartFlying();
            }
        }
        // prototype restart, to do: have this be automatic upon failure
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("FailScreen");
        }
        if(Input.GetButtonUp("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            M_UIManager.enabled = true;
            M_UIManager.Pasued();
        }
    }

    public void Resume()
    {
        M_UIManager.enabled = false;
        m_justUnpaused = true;
        Time.timeScale = 1;
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

    public void StartWalking()
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

    public void StartLaunching()
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
        M_freeFlyingPlayer.SetActive(true);
        M_isFreeFlying = true;
        switch (m_state)
        {
            case ArmadilloState.walk:
                M_freeFlyingPlayer.transform.position = M_walkingPlayer.transform.position;
                break;
            case ArmadilloState.launching:
                M_freeFlyingPlayer.transform.position = M_launchingPlayer.transform.position;
                break;

            default:
                break;
        }
    }

    public void Grow()
    {
        M_sizeState++;
        if (M_sizeState > 2)
        {
            M_sizeState = 2;
        }
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

    //acquire property of bounciness with Jelly pick-up, called in PowerUp_SizeChanger (script)
    public void Jellify()
    {
        {M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = 0.9f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 0.6f;
        }
    }
    //acquire property of stickiness with Jelly pick-up, called in PowerUp_SizeChanger (script)
    public void Honify()
    {
        {
            M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 50f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 50f;
        }
    }
}
