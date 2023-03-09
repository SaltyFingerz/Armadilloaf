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
    public GameObject M_arrow;
    public GameObject M_arrowMaximum;

    public Camera M_freeRotationCamera;
    public GameObject M_playerManager;
    public UnityEngine.UI.Image M_fillImage;
    public Canvas M_canvas;

    float m_rotationMouseY, m_rotationMouseX;
    [SerializeField] float m_mouseSensitivityX = 100f;
    [SerializeField] float m_mouseSensitivityY = 200f;
    float m_currentScroll;

    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;
    bool m_isOnFloor = false;

    public int M_maxPower;
    public float M_minimumDirectionY, M_maximumDirectionY;
    public float M_angleChangeRadians;
    public float M_floorAngleChangeRadians;

    [SerializeField] float m_powerSizeStep = 1.0f;
    [SerializeField] float m_baseLength = 10.0f;
    [SerializeField] private float m_minimumSpeed = 0.2f;

    public void Start()
    {
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
        M_fillImage.fillAmount = 0.0f;
        m_rigidbody = GetComponent<Rigidbody>();
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
                M_playerManager.GetComponent<PlayerManagerScript>().StartWalking();
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
        float l_angle = M_angleChangeRadians;

        if (m_isOnFloor)
        {
            l_angle = M_floorAngleChangeRadians;
        }
        // change direction
        if (Input.GetKey(KeyCode.A))
        {
            l_rotatedDirection.x = l_direction.x * Mathf.Cos(l_angle * Time.deltaTime) - l_direction.z * Mathf.Sin(l_angle * Time.deltaTime);
            l_rotatedDirection.z = l_direction.x * Mathf.Sin(l_angle * Time.deltaTime) + l_direction.z * Mathf.Cos(l_angle * Time.deltaTime);
            l_direction = l_rotatedDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            l_rotatedDirection.x = l_direction.x * Mathf.Cos(-l_angle * Time.deltaTime) - l_direction.z * Mathf.Sin(-l_angle * Time.deltaTime);
            l_rotatedDirection.z = l_direction.x * Mathf.Sin(-l_angle * Time.deltaTime) + l_direction.z * Mathf.Cos(-l_angle * Time.deltaTime);
            l_direction = l_rotatedDirection;
        }
        l_direction.Normalize();
        m_rigidbody.velocity = l_direction * m_rigidbody.velocity.magnitude;

    }

    void HandleLaunchInput()
    {
        /*if (!m_isOnFloor)
        {
            return;
        }*/
        //override player rotation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {       
            LaunchingStart();
            return;
        }

        //m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        m_launchingPower += (m_currentScroll - Input.mouseScrollDelta.y);
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
        // Mouse RB is dragged, calculate camera rotation from the mouse position difference between frames
        m_rotationMouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_mouseSensitivityX;
        m_rotationMouseY = -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * m_mouseSensitivityY;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_result;
        l_result.x = m_direction.x * Mathf.Cos(m_rotationMouseX) - m_direction.z * Mathf.Sin(m_rotationMouseX);
        l_result.y = m_direction.y;
        l_result.z = m_direction.x * Mathf.Sin(m_rotationMouseX) + m_direction.z * Mathf.Cos(m_rotationMouseX);

        // Calculate and clamp Y value
        Vector3 l_axis = Vector3.Cross(l_result, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_rotationMouseY, l_axis) * l_result;

        if (l_direction.y < M_minimumDirectionY)
        {
            l_direction.y = M_minimumDirectionY;
        }
        else if (l_direction.y > M_maximumDirectionY)
        {
            l_direction.y = M_maximumDirectionY;
        }

        //rotate the player after getting the updated direction
        Quaternion l_rotation = Quaternion.LookRotation(l_direction * Time.deltaTime);
        m_rigidbody.MoveRotation(l_rotation);

        // final direction
        m_direction = l_direction;
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
    
