using Cinemachine;
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
    public int M_sizeState = (int)SizeState.normal;             // Keeps Track of size, int type to use as M_sizes index
    public float M_jellyBounciness = 0.8f;
    public enum AbilityState { normal = 0, jelly = 1, honey = 2, both = 3 };
    public AbilityState M_abilityState = AbilityState.normal;   // Keeps track of abilities the player has

    //Player HUD objects
    public Canvas M_playerHUD;
    public GameObject M_freshnessBar;
    public Image armadilloaf;
    public TextMeshProUGUI lifeText;

    //Player values
    public Vector3 currentCheckpoint;
    public int lives = 5;

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

        M_freshnessBar.SetActive(false);

        StartCoroutine(FadeAway(armadilloaf));
        StartCoroutine(FadeAway(lifeText));

        m_state = ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
        M_sizeState = (int)SizeState.normal;
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        currentCheckpoint = M_walkingPlayer.transform.position;
        M_UIManager.Resume();
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
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            a_image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

        // Update is called once per frame
        void Update()
    {
        if (m_justUnpaused)
        {
            m_justUnpaused = false;
            return;
        }
        // grow
        if (Input.GetKeyDown(KeyCode.T))
        {
            Grow();
        }
        // shrink
        if (Input.GetKeyDown(KeyCode.I))
        {
            Shrink();
        }

        // state changing
        if (Input.GetKeyUp(KeyCode.Q) && !M_isFreeFlying)
        {
            StateCheck();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (M_isFreeFlying)
            {

                switch (m_state)
                {
                    case ArmadilloState.walk:
                        StartWalking();
                        break;
                    case ArmadilloState.launching:
                        StartLaunching();
                        break;

                    default:
                        break;
                }
                M_isFreeFlying = false;
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
        if (M_walkingPlayer)
        {
            CustomController m_controller = M_walkingPlayer.GetComponent<CustomController>();
            m_controller.rb.isKinematic = true;
            M_walkingPlayer.transform.position = currentCheckpoint;
            m_controller.rb.isKinematic = false;
        }
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

        // enable walking
        m_state = ArmadilloState.walk;
        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
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
    }

    public void StartLaunching()
    {
        M_additionalCamera.SetActive(false);

        // change camera
        M_launchCamera.SetActive(true);

        // get values from the walking armadillo
        M_launchingPlayer.transform.position = M_walkingPlayer.transform.position;
        M_launchingPlayer.GetComponent<Rigidbody>().velocity = M_walkingPlayer.GetComponent<CustomController>().rb.velocity;
        m_state = ArmadilloState.launching;

        // rotation change
        Vector3 l_rotation = M_launchingPlayer.transform.localRotation.eulerAngles;
        l_rotation.Set(0f, M_walkingPlayer.transform.localRotation.eulerAngles.y, 0f);
        M_launchingPlayer.transform.rotation = Quaternion.Euler(l_rotation);

        // activate and deactivate players
        M_launchingPlayer.SetActive(true);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);

        if (!M_isFreeFlying)
        {
            // animation reset before deactivating
            M_walkingPlayer.GetComponent<Animator>().CrossFade("Empty", 0f);
            M_walkingPlayer.GetComponent<Animator>().Update(0.0f);
            M_walkingPlayer.GetComponent<Animator>().Update(0.0f);
        }
       
        M_walkingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
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

            M_PlayerMovement.m_jumpHeight = 2;
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

            M_PlayerMovement.m_jumpHeight = 1;
            M_abilityState = AbilityState.honey;
        }
    }

    public void ResetAbilities()
    {
        M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = 0f;
        M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
        M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 0.6f;

        M_PlayerMovement.m_jumpHeight = 1;
        M_abilityState = 0;
    }

    public bool isWalking()
    {
        return (m_state == ArmadilloState.walk);
    }
}
