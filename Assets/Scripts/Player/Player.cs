using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that is inherited by CustomController and PlayerLaunchScript
public class PlayerBase : MonoBehaviour
{
    public GameObject M_camera;
    protected float m_mouseSensitivity = 400.0f;
    protected float m_cameraRotationY = 0.0f;
    protected Vector3 m_cameraOffset = new Vector2(14.0f, 8.0f);
    float m_mouseRotationX = 0.0f , m_mouseRotationY = 0.0f;

    public Vector2 GetMouseRotation()
    {
        return new Vector2(m_mouseRotationX, m_mouseRotationY);
    }

    public void SetMouseRotation(Vector2 a_mouseRotation)
    {
        m_mouseRotationX = a_mouseRotation.x;
        m_mouseRotationY = a_mouseRotation.y;
    }
    protected void PlayerAndCameraRotation()
    {
        // Mouse is dragged, calculate player rotation from the mouse position difference between frames
        m_mouseRotationX += Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivity;
        m_mouseRotationY -= Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivity * 0.3f;
        m_mouseRotationY = Mathf.Clamp(m_mouseRotationY, 0.0f, 35.0f);

        // rotate the player (left-right)
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.AngleAxis(m_mouseRotationY, this.transform.right) * Quaternion.AngleAxis(m_mouseRotationX, Vector3.up), Time.fixedDeltaTime * 10.0f);
        //Debug.Log(m_mouseSensitivity);
    }


    public void SetRotation(Vector3 a_direction)
    {
        Vector3 l_axis = Vector3.Cross(a_direction, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_mouseRotationY, l_axis) * a_direction;
        m_cameraRotationY = l_direction.y;

        this.transform.rotation = Quaternion.AngleAxis(m_mouseRotationY, this.transform.right) * Quaternion.AngleAxis(m_mouseRotationX, Vector3.up);
        this.transform.rotation = Quaternion.AngleAxis(m_mouseRotationY, this.transform.right) * Quaternion.AngleAxis(m_mouseRotationX, Vector3.up);
        HandleCameraInput(a_direction);
    }

    protected void HandleCameraInput(Vector3 a_rotation)
    {
        Vector3 l_axis = Vector3.Cross(a_rotation, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_mouseRotationY, l_axis) * a_rotation;
        Vector3 l_directionRotation = Quaternion.AngleAxis(-m_mouseRotationY + 24.0f, l_axis) * a_rotation;

        m_cameraRotationY = Mathf.Lerp(m_cameraRotationY, l_direction.y, Time.fixedDeltaTime * 5.0f);

        l_direction.Normalize();

        Quaternion l_rotationFinal = Quaternion.LookRotation(l_directionRotation);

        //camera transform change
        M_camera.transform.rotation = Quaternion.Lerp(M_camera.transform.rotation, l_rotationFinal, Time.fixedDeltaTime * 10.0f);
        M_camera.transform.position = this.transform.position + new Vector3(-M_camera.transform.forward.x * m_cameraOffset.x, m_cameraOffset.y * (-m_cameraRotationY * 1.6f), -M_camera.transform.forward.z * m_cameraOffset.x);

    }


    public void SetDirection(Vector3 a_direction)
    {
        // calculate new camera Rotation Y
        Vector3 l_axis = Vector3.Cross(a_direction, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_mouseRotationY, l_axis) * a_direction;
        m_cameraRotationY = l_direction.y;

        // set new position and rotations for camera and player
        this.GetComponent<Rigidbody>().isKinematic = true;
        M_camera.transform.rotation = Quaternion.LookRotation(a_direction);
        M_camera.transform.position = this.transform.position + new Vector3(-M_camera.transform.forward.x * m_cameraOffset.x, m_cameraOffset.y * (-m_cameraRotationY), -M_camera.transform.forward.z * m_cameraOffset.x);
        this.transform.rotation = Quaternion.LookRotation(a_direction);
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void SetCameraOffset(Vector2 a_offset)
    {
        m_cameraOffset = a_offset;
    }
}
