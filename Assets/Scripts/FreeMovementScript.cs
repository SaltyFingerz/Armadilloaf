using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FreeMovementScript : MonoBehaviour
{
    public CinemachineFreeLook M_freeLook;
    public float m_playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // todo: movement based on camera direction
        Vector3 l_movementDirection;

        // movement with AWSD keys
        l_movementDirection = Input.GetAxis("Vertical") * M_freeLook.transform.forward;
        l_movementDirection += Input.GetAxis("Horizontal") * M_freeLook.transform.right;

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
