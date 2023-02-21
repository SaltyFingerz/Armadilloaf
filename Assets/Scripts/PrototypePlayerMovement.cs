using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerMovement : MonoBehaviour
{
    public CinemachineVirtualCamera M_walkCamera;
    public PlayerManagerScript M_playerManagerScript;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    public float slowSlide = -0.05f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private bool isHittingWall = false;

    float m_mouseSensitivity = 1000.0f;
    float m_rotationMouseX;

    private void Start()
    {
        M_playerManagerScript = FindObjectOfType<PlayerManagerScript>();
        controller = gameObject.GetComponent<CharacterController>();
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
        if (M_playerManagerScript.M_isFreeFlying)
        {
            return;
        }

        HandleInput();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        if (isHittingWall)
        {
            switch (M_playerManagerScript.M_sizeState)
            {
                case (int)PlayerManagerScript.SizeState.big:
                    playerVelocity = new Vector3(playerVelocity.x, slowSlide, playerVelocity.z);
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
        playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += gravityValue * Time.deltaTime, playerVelocity.z);
        }
        controller.Move(playerVelocity * Time.deltaTime);
        isHittingWall = false;  
    }

    private void HandleInput()
    {
        // Mouse RB is dragged, calculate player rotation from the mouse position difference between frames
        m_rotationMouseX = -Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_mouseSensitivity;
        controller.transform.Rotate(Vector3.up, m_rotationMouseX);

        Vector3 l_movementDirection = Vector3.zero;

        // movement with AWSD keys
        l_movementDirection = -Input.GetAxis("Vertical") * this.transform.forward;
        l_movementDirection -= Input.GetAxis("Horizontal") * this.transform.right;
        controller.Move(l_movementDirection * Time.deltaTime * playerSpeed);
        

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    public void SetSize(float a_size)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
    }
}
