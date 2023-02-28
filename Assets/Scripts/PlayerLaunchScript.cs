using Cinemachine;
using Newtonsoft.Json.Bson;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerLaunchScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    public GameObject M_arrow;
    public GameObject M_arrowMaximum;

    public CinemachineFreeLook M_holdDownCamera;
    public CinemachineFreeLook M_freeRotationCamera;
    public GameObject M_playerManager;
    enum CameraMode { holdDown, freeRotation };
    CameraMode m_cameraMode;

    float m_rotationMouseY, m_rotationMouseX;
    float m_mouseSensitivity = 100f;
    float m_mouseSpeedY = 20f;
    float m_currentScroll;

    Vector3 m_launchingDirection;
    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;

    const int m_cameraMaxPriority = 8;
    public int M_maxPower;
    public float M_minimumDirectionY;
    const float m_powerSizeStep = 1.0f;
    const float m_baseLength = 10.0f;
    public float M_rollingDirectionChange;
    

    public void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        M_holdDownCamera.Priority = 0;
        M_freeRotationCamera.Priority = m_cameraMaxPriority;
        m_cameraMode = CameraMode.freeRotation;
        M_arrowMaximum.transform.localScale = new Vector3(5.4f, 5.4f, m_baseLength + m_powerSizeStep * M_maxPower);
    }

    // Handle rigidbody physics
    public void FixedUpdate()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }
        // Manage launching stage
        switch (m_launchingStage)
        {
            case 0:
                DirectionInput();
                break;

            case 1:
                break;

            default:
                break;
        }
    }

    // Handle key inputs
    public void Update()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        // switch camera mode
        if (Input.GetKeyUp(KeyCode.V))
        {
            switch (m_cameraMode)
            {
                case CameraMode.holdDown:
                    M_holdDownCamera.Priority = 0;
                    M_freeRotationCamera.Priority = m_cameraMaxPriority;
                    m_cameraMode = CameraMode.freeRotation;
                    break;

                case CameraMode.freeRotation:
                    M_freeRotationCamera.Priority = 0;
                    M_holdDownCamera.Priority = m_cameraMaxPriority;
                    m_cameraMode = CameraMode.holdDown;
                    break;

                default:
                    break;
            }
        }

        // Manage launching stage
        switch (m_launchingStage)
        {
            case 0:
                HandleLaunchInput();
                break;
            case 1:
                RollingDirectionInput();
                break;
            default:
                break;
        }

    }

    void RollingDirectionInput()
    {
        Vector3 l_direction = m_rigidbody.velocity.normalized;
        // change direction
        if (Input.GetKey(KeyCode.A))
        {
            l_direction.x += M_rollingDirectionChange * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            l_direction.x -= M_rollingDirectionChange * Time.deltaTime;
        }
        l_direction.Normalize();
        m_rigidbody.velocity = l_direction * m_rigidbody.velocity.magnitude;

    }

    void HandleLaunchInput()
    {
        //override player rotation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 l_axis = Vector3.Cross(m_direction, Vector3.up);
            if (l_axis == Vector3.zero) l_axis = Vector3.right;
            m_launchingDirection = m_direction;
            //m_direction = Quaternion.AngleAxis(-m_rotationMouseY * 5.0f, l_axis) * m_direction;
            m_launchingDirection.Normalize();
            m_launchingStage++;
            LaunchingStart();
        }
        m_launchingPower += (m_currentScroll - Input.mouseScrollDelta.y);

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
        m_rigidbody.useGravity = false;

        m_launchingStage = 0;
        m_launchingPower = 0;
        M_holdDownCamera.Priority = 0;
        M_freeRotationCamera.Priority = m_cameraMaxPriority;
        m_cameraMode = CameraMode.freeRotation;

        m_currentScroll = Input.mouseScrollDelta.y;
    }
    private void LaunchingStart()
    {
        m_rigidbody.freezeRotation = true;
        M_arrow.SetActive(false);
        M_arrowMaximum.SetActive(false);
        m_rigidbody.useGravity = true;
        m_launchingPower *= 3.0f;
        m_rigidbody.velocity = new Vector3(m_launchingDirection.x * m_launchingPower, m_launchingDirection.y * m_launchingPower, m_launchingDirection.z * m_launchingPower);
    }
    
    private void DirectionInput()
    {
        if(Time.timeScale < 0.1f)
        {
            return;
        }

        // Mosue input is disabled when holding RMB.
        // When the camera rotates without RMB press, the direction is calculated from position of the player and the camera.
        // Otherwise, calculate the direction from mouse input.
        // Direction will be used in launching.
        
        if (Input.GetMouseButton(1))
        {
            return;
        }

        // Mouse RB is dragged, calculate camera rotation from the mouse position difference between frames
        m_rotationMouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_mouseSensitivity;
        m_rotationMouseY = -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * m_mouseSensitivity * m_mouseSpeedY;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_result;
        l_result.x = m_direction.x * Mathf.Cos(m_rotationMouseX) - m_direction.z * Mathf.Sin(m_rotationMouseX);
        l_result.y = m_direction.y;
        l_result.z = m_direction.x * Mathf.Sin(m_rotationMouseX) + m_direction.z * Mathf.Cos(m_rotationMouseX);

        // Calculate the arrow look at position to use in hold camera,
        // or Y value for the free rotation camera
        Vector3 l_axis = Vector3.Cross(l_result, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_rotationMouseY, l_axis) * l_result;

        if (m_cameraMode == CameraMode.freeRotation)
        {
            Vector3 l_distance = this.transform.position - M_freeRotationCamera.transform.position;
            l_distance.Normalize();
            l_direction = new Vector3(l_distance.x, l_direction.y, l_distance.z);
        }

        if (l_direction.y < M_minimumDirectionY)
        {
            l_direction.y = M_minimumDirectionY;
        }
        // clamp Y value so direction change is easier
        //rotate the player after getting the updated direction
        Quaternion l_rotation = Quaternion.LookRotation(l_direction * Time.deltaTime);
        m_rigidbody.MoveRotation(l_rotation);

        // final direction
        m_direction = l_direction;
    }
    public void SetSize(float a_size)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
    }
}
    
