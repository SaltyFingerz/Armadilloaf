using Cinemachine;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerLaunchScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private GameObject M_arrow;
    private GameObject M_arrowMaximum;

    public GameObject M_launchCamera;
    public GameObject M_playerManager;
    public UnityEngine.UI.Image M_fillImage;
    public Canvas M_canvas;

    float m_rotationMouseY, m_rotationMouseX;
    public float m_mouseSensitivityX;
    public float m_mouseSensitivityY;
    float m_currentScroll;

    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;
    bool m_isOnFloor = false;

    public int M_maxPower;
    public float M_minimumDirectionY, M_maximumDirectionY;
    public float M_angleChangeRadians;
    public float M_floorAngleChangeRadians;

    [SerializeField] float m_powerSizeStep = 1.0f;          // Determines how big is the scale difference in the arrow when choosing launching power.
    [SerializeField] float m_baseLength = 10.0f;            // Minimum lenght of the arrow.
    [SerializeField] private float m_minimumSpeed = 0.2f;   // Speed minimum limit before the player changes to walking player.

    public Vector2 M_cameraOffset;

    public void Start()
    {
        // get objects
        m_rigidbody = GetComponent<Rigidbody>();
        M_arrowMaximum = this.gameObject.transform.GetChild(0).gameObject;
        M_arrow = this.gameObject.transform.GetChild(1).gameObject;

        // assign starting values
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
        M_fillImage.fillAmount = 0.0f;
        M_arrowMaximum.transform.localScale = new Vector3(5.4f, 5.4f, m_baseLength + m_powerSizeStep * M_maxPower);

    }

    // Handle rigidbody physics
    public void FixedUpdate()
    {
        // if paused or free flying, don't update
        if (Time.timeScale < 0.1f || M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.7f) && m_rigidbody.velocity.magnitude < m_minimumSpeed)
        {
            m_isOnFloor = true;
            if(m_launchingStage != 0)
            {
               // M_playerManager.GetComponent<PlayerManagerScript>().StartWalking();
            }
        }
        else
        {
            m_isOnFloor = false;
        }

        // Manage launching stage
        switch (m_launchingStage)
        {
            case 0:
                DirectionInput();
                break;

            case 1:
                RollingDirectionInput();
                break;

            default:
                break;
        }
    }

    // Handle key inputs
    public void Update()
    {
        // if paused or free flying, don't update
        if (Time.timeScale < 0.1f || M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        // Manage launching stage
        if (m_launchingStage == 0)
        {
            HandleLaunchInput();
        }

    }

    void RollingDirectionInput()
    {
        Vector3 l_direction = m_rigidbody.velocity.normalized;
        Vector3 l_rotatedDirection = l_direction;
        float l_angleChange = M_angleChangeRadians;

        if (m_isOnFloor)
        {
            l_angleChange = M_floorAngleChangeRadians;
        }

        // Change direction based on input
        if (Input.GetKey(KeyCode.A))
        {
            l_rotatedDirection.x = l_direction.x * Mathf.Cos(l_angleChange * Time.fixedDeltaTime) - l_direction.z * Mathf.Sin(l_angleChange * Time.fixedDeltaTime);
            l_rotatedDirection.z = l_direction.x * Mathf.Sin(l_angleChange * Time.fixedDeltaTime) + l_direction.z * Mathf.Cos(l_angleChange * Time.fixedDeltaTime);
            l_direction = l_rotatedDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            l_rotatedDirection.x = l_direction.x * Mathf.Cos(-l_angleChange * Time.fixedDeltaTime) - l_direction.z * Mathf.Sin(-l_angleChange * Time.fixedDeltaTime);
            l_rotatedDirection.z = l_direction.x * Mathf.Sin(-l_angleChange * Time.fixedDeltaTime) + l_direction.z * Mathf.Cos(-l_angleChange * Time.fixedDeltaTime);
            l_direction = l_rotatedDirection;
        }

        // normalize and apply changed direction
        l_direction.Normalize();
        m_rigidbody.velocity = l_direction * m_rigidbody.velocity.magnitude;

        // Calculate camera rotation
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;
        
        //rotate the camera and interpolate
        Quaternion l_rotation = Quaternion.LookRotation(l_desiredRotation);
        l_rotation = Quaternion.Lerp(M_launchCamera.transform.rotation, l_rotation, Time.fixedDeltaTime * 10.0f);

        //camera transform change
        M_launchCamera.transform.position = this.transform.position + new Vector3(-M_launchCamera.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y, -M_launchCamera.transform.forward.z * M_cameraOffset.x);
        M_launchCamera.transform.rotation = l_rotation;

    }

    void HandleLaunchInput()
    {
        //override player rotation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {       
            LaunchingStart();
            return;
        }

        //m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        m_launchingPower += (m_currentScroll + Input.mouseScrollDelta.y);
        M_fillImage.fillAmount = m_launchingPower / M_maxPower;

        // clamp the power of the lauch between 0 and the limit
        if (m_launchingPower < 1)
        {
            m_launchingPower = 1;
        }
        else if (m_launchingPower > M_maxPower)
        {
            m_launchingPower = M_maxPower;
        }
        M_arrow.transform.localScale = new Vector3(5, 5, m_baseLength + m_powerSizeStep * m_launchingPower);
    }

    public void Reset()
    {
        m_rigidbody.freezeRotation = false;
        m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        M_arrow.SetActive(true);
        M_arrowMaximum.SetActive(true);
        M_arrow.transform.localScale = new Vector3(5f, 5f, 5f);
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);

        m_launchingStage = 0;
        m_launchingPower = 0;
        M_canvas.enabled = true;

        m_currentScroll = Input.mouseScrollDelta.y;
    }
    private void LaunchingStart()
    {
        M_canvas.enabled = false;
        m_direction.Normalize();
        m_launchingStage++;
        m_rigidbody.constraints = RigidbodyConstraints.None;
        m_rigidbody.freezeRotation = true;
        M_arrow.SetActive(false);
        M_arrowMaximum.SetActive(false);
        m_launchingPower *= 3.0f;
        m_rigidbody.velocity = new Vector3(m_direction.x * m_launchingPower, m_direction.y * m_launchingPower, m_direction.z * m_launchingPower);
    }

    private void DirectionInput()
    {
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;

        //rotate the player after getting the updated direction, interpolate
        Quaternion l_rotation = Quaternion.LookRotation(l_desiredRotation);
        m_rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, l_rotation, Time.fixedDeltaTime * 10.0f));

        // Calculate and clamp Y value
        //Vector3 l_axis = Vector3.Cross(l_result, Vector3.up);
        //if (l_axis == Vector3.zero) l_axis = Vector3.right;
        //Vector3 l_direction = Quaternion.AngleAxis(-m_rotationMouseY, l_axis) * l_result;
        //l_direction.Normalize();

        //camera transform change
        M_launchCamera.transform.position = this.transform.position + new Vector3(-this.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y, -this.transform.forward.z * M_cameraOffset.x);
        M_launchCamera.transform.rotation = this.transform.rotation;
    }

    Vector3 GetDesiredRotationFromMouseInput()
    {
        // Mouse is moved, calculate camera rotation from the mouse position difference between frames
        float l_mouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivityX;
        float l_mouseY = -Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivityY;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_rotation;
        l_rotation.x = m_direction.x * Mathf.Cos(l_mouseX) - m_direction.z * Mathf.Sin(l_mouseX);
        l_rotation.y = 0.0f;
        l_rotation.z = m_direction.x * Mathf.Sin(l_mouseX) + m_direction.z * Mathf.Cos(l_mouseX);

        return l_rotation;
    }

    public void SetValues(float a_size, float a_mass)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
        m_rigidbody.mass = a_mass;
    }

    private void OnTriggerEnter(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Enemy") || a_hit.gameObject.CompareTag("Hazard"))
        {
            SceneManager.LoadScene("FailScreen");
            PlayerPrefs.SetInt("tute", 1); //this is to not load the tutorial upon reloading the scene (temporary until respawn)
        }
    }
  
    void OnCollisionStay(Collision a_collider)
    {
        if (a_collider.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("FailScreen");
            PlayerPrefs.SetInt("tute", 1);
        }
    }

    public bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.7f);
    }
}
    
