using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PrototypePlayerMovement : MonoBehaviour
{
    public Camera M_walkCamera;
    public GameObject M_playerManager;
    public CustomController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] public float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = 1;
    [SerializeField] public float m_jumpHeight = 4.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 2.0f;

    private void Start()
    {
        m_controller = gameObject.GetComponent<CustomController>();
    }

    private void OnTriggerEnter(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Enemy") || a_hit.gameObject.CompareTag("Hazard"))
        {
            SceneManager.LoadScene("FailScreen");
            PlayerPrefs.SetInt("tute", 1); //this is to not load the tutorial upon reloading the scene (temporary until respawn)

        }

        if(a_hit.gameObject.CompareTag("Collectible"))
        {
            print("collectible +1");
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();
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
       // m_controller.Move(playerVelocity * Time.deltaTime);
        m_isHittingWall = false;  
    }

    private void HandleInput()
    {
       

        Vector3 l_movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_playerSpeed = 8.0f;
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
