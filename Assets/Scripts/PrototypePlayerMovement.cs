using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody myrigidBody;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    public float slowSlide = -0.05f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private bool isHittingWall = false;

    enum SizeState { small = 0, normal = 1, big = 2};
    float[] m_sizes = { 0.5f, 1.0f, 2.0f };
    int m_sizeState = (int)SizeState.normal;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        myrigidBody = gameObject.GetComponent<Rigidbody>(); 
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
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // grow
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_sizeState = (m_sizeState + 1) % 3;
            transform.localScale = new Vector3(m_sizes[m_sizeState], m_sizes[m_sizeState], m_sizes[m_sizeState]);
        }
        // shrink
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_sizeState--;
            if(m_sizeState < 0)
            {
                m_sizeState = 0;
            }
            transform.localScale = new Vector3(m_sizes[m_sizeState], m_sizes[m_sizeState], m_sizes[m_sizeState]);
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
}
