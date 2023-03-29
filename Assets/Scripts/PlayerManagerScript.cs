using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static PlayerManagerScript;

public partial class PlayerManagerScript : MonoBehaviour
{
    public bool M_isFreeFlying = false;

    public GameObject M_launchingPlayer, M_walkingPlayer, M_freeFlyingPlayer;
    public GameObject M_walkingCamera, M_launchCamera, M_additionalCamera;

    public float M_velocityRetain = 0.65f;      // how much velocity walking player gets from the launch, keep below 1
    public float M_velocityRetiainAir = 0.85f;  // how much velocity walking player mid-air gets from the launch, keep below 1

    // State enums
    public enum ArmadilloState { walk, launching };
    public ArmadilloState m_state = ArmadilloState.walk;        // Keeps track of the movement state
    public enum SizeState { small = 0, normal = 1, big = 2 };
    public float[] M_sizes = { 0.25f, 0.5f, 1.0f };             // Array of sizes, M_sizeState should be the index
    public float[] M_weights = { 1.0f, 3.0f, 10.0f };
    public Vector2[] M_cameraOffsets = { new Vector2(2.5f, 1), new Vector2(5, 2), new Vector2(10, 4) };
    public int M_sizeState = (int)SizeState.normal;             // Keeps Track of size, int type to use as M_sizes index
    public float M_jellyBounciness = 0.8f;
    public enum AbilityState { normal = 0, jelly = 1, honey = 2, both = 3 };
    public AbilityState M_abilityState = AbilityState.normal;   // Keeps track of abilities the player has

    public Animator M_BallAnimator;

    //Player HUD objects
    public Canvas M_playerHUD;
    public GameObject M_freshnessBar;
    public Slider M_freshnessSlider;
    public Image M_armadilloaf;
    public Image M_transitionSprite;
    public TextMeshProUGUI M_lifeText;

    //Player values
    public Vector3 currentCheckpoint;
    public int M_lives = 5;
    public float M_hitPoints = 5.0f;
    public bool M_takingDamage = false;
    public bool M_transitionIn = false;
    public bool M_transitionOut = false;

