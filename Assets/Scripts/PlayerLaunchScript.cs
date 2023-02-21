using Cinemachine;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerLaunchScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    public GameObject M_arrow;

    public CinemachineFreeLook M_holdDownCamera;
    public CinemachineFreeLook M_freeRotationCamera;
    enum CameraMode { holdDown, freeRotation };
    CameraMode m_cameraMode;

    float m_mouseX, m_mouseY;
    float m_rotationMouseY, m_rotationMouseX;
    float m_mouseSensitivity = 1.5f;
    float m_mouseSpeedY = 20.0f;
    float m_currentScroll;

    Vector3 m_launchingDirection;
    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;

    const int m_cameraMaxPriority = 8;
    public int M_maxPower;
    public float M_minimumDirectionY;
    

    public void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        M_holdDownCamera.Priority = 0;
        M_freeRotationCamera.Priority = m_cameraMaxPriority;
        m_cameraMode = CameraMode.freeRotation;
    }

    public void Update()
    {
        // shrink
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Shrink();
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
                HandleDirectionInput();
                break;
            case 1:
                HandlePowerInput();
                break;
            case 2:
                LaunchingStart();
                break;
        }

    }

    public void FixedUpdate()
    {
        // Manage launching stage
        switch (m_launchingStage)
        {
            case 0:
                DirectionInput();
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
        public void Reset()
    {
        M_arrow.SetActive(true);
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        
        m_launchingStage = 0;
        m_launchingPower = 0;
        M_holdDownCamera.Priority = 0;
        M_freeRotationCamera.Priority = m_cameraMaxPriority;
        m_cameraMode = CameraMode.freeRotation;
    }
    private void LaunchingStart()
    {
        M_arrow.SetActive(false);
        m_launchingDirection.y = -m_launchingDirection.y;
        m_rigidbody.AddForce(m_launchingDirection * m_launchingPower * 100.0f);
        m_launchingStage++;

    }
    private void HandlePowerInput()
    {
        //override player rotation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 l_axis = Vector3.Cross(m_direction, Vector3.up);
            if (l_axis == Vector3.zero) l_axis = Vector3.right;
            m_direction = Quaternion.AngleAxis(-m_rotationMouseY * 5.0f, l_axis) * m_direction;
            m_launchingDirection.Normalize();
            m_launchingStage++;
        }
        m_launchingPower += (m_currentScroll - Input.mouseScrollDelta.y);

        // clamp the power of the lauch between 0 aand 100
        if(m_launchingPower < 0)
        {
            m_launchingPower = 0;
        }
        else if(m_launchingPower > M_maxPower)
        {
            m_launchingPower = M_maxPower;
        }
        Debug.Log(m_launchingPower);
    }

    private void HandleDirectionInput()
    {
        // stop when space or LMB is pressed
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {
            m_launchingDirection = m_direction;
            m_launchingStage++;
            m_currentScroll = Input.mouseScrollDelta.y;
        }
        if (Input.GetMouseButtonUp(1))
        {
            // Get the new mouse position to add smooth direction change when RMB is released
            m_mouseX = Input.mousePosition.x;
            m_mouseY = Input.mousePosition.y;
        }
    }
    
    private void DirectionInput()
    {
        // Mosue input is disabled when holding RMB.
        // When the camera rotates without RMB press, the direction is calculated from position of the player and the camera.
        // Otherwise, calculate the direction from mouse input.
        // Direction will be used in launching.
        
        if (Input.GetMouseButton(1))
        {
            m_mouseX = Input.mousePosition.x;
            m_mouseY = Input.mousePosition.y;
            return;
        }

        // Mouse RB is dragged, calculate camera rotation from the mouse position difference between frames
        m_rotationMouseX = -(Input.mousePosition.x - m_mouseX) * Time.deltaTime * m_mouseSensitivity;
        m_rotationMouseY = -(Input.mousePosition.y - m_mouseY) * Time.deltaTime * m_mouseSensitivity * m_mouseSpeedY;

        // Get the new mouse position for the new frame
        m_mouseX = Input.mousePosition.x;
        m_mouseY = Input.mousePosition.y;

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

        // final direction
       
        m_direction = l_direction;
        if (m_cameraMode == CameraMode.freeRotation)
        {
            Vector3 l_distance = this.transform.position - M_freeRotationCamera.transform.position;
            l_distance.Normalize();
            m_direction = new Vector3(l_distance.x, l_direction.y, l_distance.z);
        }

        if (m_direction.y < M_minimumDirectionY)
        {
            m_direction.y = M_minimumDirectionY;
        }
        // clamp Y value so direction change is easier
        //rotate the player after getting the updated direction
        Quaternion l_rotation = Quaternion.LookRotation(m_rigidbody.position + m_direction);
        m_rigidbody.MoveRotation(l_rotation);
    }
    public void SetSize(float a_size)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
    }
}
    
