using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PrototypePlayerMovement : MonoBehaviour
{
    public TutorialManager M_TuteMan;
    public GameObject M_playerManager;
    public GameObject M_Tutorial;
    public GameObject M_CurlPrompt;
    public GameObject M_AimPrompt;
    public GameObject M_ShrinkPrompt;
    public GameObject M_BoostTip;
    public GameObject M_TiltTip;
    public GameObject M_FreeCamEntry;
    public GameObject M_LaunchPrompt;
    public GameObject M_LaunchPrompt2;
    public GameObject M_UncurlPrompt;
    public GameObject M_JumpPrompt;
    public DecalProjector M_BlobShadowDecal;
    public CustomController m_controller;
    public Vector3 playerVelocity;
    private bool m_groundedPlayer;
    [SerializeField] public float m_playerSpeed = 2.0f;
    [SerializeField] public float m_slowSlide = 1;
    [SerializeField] public float m_jumpHeight = 8.0f;
    private float m_gravityValue = -9.81f;
    private bool m_isHittingWall = false;
    private float m_pushForce = 4.0f;
    public Vector3 M_TargetBlobSize;
    private bool m_gradualSize = true;
   
    [SerializeField] AudioClip[] m_painClip;
    public AudioSource M_PainAudio;
    public GameObject M_FreshBiscuit;
    
    private UnityEngine.UI.Image m_Yellow;
    public GameObject M_BananaPrompt;
    [SerializeField] AudioClip[] m_drownClip;
    public AudioSource M_DrawnAudio;
    public ParticleSystem M_CurlingDust;
    [SerializeField] AudioClip[] m_waterDrops;
    public AudioSource M_WaterDrop;
    public static bool M_InLaunchZone;
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

    public void PlayCurlingDust()
    {
        M_CurlingDust.Play();
    }

    public void ResetPainState()
    {
        m_canPain = true;
        m_canDrown = true;
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
        }

        if (a_hit.gameObject.CompareTag("Hazard") && a_hit.gameObject.name.Contains("Water"))
        {
            StartCoroutine(YellowScreen()); //activate yellow overlay when underwater
        }
    }
    
    IEnumerator YellowScreen()
    {
        PlayerManagerScript.M_UnderWater = true;
        M_Water.SetActive(true);
        
        while (PlayerManagerScript.alphaYellow < 0.66f)
        {

            m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, PlayerManagerScript.alphaYellow);
            PlayerManagerScript.alphaYellow += 0.01f * Time.deltaTime;
            yield return null;

        }
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0.66f);
    }

    private void Fluffing()
    {
        PlayerManagerScript.M_Fluffed = true;
        M_playerManager.GetComponent<PlayerManagerScript>().ResetFluffFunction();
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
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();

        }

        if (a_hit.gameObject.CompareTag("Enemy") && M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState != 2)
        {
            Fluffing();
        }

        if (a_hit.gameObject.name.Contains("FirstLaunchZone"))
        {           
            M_TuteMan.M_FirstLaunchTute.ChangeState(FirstLaunchTuteController.TutorialState.curl);
        }

        if (a_hit.gameObject.name.Contains("ShrinkZone"))
        {
            M_AimPrompt.SetActive(false);
            M_ShrinkPrompt.SetActive(true);
        }

        if (a_hit.gameObject.name.Contains("CloseBananaZone"))
        {
            M_BananaPrompt.SetActive(false);
        }

        if (a_hit.gameObject.name.Contains("FreeCamZone"))
        {
            M_FreeCamEntry.SetActive(true);
            M_UncurlPrompt.SetActive(false);
            M_CurlPrompt.SetActive(false);
            M_AimPrompt.SetActive(false);
            M_ShrinkPrompt.SetActive(false);
            M_TiltTip.SetActive(false);
            M_BoostTip.SetActive(false);
            M_UncurlPrompt.SetActive(false);
            M_JumpPrompt.SetActive(false);
        }

        if (a_hit.gameObject.name.Contains("JumpPromptZone"))
        {
            M_CurlPrompt.SetActive(false);
            M_AimPrompt.SetActive(false);
            M_ShrinkPrompt.SetActive(false);
            M_TiltTip.SetActive(false);
            M_BoostTip.SetActive(false);
            M_FreeCamEntry.SetActive(false);
            M_UncurlPrompt.SetActive(false);
            M_JumpPrompt.SetActive(true);
        }

        if (a_hit.gameObject.name.Contains("Water"))
        {
            AudioClip clip = m_waterDrops[UnityEngine.Random.Range(0, m_waterDrops.Length)];
            M_WaterDrop.PlayOneShot(clip);
        }

       

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
        m_playerManagerScript.M_takingDamage = false;


        if (other.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_InLaunchZone = false;
            M_CurlPrompt.SetActive(false);
        }

        if (other.gameObject.name.Contains("Water")) //deactivate yellow overlay
        {

            TurnOffYellow();

            m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
        }
    }

    public void TurnOffYellow()
    {
        PlayerManagerScript.M_UnderWater = false;
        StopCoroutine(YellowScreen());
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

    private void OnCollisionStay(Collision a_hit)
    {
        Rigidbody m_rb = a_hit.collider.attachedRigidbody;
        if (m_rb != null && !m_rb.isKinematic)
        {
            m_rb.velocity = transform.forward * m_pushForce;
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
                m_pushForce = 20.0f;
                M_TargetBlobSize = new Vector3(10, 10, 50);
                m_jumpHeight = 15;
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(3, 3, 50);
                m_jumpHeight = 10;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(1.5f, 1.5f, 50);
                m_jumpHeight = 8;
                break;

            default:
                //Did you forget to set a size state?
                break;
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

       
        else if (transform.localScale.x < PlayerManagerScript.M_TargetSize && PlayerManagerScript.M_Shrinking &&!PlayerManagerScript.M_Growing)
        {
            transform.localScale = new Vector3(PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize, PlayerManagerScript.M_TargetSize);
            M_BlobShadowDecal.size = M_TargetBlobSize;
            PlayerManagerScript.M_Shrinking = false;
        } 

        else if (!PlayerManagerScript.M_Growing && !PlayerManagerScript.M_Shrinking)
        {
            PlayerManagerScript.M_Growing = true;
        }

        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying ||!M_playerManager.GetComponent<PlayerManagerScript>().isWalking())
        {
            return;
        }

        if (M_AimPrompt.activeSelf)
        {
            M_CurlPrompt.SetActive(false);
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
       
        if (!M_playerManager.GetComponent<PlayerManagerScript>().levelCompleted)
        { 
            if (Input.GetKey(KeyCode.LeftShift) && !PlayerManagerScript.M_Fluffed) 
            {
                 m_playerSpeed = 2.5f; 
            }
            else if(!PlayerManagerScript.M_Fluffed)
            {
                m_playerSpeed = 2.0f;
            }
            else if(PlayerManagerScript.M_Fluffed)
            {
                m_playerSpeed = 0.5f;
                if (!PlayerManagerScript.m_resettingFluff)
                {
                   M_playerManager.GetComponent<PlayerManagerScript>().ResetFluffFunction();
                }
            }
        }

    }
  
    public void SetSizeImmediate(float a_size, float a_mass)
    {
        transform.localScale = new Vector3(a_size, a_size, a_size);
        M_BlobShadowDecal.size = M_TargetBlobSize;
    }


    public void SetValues(float a_size, float a_mass)
    {

        switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
        {
            case (int)PlayerManagerScript.SizeState.big:
                m_pushForce = 20.0f;
                M_TargetBlobSize = new Vector3(10, 10, 50);
                m_jumpHeight = 15;
                break;

            case (int)PlayerManagerScript.SizeState.normal:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(3, 3, 50);
                m_jumpHeight = 10;
                break;

            case (int)PlayerManagerScript.SizeState.small:
                m_pushForce = 0.0f;
                M_TargetBlobSize = new Vector3(1.5f, 1.5f, 50);
                m_jumpHeight = 8;
                break;

            default:
                //Did you forget to set a size state?
                break;
        }
    }
}
