using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanThruster : MonoBehaviour
{
    Rigidbody m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rb.AddForce(new Vector3(1.0f, 1.0f, 1.0f));
    }
}
