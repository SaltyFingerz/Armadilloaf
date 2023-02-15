using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class HoldFreeCameraScript : MonoBehaviour
{
    CinemachineFreeLook m_camera;
    public void Start()
    {
        m_camera = GetComponent<CinemachineFreeLook>();
        LockRotation();
    }

    public void Update()
    {
        // camera rotates only when RMB is pressed.
        if (Input.GetMouseButton(1))
        {
            UnlockRotation();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            LockRotation();
        }
    }

    private void LockRotation()
    {
        m_camera.m_XAxis.m_MaxSpeed = 0;
        m_camera.m_YAxis.m_MaxSpeed = 0;
    }

    private void UnlockRotation()
    {
        m_camera.m_XAxis.m_MaxSpeed = 200;
        m_camera.m_YAxis.m_MaxSpeed = 2;
    }
}