    public PauseManagerScript M_UIManager;
    public PrototypePlayerMovement M_PlayerMovement;
    bool m_justUnpaused;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0.0f, -19.77f, 0.0f);
        m_justUnpaused = false;
        M_additionalCamera.SetActive(false);
        M_launchCamera.SetActive(false);
        M_UIManager = FindObjectOfType<PauseManagerScript>();
        M_UIManager.Resume();
        Cursor.lockState = CursorLockMode.Locked;

        M_freshnessSlider = GetComponentInChildren<Slider>();
        M_freshnessSlider.maxValue = M_hitPoints;
        M_freshnessSlider.value = M_hitPoints;
        M_freshnessBar.SetActive(false);

        M_transitionSprite.enabled = false;

        StartCoroutine(FadeAway(M_armadilloaf));
        StartCoroutine(FadeAway(M_lifeText));

        m_state = ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
        M_sizeState = (int)SizeState.normal;
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        M_UIManager.Resume();
    }

    public IEnumerator ShowUIQuickly()
    {
        M_lifeText.text = M_lives.ToString();
        StartCoroutine(FadeIn(M_armadilloaf));
        StartCoroutine(FadeIn(M_lifeText));
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeAway(M_armadilloaf));
        StartCoroutine(FadeAway(M_lifeText));
    }

    IEnumerator FadeAway(Image a_image)
    {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                a_image.color = new Color(1, 1, 1, i);
                yield return null;
            }
    }

    IEnumerator FadeAway(TextMeshProUGUI a_text)
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            a_text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator FadeIn(Image a_image)
    {
        a_image.enabled = true;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            a_image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator FadeIn(TextMeshProUGUI a_text)
    {
        a_text.enabled = true;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            a_text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        M_BallAnimator.SetInteger("Size", M_sizeState);

        if (M_takingDamage)
        {
            M_freshnessBar.SetActive(true);
            M_hitPoints -= (1 * Time.deltaTime);
            M_freshnessSlider.value = M_hitPoints;
            if (M_hitPoints <= 0)
            {
                M_transitionIn = true;
                M_takingDamage = false;
            }
        }

        if (M_transitionIn)
        {
          M_transitionSprite.enabled = true;
          M_transitionSprite.transform.localScale += new Vector3(15.00f * Time.deltaTime, 15.00f * Time.deltaTime, 15.00f * Time.deltaTime);
          if (M_transitionSprite.transform.localScale.x >= 20.0f)
            {
                if (M_lives > 0)
                { 
                M_lives--;
                Respawn();
                StartCoroutine(ShowUIQuickly());
                M_transitionIn = false;
                M_transitionOut = true;
                }
                else
                {
                    M_transitionIn = false;
                    SceneManager.LoadScene("FailScreen");
                }
            }
        }

        if (M_transitionOut)
        {
            M_transitionSprite.transform.localScale -= new Vector3(15.00f * Time.deltaTime, 15.00f * Time.deltaTime, 15.00f * Time.deltaTime);
            if (M_transitionSprite.transform.localScale.x <= 0.01f)
            {
                M_transitionSprite.enabled = false;
                M_transitionOut = false;
            }
        }
        if (m_justUnpaused)
        {
            m_justUnpaused = false;
            return;
        }

        if (!M_isFreeFlying)
        {
            // grow
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Grow();
            }
            // shrink
            if (Input.GetKeyDown(KeyCode.E))
            {
                Shrink();
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            Respawn();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            StartCoroutine(ShowUIQuickly());
        }

        // state changing
        if (( Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) && !M_isFreeFlying)
        {
            StateCheck();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (M_isFreeFlying)
            {
                M_walkingPlayer.transform.rotation = Quaternion.identity;
                M_isFreeFlying = false;
                switch (m_state)
                {
                    case ArmadilloState.walk:
                        M_additionalCamera.SetActive(false);
                        M_walkingCamera.SetActive(true);
                        M_launchCamera.SetActive(false);
                        M_freeFlyingPlayer.SetActive(false);
                        break;
                    case ArmadilloState.launching:
                        M_additionalCamera.SetActive(false);
                        M_walkingCamera.SetActive(false);
                        M_launchCamera.SetActive(true);
                        M_freeFlyingPlayer.SetActive(false);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                StartFlying();
            }
        }
        // prototype restart, to do: have this be automatic upon failure
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("FailScreen");
        }

        if(Input.GetButtonUp("Cancel") || Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            M_UIManager.enabled = true;
            M_UIManager.Paused();
        }
    }

    public void Respawn()
    {
        CustomController l_controller = M_walkingPlayer.GetComponent<CustomController>();
        l_controller.rb.isKinematic = true;
        M_launchingPlayer.GetComponent<Rigidbody>().isKinematic = true;
        M_walkingPlayer.transform.position = currentCheckpoint;
        M_launchingPlayer.transform.position = currentCheckpoint;
        M_launchingPlayer.GetComponent<Rigidbody>().isKinematic = false;
        l_controller.rb.isKinematic = false;
        M_hitPoints = 5;
        M_freshnessSlider.value = M_hitPoints;
        M_freshnessBar.SetActive(false);
    }

    public void Resume()
    {
        M_UIManager.enabled = false;
        m_justUnpaused = true;
        Time.timeScale = 1;
    }

    void StateCheck()
    {
        switch (m_state)
        {
            case ArmadilloState.walk:
                StartLaunching();
                break;

            case ArmadilloState.launching:
                StartWalking();
                break;

            default:
                break;
        }
    }

    public void StartWalking()
    {
        // change camera
        M_launchCamera.SetActive(false);
        M_additionalCamera.SetActive(false);

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingCamera.SetActive(true);
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);

        // set rotation
        Vector3 l_rotation = M_walkingPlayer.transform.localRotation.eulerAngles;
        l_rotation.Set(0f, M_launchingPlayer.transform.localRotation.eulerAngles.y, 0f);
        M_walkingPlayer.transform.rotation = Quaternion.Euler(l_rotation);

        // retaining velocity after launch, different when mid-air and on ground
        if (M_launchingPlayer.GetComponent<PlayerLaunchScript>().isGrounded())
        {
            M_walkingPlayer.GetComponent<CustomController>().rb.velocity = M_launchingPlayer.GetComponent<Rigidbody>().velocity * M_velocityRetain;
        }
        else
        {
            M_walkingPlayer.GetComponent<CustomController>().rb.velocity = M_launchingPlayer.GetComponent<Rigidbody>().velocity * M_velocityRetiainAir;
        }
        M_walkingPlayer.GetComponent<CustomController>().PlayerLaunched();

        // deactivate other plyer states
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().Reset();
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
        m_state = ArmadilloState.walk;
        M_walkingPlayer.SetActive(true);
    }

    public void StartLaunching()
    {
        // change camera and apply new rotations to the launching player so it matches it previous rotation
        M_additionalCamera.SetActive(false);
        M_launchCamera.SetActive(true);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetDirection(M_launchCamera.transform.forward);
        M_launchingPlayer.transform.rotation = M_walkingPlayer.transform.rotation;
        M_launchCamera.transform.rotation = M_walkingCamera.transform.rotation;

        // get values from the walking armadillo
        M_launchingPlayer.transform.position = M_walkingPlayer.transform.position;
        M_launchingPlayer.GetComponent<Rigidbody>().velocity = M_walkingPlayer.GetComponent<CustomController>().rb.velocity;
       

        // rotation change
        Vector3 l_rotation = M_launchingPlayer.transform.localRotation.eulerAngles;
        l_rotation.Set(0f, M_walkingPlayer.transform.localRotation.eulerAngles.y, 0f);
        M_launchingPlayer.transform.rotation = Quaternion.Euler(l_rotation);

        if (!M_isFreeFlying)
        {
            // animation reset before deactivating
            M_walkingPlayer.GetComponent<Animator>().CrossFade("Empty", 0f);
            M_walkingPlayer.GetComponent<Animator>().Update(0.0f);
            M_walkingPlayer.GetComponent<Animator>().Update(0.0f);
        }
       
        M_freeFlyingPlayer.SetActive(false);
        M_walkingPlayer.SetActive(false);
        m_state = ArmadilloState.launching;
        M_launchingPlayer.SetActive(true);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
    }

    void StartFlying()
    {
        
        M_additionalCamera.SetActive(true);
        M_walkingCamera.SetActive(false);
        M_launchCamera.SetActive(false);

        M_freeFlyingPlayer.SetActive(true);
        M_isFreeFlying = true;

        switch (m_state)
        {
            case ArmadilloState.walk:
                M_freeFlyingPlayer.transform.position = M_walkingPlayer.transform.position;
                break;
            case ArmadilloState.launching:
                M_freeFlyingPlayer.transform.position = M_launchingPlayer.transform.position;
                break;

            default:
                break;
        }
    }

    public void Grow()
    {
        M_sizeState++;
        if (M_sizeState > 2)
        {
            M_sizeState = 2;
        }
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
    }

    public void Shrink()
    {
        M_sizeState--;
        if (M_sizeState < 0)
        {
            M_sizeState = 0;
        }
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        ResetAbilities();
    }

    //acquire property of bounciness with Jelly pick-up, called in PowerUp_SizeChanger (script)
    public void Jellify()
    {
        {
            M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = M_jellyBounciness;
            M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 0.6f;

            M_walkingPlayer.GetComponent<SphereCollider>().material.bounciness = M_jellyBounciness;

            M_PlayerMovement.m_jumpHeight = 8;
            M_abilityState = AbilityState.jelly;
        }
    }
    //acquire property of stickiness with Jelly pick-up, called in PowerUp_SizeChanger (script)
    public void Honify()
    {
        {
            M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 50f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 50f;

            M_walkingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;

            M_PlayerMovement.m_jumpHeight = 5;
            M_abilityState = AbilityState.honey;
        }
    }

    public void ResetAbilities()
    {
        M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;
        M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
        M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 0.6f;

        M_walkingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;

        M_PlayerMovement.m_jumpHeight = 8;
        M_abilityState = AbilityState.normal;
        M_abilityState = 0;
      
    }

    public bool isWalking()
    {
        return (m_state == ArmadilloState.walk);
    }
}
