using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CustomController : MonoBehaviour
{
    public Rigidbody rb;
    public bool isGrounded;
    private PrototypePlayerMovement m_playerMovement;
    public CinemachineVirtualCamera M_walkCamera;

    private Vector2 m_moveInput;

    public LayerMask M_whatIsGround;
    public Transform M_groundPoint;
    private bool m_justLaunched = false;
    
    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = gameObject.GetComponent<PrototypePlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerRelativeToCamera();
       // MovePlayerIndependentFromCamera();
        RaycastHit hit;
        if(Physics.Raycast(M_groundPoint.position, Vector3.down, out hit, 0.3f))
        {
            isGrounded = true;
            m_justLaunched = false;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity += new Vector3(0f, m_playerMovement.m_jumpHeight, 0f);
        }
    }

    void MovePlayerRelativeToCamera()
    {
        if(m_justLaunched)
        {
            return;
        }
        //get player input
        float m_playerVerticalInput = Input.GetAxis("Vertical");
        float m_playerHorizontalInput = Input.GetAxis("Horizontal");

        //get camera normalized directional vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
       
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        //create direction-relative-input vectors
        Vector3 forwardRelativeVerticalInput = m_playerVerticalInput * forward * m_playerMovement.m_playerSpeed * Time.deltaTime * 200;
        Vector3 rightRelativeHorizontalInput = m_playerHorizontalInput * right * m_playerMovement.m_playerSpeed * Time.deltaTime * 200;


        //Create and apply camera relative movement
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        rb.velocity = new Vector3(cameraRelativeMovement.x, rb.velocity.y, cameraRelativeMovement.z);
    }

    void MovePlayerIndependentFromCamera()
    {
        m_moveInput.x = Input.GetAxis("Horizontal");
        m_moveInput.y = Input.GetAxis("Vertical");
        m_moveInput.Normalize();

        rb.velocity = new Vector3(m_moveInput.x * m_playerMovement.m_playerSpeed, rb.velocity.y, m_moveInput.y * m_playerMovement.m_playerSpeed);
    }

    public void PlayerLaunched()
    {
        m_justLaunched = true;
    }

 
}
