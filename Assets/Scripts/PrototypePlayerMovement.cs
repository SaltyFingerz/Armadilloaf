using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PrototypePlayerMovement : MonoBehaviour
{
    public GameObject M_playerManager;
    public GameObject M_Tutorial;
    public GameObject M_TuteWorld;
    public DecalProjector M_BlobShadowDecal;
    public CustomController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] public float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = 1;
    [SerializeField] public float m_jumpHeight = 8.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 2.0f;
    public bool M_InLaunchZone = false;
    public Vector3 M_TargetBlobSize;
    private bool m_gradualSize = true;
    public GameObject M_FinishUI;
    [SerializeField] AudioClip[] m_painClip;
    public AudioSource M_PainAudio;
    public GameObject M_FreshBiscuit;
    private float alphaYellow;
    private UnityEngine.UI.Image m_Yellow;

    [SerializeField] AudioClip[] m_drownClip;
    public AudioSource M_DrawnAudio;

    [SerializeField] AudioClip[] m_waterDrops;
    public AudioSource M_WaterDrop;

    private bool m_canPain = true;
    private bool m_canDrown = true;
    public GameObject M_Water;
    Renderer m_renderer;
    private void Start()
    {
        m_controller = gameObject.GetComponent<CustomController>();
        m_renderer = gameObject.GetComponent<Renderer>();

        m_Yellow = M_Water.GetComponent<UnityEngine.UI.Image>();
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
    }

    private void OnTriggerStay(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Hazard") || a_hit.gameObject.CompareTag("Enemy"))
        {

            PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            if ((m_playerManagerScript.M_sizeState != 2 && a_hit.gameObject.CompareTag("Enemy")) || !a_hit.gameObject.CompareTag("Enemy"))
            {
                m_playerManagerScript.M_takingDamage = true;
                if (m_canPain)
                {
                    PainSound();
                   
                }
            }

            if(a_hit.gameObject.name.Contains("Water") && m_canDrown)
            {
                DrownSound();
            }

            if(a_hit.gameObject.CompareTag("Enemy") && m_playerManagerScript.M_sizeState != 2)
            {
                Fluffing();
            }
        }

        if (a_hit.gameObject.CompareTag("Hazard") && a_hit.gameObject.name.Contains("Water"))
        {

        
            StartCoroutine(YellowScreen()); //activate yellow overlay when underwater

        }
        
    }

    
    IEnumerator YellowScreen()
    {
        M_Water.SetActive(true);
        while (alphaYellow < 0.66f)
        {

            m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, alphaYellow);
            alphaYellow += 0.01f * Time.deltaTime;
            yield return null;

        }
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0.66f);
    }


    private void Fluffing()
    {
        PlayerManagerScript.M_Fluffed = true;
        m_playerSpeed = 1f;
        m_renderer.material.color = Color.cyan;
        M_FreshBiscuit.GetComponent<Image>().color = Color.cyan;
        StartCoroutine(resetFluff());
    }

    public void Defluff()
    {
        m_playerSpeed = 2;
        m_renderer.material.color = Color.white;
        M_FreshBiscuit.GetComponent<Image>().color = Color.white;
        PlayerManagerScript.M_Fluffed = false;
    }
    IEnumerator resetFluff()
    {
        
        yield return new WaitForSeconds(10);
        Defluff();
        yield return null;
    }

    IEnumerator waitForPain()
    {
        yield return new WaitForSeconds(1);
        m_canPain = true;

    }

    IEnumerator waitForDrown()
    {
        yield return new WaitForSeconds(3);
        m_canDrown = true;
    }

    public void PainSound() //random sound of the player character taunting her opponents by exclaiming "Pathetic!" called upon clearing the enemies of a room.
    {
        AudioClip clip = m_painClip[UnityEngine.Random.Range(0, m_painClip.Length)];
        M_PainAudio.PlayOneShot(clip);
        m_canPain = false;
        StartCoroutine(waitForPain());
    }

    public void DrownSound()
    {
        AudioClip clip = m_drownClip[UnityEngine.Random.Range(0, m_drownClip.Length)];
        M_DrawnAudio.PlayOneShot(clip);
        m_canDrown = false;
        StartCoroutine(waitForDrown());
    }

    private void OnTriggerEnter(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Collectible"))
        {
            a_hit.gameObject.GetComponent<HoverScript>().PlayPickupSound();
            //if (a_hit.gameObject.name == "Collectible Banana")
            //{
            //}
            print("collectible +1");
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();

        }

        if(a_hit.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_TuteWorld.transform.GetChild(4).gameObject.SetActive(false);
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(true);
            M_InLaunchZone = true;
            
        }

       //the following "Zone""triggers regulate the tutorial prompts
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
            M_TuteWorld.transform.GetChild(4).gameObject.SetActive(false);
            M_TuteWorld.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (a_hit.gameObject.name.Contains("CloseBananaZone"))
        {
            M_TuteWorld.transform.GetChild(3).gameObject.SetActive(false);

        }

        if (a_hit.gameObject.name.Contains("FreeCamZone") && !M_Tutorial.transform.GetChild(5).gameObject.activeSelf && !M_Tutorial.transform.GetChild(4).gameObject.activeSelf)
        {
            M_Tutorial.transform.GetChild(6).gameObject.SetActive(true);

        }

        if (a_hit.gameObject.name.Contains("JumpPromptZone"))
        {
            M_Tutorial.transform.GetChild(0).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(1).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(2).gameObject.SetActive(true);
            M_Tutorial.transform.GetChild(3).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(4).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(5).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(6).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(7).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(8).gameObject.SetActive(false);
            M_Tutorial.transform.GetChild(9).gameObject.SetActive(false);

        }

        if (a_hit.gameObject.name.Contains("Water"))
        {
            AudioClip clip = m_waterDrops[UnityEngine.Random.Range(0, m_waterDrops.Length)];
            M_WaterDrop.PlayOneShot(clip);
        }

        if (a_hit.gameObject.name.Contains("Finish"))
        {Time.timeScale = 0f;
            M_FinishUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
        m_playerManagerScript.M_takingDamage = false;


        if (other.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(false);
           
            M_InLaunchZone = false;

        }



      
          
          
       

     

    }

    public void TurnOffYellow()
    {
        StopCoroutine(YellowScreen());
        // M_Water.SetActive(false);
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
    }

    private void OnControllerColliderHit(ControllerColliderHit a_hit)
    {
        if (a_hit.gameObject.CompareTag("Wall"))
        {
            m_isHittingWall = true;
        }
        else if (a_hit.gameObject.CompareTag("Enemy"))
        {
            PlayerPrefs.SetInt("tute", 1);
            SceneManager.LoadScene("FailScreen");
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

    public void StartLaunching()
    {
        M_playerManager.GetComponent<PlayerManagerScript>().StartLaunching();
    }

    void Update()
    {

        switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
        {
            case (int)PlayerManagerScript.SizeState.big:
                m_pushForce = 4.0f;
                M_TargetBlobSize = new Vector3(10, 10, 50);
                m_jumpHeight = 15;
                print("jumpheightshouldbesettohigh" + m_jumpHeight);
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(3, 3, 50);
                m_jumpHeight = 8;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(1.5f, 1.5f, 50);
                m_jumpHeight = 8;
                break;

            default:
                Debug.Log("Error! Did you forget to set a size state?");
                break;
        }


        if (PlayerManagerScript.M_Fluffed)
        {
            Fluffing();
        }
        
            //change size gradually each frame
            Vector3 growthIncrement = new Vector3(1f, 1f, 1f);
            Vector3 shadowGrowthIncrement = new Vector3(.06f, .06f, 0f);
            if (PlayerManagerScript.M_Growing)
            {
                if (transform.localScale.x < PlayerManagerScript.M_TargetSize)
                {
                    transform.localScale += growthIncrement * Time.deltaTime;
                }

                else
            {
                PlayerManagerScript.M_Growing = false;
            }
                if (M_BlobShadowDecal.size.x < M_TargetBlobSize.x)
                {
                    M_BlobShadowDecal.size += shadowGrowthIncrement;
                }

            }
            else if (PlayerManagerScript.M_Shrinking)
            {
                if (transform.localScale.x > PlayerManagerScript.M_TargetSize)
                {
                    transform.localScale -= growthIncrement * Time.deltaTime;
                
                }

                if (M_BlobShadowDecal.size.x > M_TargetBlobSize.x)
                {
                    M_BlobShadowDecal.size -= shadowGrowthIncrement;
              
                }

            }
            else if (transform.localScale.x > PlayerManagerScript.M_TargetSize && PlayerManagerScript.M_Growing)
            {
                transform.localScale = new Vector3(PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize);
                M_BlobShadowDecal.size = M_TargetBlobSize;
                PlayerManagerScript.M_Growing = false;
            }

       
            else if (transform.localScale.x < PlayerManagerScript.M_TargetSize && PlayerManagerScript.M_Shrinking)
            {
                transform.localScale = new Vector3(PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize);
                M_BlobShadowDecal.size = M_TargetBlobSize;
                PlayerManagerScript.M_Shrinking = false;
            }

            else if (!PlayerManagerScript.M_Growing && !PlayerManagerScript.M_Shrinking)
        {
            transform.localScale = new Vector3(PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize);
            M_BlobShadowDecal.size = M_TargetBlobSize;
        }
     

      

        M_Tutorial.transform.GetChild(4).gameObject.SetActive(false);
        M_TuteWorld.transform.GetChild(4).gameObject.SetActive(false);


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

        if (Input.GetKey(KeyCode.LeftShift)) //sprint currently deactivated
        {
             m_playerSpeed = 2.5f; 
        }
        else
        {
            m_playerSpeed = 2.0f;
        }

        // movement with AWSD keys
        l_movementDirection = -Input.GetAxis("Vertical") * this.transform.forward;
        l_movementDirection -= Input.GetAxis("Horizontal") * this.transform.right;
    
    }

    //public static Vector3 Lerp(Vector3 a,  Vector3(a_size, a_size, a_size), 1f);
  
    public void SetSizeImmediate(float a_size, float a_mass)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);

    }

 

    public void SetValues(float a_size, float a_mass)
    {
        //m_controller.rb.mass = a_mass;

        // transform.localScale = new  Vector3 (a_size, a_size, a_size);
      
       //  if (transform.localScale.x >= endSize.x)
     //   {
      //      transform.localScale = new Vector3(a_size, a_size, a_size);
      //  }

        switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
        {
            case (int)PlayerManagerScript.SizeState.big:
                m_pushForce = 4.0f;
               M_TargetBlobSize = new Vector3(10, 10, 50);
                m_jumpHeight = 55;
                print("jumpheightshouldbesettohigh" + m_jumpHeight) ;
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(3, 3, 50);
                m_jumpHeight = 8;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(1.5f, 1.5f, 50);
                m_jumpHeight = 8;
                break;

            default:
                Debug.Log("Error! Did you forget to set a size state?");
                break;
        }
    }

   
}
