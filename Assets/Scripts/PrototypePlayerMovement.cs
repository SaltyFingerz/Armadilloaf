using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PrototypePlayerMovement : MonoBehaviour
{
    public GameObject M_playerManager;
    public GameObject M_Tutorial;
    public GameObject M_TuteWorld;
    public CustomController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] public float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = 1;
    [SerializeField] public float m_jumpHeight = 8.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 2.0f;

    private void Start()
    {
        m_controller = gameObject.GetComponent<CustomController>();
    }

    private void OnTriggerEnter(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("FailScreen");
            PlayerPrefs.SetInt("tute", 1); //this is to not load the tutorial upon reloading the scene (temporary until respawn)

        }
        {
            if ( a_hit.gameObject.CompareTag("Hazard"))
            {
                PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
                m_playerManagerScript.M_takingDamage = true;
            }
        }

        if(a_hit.gameObject.CompareTag("Collectible"))
        {
            print("collectible +1");
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();

        }

        if(a_hit.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(true);
            
        }

       
        if (a_hit.gameObject.name.Contains("TipZone"))
        {
            M_Tutorial.transform.GetChild(0).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(1).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(2).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(3).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(4).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(5).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(6).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(7).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(8).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(9).gameObject.SetActive(true);

        }

        if (a_hit.gameObject.name.Contains("ShrinkZone"))
        {
            M_TuteWorld.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (a_hit.gameObject.name.Contains("FreeCamZone") && !M_Tutorial.transform.GetChild(5).gameObject.activeSelf && !M_Tutorial.transform.GetChild(4).gameObject.activeSelf)
        {
            M_Tutorial.transform.GetChild(6).gameObject.SetActive(true);

        }



    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
        m_playerManagerScript.M_takingDamage = false;


        if (other.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(false);

        }
    }
    private void OnControllerColliderHit(ControllerColliderHit a_hit)
    {
        if (a_hit.gameObject.CompareTag("Wall"))
        {
            m_isHittingWall = true;
        }
        else if (a_hit.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("FailScreen");
            PlayerPrefs.SetInt("tute", 1);
        }
        else
        {
            m_isHittingWall = false;  
        }

        Rigidbody m_rb = a_hit.collider.attachedRigidbody;
        if (m_rb != null && !m_rb.isKinematic)
        {
            m_rb.velocity = a_hit.moveDirection * m_pushForce;
        }
    }

    void Update()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying ||!M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        if (M_Tutorial.transform.GetChild(4).gameObject.activeSelf)
        {
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(false);
        }


        m_groundedPlayer = m_controller.isGrounded;
        HandleInput();


        if (m_groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }



        if (m_isHittingWall)
        {
            switch (M_playerManager.GetComponent<PlayerManagerScript>().M_abilityState)
            {
                case PlayerManagerScript.AbilityState.honey:
                    playerVelocity = new Vector3(playerVelocity.x, m_slowSlide, playerVelocity.z);
                    break;

                case PlayerManagerScript.AbilityState.normal:
                    playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += m_gravityValue * Time.deltaTime, playerVelocity.z);
                    break;

                default:
                    break;
            }
        }
        else
        { 
        playerVelocity = new Vector3(playerVelocity.x, playerVelocity.y += m_gravityValue * Time.deltaTime, playerVelocity.z);
        }
        m_isHittingWall = false;  
    }

    private void HandleInput()
    {
       

        Vector3 l_movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_playerSpeed = 4.0f;
        }
        else
        {
            m_playerSpeed = 2.0f;
        }

        if (Input.GetKey(KeyCode.G))
        {
            PlayerManagerScript m_playerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            m_playerScript.Respawn();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            PlayerManagerScript m_playerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            StartCoroutine(m_playerScript.ShowUIQuickly());
        }

        // movement with AWSD keys
        l_movementDirection = -Input.GetAxis("Vertical") * this.transform.forward;
        l_movementDirection -= Input.GetAxis("Horizontal") * this.transform.right;
    
    }

    public void SetValues(float a_size, float a_mass)
    {
        //m_controller.rb.mass = a_mass;
        transform.localScale = new Vector3(a_size, a_size, a_size);
        switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
        {
            case (int)PlayerManagerScript.SizeState.big:
                m_pushForce = 4.0f;
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                break;

            default:
                Debug.Log("Error! Did you forget to set a size state?");
                break;
        }
    }
}
