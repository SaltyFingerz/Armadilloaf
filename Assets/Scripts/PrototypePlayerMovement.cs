using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerMovement : MonoBehaviour
{
    public CinemachineVirtualCamera M_walkCamera;
    public GameObject M_playerManager;
    private CharacterController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = 1;
    [SerializeField] public float m_jumpHeight = 1.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 2.0f;

    float m_mouseSensitivity = 1000.0f;
    float m_rotationMouseX;
    private void Start()
    {
        m_controller = gameObject.GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            m_isHittingWall = true;
        }
        else
        {
            m_isHittingWall = false;  
        }

        Rigidbody m_rb = hit.collider.attachedRigidbody;
        if (m_rb != null && !m_rb.isKinematic)
        {
            m_rb.velocity = hit.moveDirection * m_pushForce;
        }
    }

    void Update()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        m_groundedPlayer = m_controller.isGrounded;
        HandleInput();

        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }


        if (m_groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }



        if (m_isHittingWall)
        {
            switch (M_playerManager.GetComponent<PlayerManagerScript>().M_abilityState)
            {
                case (int)PlayerManagerScript.AbilityState.honey:
                    playerVelocity = new Vector3(playerVelocity.x, m_slowSlide, playerVelocity.z);
                    break;

                case (int)PlayerManagerScript.AbilityState.normal:
                    playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += m_gravityValue * Time.deltaTime, playerVelocity.z);
                    break;

                default:
                    break;
            }
        }
        else
        { 
        playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += m_gravityValue * Time.deltaTime, playerVelocity.z);
        }
        m_controller.Move(playerVelocity * Time.deltaTime);
        m_isHittingWall = false;  
    }

    private void HandleInput()
    {
        // Mouse RB is dragged, calculate player rotation from the mouse position difference between frames
        m_rotationMouseX = -Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_mouseSensitivity;
        m_controller.transform.Rotate(Vector3.up, -m_rotationMouseX);

        Vector3 l_movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_playerSpeed = 8.0f;
        }
        else
        {
            m_playerSpeed = 2.0f;
        }

        // movement with AWSD keys
        l_movementDirection = -Input.GetAxis("Vertical") * this.transform.forward;
        l_movementDirection -= Input.GetAxis("Horizontal") * this.transform.right;
        m_controller.Move(l_movementDirection * Time.deltaTime * m_playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            if (m_groundedPlayer)
            { 
            playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
            }
        }
    }

    public void SetSize(float a_size)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
        switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
        {
            case (int)PlayerManagerScript.SizeState.big:
                m_pushForce = 4.0f;
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                break;

            default:
                Debug.Log("Error! Did you forget to set a size state?");
                break;
        }
    }
}
