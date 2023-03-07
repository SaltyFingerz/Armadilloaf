using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class FreeMovementScript : MonoBehaviour
{
    public CinemachineVirtualCamera M_freeCamera;
    public float m_playerSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // calculate movement directions for forward and side movement
        Vector3 l_forward = this.transform.position - M_freeCamera.transform.position;
        l_forward.y = 0;
        l_forward.Normalize();
        Vector3 l_right = Quaternion.AngleAxis(90, Vector3.up) * l_forward;

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
}
