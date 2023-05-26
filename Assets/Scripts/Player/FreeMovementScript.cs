using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class FreeMovementScript : MonoBehaviour
{
    public float m_playerSpeed;
    public float m_mouseSensitivityX;
    public float m_mouseSensitivityY;
    Camera m_camera;
    Vector3 m_direction;
    Rigidbody m_rigidbody;

    public GameObject M_walkingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = this.transform.GetChild(0).GetComponent<Camera>();
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
        m_rigidbody = this.transform.GetChild(0).GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale < 0.1f)
        {
            return;
        }

        RotateCamera();
        MoveCamera();
        RotateWalkingPlayer();

    }

    void RotateWalkingPlayer()
    {
        if(!M_walkingPlayer.activeSelf)
        {
            return;
        }


        // calculte direction in which the player should be looking at
        Vector3 l_direction = M_walkingPlayer.transform.position - m_camera.transform.position;
        l_direction.Normalize();

        Quaternion l_directionQuaternion = Quaternion.LookRotation(l_direction);
        l_directionQuaternion = Quaternion.Lerp(M_walkingPlayer.transform.rotation, l_directionQuaternion, Time.fixedDeltaTime * 10.0f);

        M_walkingPlayer.transform.rotation = l_directionQuaternion;

    }

    void MoveCamera()
    {
        // calculate movement directions for forward and side movement
        Vector3 l_forward = m_camera.transform.forward;
        Vector3 l_right = m_camera.transform.right;

        // movement with AWSD keys
        Vector3 l_movementDirection;
        l_movementDirection = Input.GetAxis("Horizontal") * l_right;
        l_movementDirection += Input.GetAxis("Vertical") * l_forward;

        if (Input.GetKey(KeyCode.Q))
        {
            l_movementDirection += Vector3.up;

        }
        if (Input.GetKey(KeyCode.E))
        {
            l_movementDirection += Vector3.down;

        }

        m_rigidbody.MovePosition(m_rigidbody.position + l_movementDirection * m_playerSpeed * Time.fixedDeltaTime);
    }

    void RotateCamera()
    {
        // Mouse is moved, calculate camera rotation from the mouse position difference between frames
        float l_mouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivityX;
        float l_mouseY = -Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivityY;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_rotation;
        l_rotation.x = m_direction.x * Mathf.Cos(l_mouseX) - m_direction.z * Mathf.Sin(l_mouseX);
        l_rotation.y = m_direction.y;
        l_rotation.z = m_direction.x * Mathf.Sin(l_mouseX) + m_direction.z * Mathf.Cos(l_mouseX);

        // Calculate and clamp Y value
        Vector3 l_axis = Vector3.Cross(l_rotation, Vector3.up);
        if (l_axis == Vector3.zero)
        {
            l_axis = Vector3.right;
        }
        m_direction = Quaternion.AngleAxis(-l_mouseY, l_axis) * l_rotation;

        m_direction.y = Mathf.Clamp(m_direction.y, -0.9f, 0.9f);
        m_direction.Normalize();

        //rotate the camera and interpolate
        Quaternion l_rotationQuaternion = Quaternion.LookRotation(l_rotation);
        l_rotationQuaternion = Quaternion.Lerp(m_camera.transform.rotation, l_rotationQuaternion, Time.fixedDeltaTime * 10.0f);

        //camera transform change
        m_rigidbody.MoveRotation(l_rotationQuaternion);
    }
}
