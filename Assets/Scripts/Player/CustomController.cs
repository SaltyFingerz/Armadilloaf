using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManagerScript;
using UnityEngine.InputSystem.XR;

public class CustomController : PlayerBase
{
    public Rigidbody rb;
    public bool isGrounded;
    private PrototypePlayerMovement m_playerMovement;
    public Camera M_walkCamera;
    public GameObject M_playerManager;
    private float m_gravityValue = -9.81f;
    private Vector2 m_moveInput;
    public PlayerManagerScript m_playerManager;
    public RenderingScript M_RenderScript;

    public LayerMask M_whatIsGround;
    public Transform M_groundPoint;
    private bool m_justLaunched = false;

    private bool m_walking = false;
    [SerializeField] private AudioSource m_walkSound;

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

        PlayerAndCameraRotation();

        // Player won't move when free camera is turned on
        // don't update when player is launching
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying || !M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        MovePlayerRelativeToCamera();

        RaycastHit hit;
        if (m_playerManager.M_sizeState < 2)
        {
            if (GroundDetectionScript.M_IsGrounded)
            {
                isGrounded = true;
                if (rb.velocity.magnitude < m_minimumSpeed)
                {
                    m_justLaunched = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        else if (m_playerManager.M_sizeState == 2)
        {
            if (Physics.Raycast(M_groundPoint.position, Vector3.down, out hit, 2f))
            {
                isGrounded = true;
                if (rb.velocity.magnitude < m_minimumSpeed)
                {
                    m_justLaunched = false;
                }
            }
            else
            {
                isGrounded = false;
            }
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

        M_RenderScript.DisableBlur();

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && isGrounded)
        {
             m_walking = true;
            
        }
        else 
        {
            m_walking = false;
            m_walkSound.Stop();
        }

        if(m_walking && !m_walkSound.isPlaying)
        {
            m_walkSound.Play();
        }
    }

    void MovePlayerRelativeToCamera()
    {
        if(m_justLaunched)
        {
            return;
        }
        //get player input
        float l_playerVerticalInput = Input.GetAxis("Vertical");
        float l_playerHorizontalInput = Input.GetAxis("Horizontal");

        //get camera normalized directional vectors
        Vector3 forward = M_walkCamera.transform.forward;
        Vector3 right = M_walkCamera.transform.right;
       
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        //create direction-relative-input vectors
        Vector3 forwardRelativeVerticalInput = l_playerVerticalInput * forward * m_playerMovement.m_playerSpeed * Time.deltaTime * 200;
        Vector3 rightRelativeHorizontalInput = l_playerHorizontalInput * right * m_playerMovement.m_playerSpeed * Time.deltaTime * 200;


        //Create and apply camera relative movement
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        rb.velocity = new Vector3(cameraRelativeMovement.x, rb.velocity.y, cameraRelativeMovement.z);

        HandleCameraInput(this.transform.forward);
    }
  
    public void PlayerLaunched()
    {
        m_justLaunched = true;
    }
}
