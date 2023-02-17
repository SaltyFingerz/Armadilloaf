using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovementScript : MonoBehaviour
{
    Vector3 m_direction = Vector3.zero;
    float m_velocity = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // todo: movement based on camera direction
        m_direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            m_direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_direction += Vector3.back;

        }
        if (Input.GetKey(KeyCode.D))
        {
            m_direction += Vector3.right;

        }
        if (Input.GetKey(KeyCode.A))
        {
            m_direction += Vector3.left;

        }
        if (Input.GetKey(KeyCode.Q))
        {
            m_direction += Vector3.up;

        }
        if (Input.GetKey(KeyCode.E))
        {
            m_direction += Vector3.down;

        }

        this.transform.position += m_direction * Time.deltaTime * m_velocity;
    }
}
