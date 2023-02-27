using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerMovement : MonoBehaviour
{
    public CinemachineVirtualCamera M_walkCamera;
    public PlayerManagerScript M_playerManagerScript;
    private CharacterController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = -0.05f;
    [SerializeField] private float m_jumpHeight = 1.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 2.0f;

    float m_mouseSensitivity = 1000.0f;
    float m_rotationMouseX;

    private void Start()
    {
        M_playerManagerScript = FindObjectOfType<PlayerManagerScript>();
        m_controller = gameObject.GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Wall")
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
        m_groundedPlayer = m_controller.isGrounded;


        HandleInput();

        if (M_playerManagerScript.M_isFreeFlying)
        {
            return;
        }


        if (m_groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }



        if (m_isHittingWall)
        {
            switch (M_playerManagerScript.M_sizeState)
            {
                case (int)PlayerManagerScript.SizeState.big:
                    playerVelocity = new Vector3(playerVelocity.x, m_slowSlide, playerVelocity.z);
                    break;

                case (int)PlayerManagerScript.SizeState.normal:
                    playerVelocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z);
                    break;

                case (int)PlayerManagerScript.SizeState.small:
                    playerVelocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z);
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
        m_controller.transform.Rotate(Vector3.up, m_rotationMouseX);

        Vector3 l_movementDirection = Vector3.zero;

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
    }
}
