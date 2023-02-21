using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePlayerMovement : MonoBehaviour
{
    public CinemachineVirtualCamera M_walkCamera;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    public float slowSlide = -0.05f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private bool isHittingWall = false;

    float m_mouseSensitivity = 100.0f;
    float m_mouseX, m_rotationMouseX;
    Vector3 m_rotation;

    enum SizeState { small = 0, normal = 1, big = 2};
    float[] m_sizes = { 0.5f, 1.0f, 2.0f };
    int m_sizeState = (int)SizeState.normal;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        m_rotation = Vector3.zero;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Wall")
        {
            isHittingWall = true;
        }
        else
        {
            isHittingWall = false;  
        }
    }

    void Update()
    {
        HandleInput();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        if (isHittingWall)
        {
            switch (m_sizeState)
            {
                case (int)SizeState.big:
                    playerVelocity = new Vector3(playerVelocity.x, slowSlide, playerVelocity.z);
                    break;

                case (int)SizeState.normal:
                    playerVelocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z);
                    break;

                case (int)SizeState.small:
                    playerVelocity = new Vector3(playerVelocity.x, 0.0f, playerVelocity.z);
                    break;

                default:
                    break;
            }
        }
        else
        { 
        playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += gravityValue * Time.deltaTime, playerVelocity.z);
        }
        controller.Move(playerVelocity * Time.deltaTime);
        isHittingWall = false;  
    }

    private void HandleInput()
    {
        // Mouse RB is dragged, calculate player rotation from the mouse position difference between frames
        m_rotationMouseX = -(Input.mousePosition.x - m_mouseX) * Time.deltaTime * m_mouseSensitivity;
        Debug.Log(m_rotationMouseX);
        this.transform.rotation *= Quaternion.Euler(new Vector3(0.0f, m_rotationMouseX, 0.0f));

        // Get the new mouse position for the new frame
        m_mouseX = Input.mousePosition.x;

        Vector3 l_movementDirection = Vector3.zero;

        // movement with AWSD keys
        l_movementDirection = -Input.GetAxis("Vertical") * this.transform.forward;
        l_movementDirection -= Input.GetAxis("Horizontal") * this.transform.right;
        controller.Move(l_movementDirection * Time.deltaTime * playerSpeed);


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

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    public void Grow()
    {
        m_sizeState = (m_sizeState + 1) % 3;
        transform.localScale = new Vector3(m_sizes[m_sizeState], m_sizes[m_sizeState], m_sizes[m_sizeState]);
    }

    public void Shrink()
    {
        m_sizeState--;
        if (m_sizeState < 0)
        {
            m_sizeState = 0;
        }
        transform.localScale = new Vector3(m_sizes[m_sizeState], m_sizes[m_sizeState], m_sizes[m_sizeState]);
    }
}
