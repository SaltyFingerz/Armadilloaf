using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrailScript : MonoBehaviour
{
    public Transform M_BallPosition;
    private ParticleSystem m_launchTrail;
    public PlayerManagerScript M_PManager;
    public GameObject M_BallPlayer;
    private ParticleSystem.MainModule m_pMain;
    private ParticleSystem.EmissionModule m_pEmission;
    // Start is called before the first frame update
    void Start()
    {
        m_launchTrail = gameObject.GetComponent<ParticleSystem>();
        m_pMain = m_launchTrail.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!M_BallPlayer.activeSelf)
        {
            DeactivateTrail();
        }
        gameObject.transform.position = M_BallPosition.position;



        //adjust particle size to ball size

        //adjust 
        if (M_PManager.M_sizeState == 1)
        {
            m_pMain.startSize = 1;
        }

        else if (M_PManager.M_sizeState == 2)
        {
            m_pMain.startSize = 3f;
        }

        else if (M_PManager.M_sizeState == 0)
        {
            m_pMain.startSize = 0.5f;
        }
    }

    public void ActivateTrail()
    {
        m_launchTrail.Play();

    }

    public void DeactivateTrail()
    {
        m_launchTrail.Stop();
    }
}
