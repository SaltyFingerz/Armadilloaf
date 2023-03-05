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
    
    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = gameObject.GetComponent<PrototypePlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
       MovePlayerRelativeToCamera();

        RaycastHit hit;
        if(Physics.Raycast(M_groundPoint.position, Vector3.down, out hit, 0.3f))
        {
            isGrounded = true;
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
        Vector3 forwardRelativeVerticalInput = m_playerVerticalInput * forward * m_playerMovement.m_playerSpeed *Time.deltaTime;
        Vector3 rightRelativeHorizontalInput = m_playerHorizontalInput * right * m_playerMovement.m_playerSpeed * Time.deltaTime;

        //Create and apply camrea relative movement
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        this.transform.Translate(cameraRelativeMovement, Space.World);

    }

   

 
}
