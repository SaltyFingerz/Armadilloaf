using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class FreeMovementScript : MonoBehaviour
{
    public float m_playerSpeed;
    public float m_mouseSensitivity;
    Camera m_camera;
    Vector3 m_direction;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = this.transform.GetChild(0).GetComponent<Camera>();
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale < 0.1f)
        {
            return;
        }

        RotateCamera();

       

        // calculate movement directions for forward and side movement
        Vector3 l_forward = this.transform.forward;
        Vector3 l_right = this.transform.right;

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

        this.transform.position += l_movementDirection * m_playerSpeed * Time.deltaTime;
    }

    void RotateCamera()
    {
        // Mouse is moved, calculate camera rotation from the mouse position difference between frames
        float l_mouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivity;
        float l_mouseY = -Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivity;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_rotation;
        l_rotation.x = m_direction.x * Mathf.Cos(l_mouseX) - m_direction.z * Mathf.Sin(l_mouseX);
        l_rotation.y = 0.0f;
        l_rotation.z = m_direction.x * Mathf.Sin(l_mouseX) + m_direction.z * Mathf.Cos(l_mouseX);

        m_direction = l_rotation;

        //rotate the camera and interpolate
        Quaternion l_rotationQuaternion = Quaternion.LookRotation(l_rotation);
        l_rotationQuaternion = Quaternion.Lerp(m_camera.transform.rotation, l_rotationQuaternion, Time.fixedDeltaTime * 10.0f);

        //camera transform change
        m_camera.transform.rotation = l_rotationQuaternion;
    }
}
