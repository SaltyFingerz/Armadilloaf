using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManagerScript;
using UnityEngine.InputSystem.XR;

public class CustomController : MonoBehaviour
{
    public Rigidbody rb;
    public bool isGrounded;
    private PrototypePlayerMovement m_playerMovement;
    public Camera M_walkCamera;
    public GameObject M_playerManager;
    private float m_gravityValue = -9.81f;
    private Vector2 m_moveInput;

    public RenderingScript M_RenderScript;

    public LayerMask M_whatIsGround;
    public Transform M_groundPoint;
    private bool m_justLaunched = false;

    float m_mouseSensitivity = 1000.0f;
    float m_rotationMouseX;

    [SerializeField] private float m_minimumSpeed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = gameObject.GetComponent<PrototypePlayerMovement>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (Time.timeScale < 0.1f)
        {
            return;
        }


        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying || !M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        // Mouse RB is dragged, calculate player rotation from the mouse position difference between frames
        m_rotationMouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivity;
        transform.Rotate(Vector3.up, -m_rotationMouseX);

        // Player won't move when free camera is turned on
        // don't update when player is launching
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying || !M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        MovePlayerRelativeToCamera();

        RaycastHit hit;
        if(Physics.Raycast(M_groundPoint.position, Vector3.down, out hit, 0.5f))
        {
            isGrounded = true;
            if(rb.velocity.magnitude < m_minimumSpeed)
            {
                m_justLaunched = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    public void Update()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying || !M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity += new Vector3(0f, m_playerMovement.m_jumpHeight, 0f);
        }

       // M_RenderScript.DisableBlur();
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
        Vector3 forward = M_walkCamera.transform.forward;
        Vector3 right = M_walkCamera.transform.right;
       
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

        rb.velocity = new Vector3(m_moveInput.x * m_playerMovement.m_playerSpeed, rb.velocity.y + m_gravityValue * Time.deltaTime, m_moveInput.y * m_playerMovement.m_playerSpeed);
    }

    public void PlayerLaunched()
    {
        m_justLaunched = true;
    }

 
}
