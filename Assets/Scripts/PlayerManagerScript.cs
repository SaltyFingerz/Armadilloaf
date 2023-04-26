using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static PlayerManagerScript;

public partial class PlayerManagerScript : MonoBehaviour
{

    public GameObject M_launchingPlayer, M_walkingPlayer, M_freeFlyingPlayer;
    public GameObject M_walkingCamera, M_launchCamera, M_additionalCamera;
    public RenderingScript M_Rendering;
    public float M_velocityRetain = 0.65f;      // how much velocity walking player gets from the launch, keep below 1
    public float M_velocityRetiainAir = 0.85f;  // how much velocity walking player mid-air gets from the launch, keep below 1
    public ParticleSystem M_EjectionPS1;
    public ParticleSystem M_EjectionPS2;
    public GameObject M_JellyDecal;
    // State enums
    public enum ArmadilloState { walk, launching };
    public ArmadilloState m_state = ArmadilloState.walk;        // Keeps track of the movement state
    public enum SizeState { small = 0, normal = 1, big = 2 };
    public  float[] M_sizes = { 0.25f, 0.5f, 1.0f };             // Array of sizes, M_sizeState should be the index
    public float[] M_weights = { 1.0f, 3.0f, 10.0f };
    public Vector2[] M_cameraOffsets = { new Vector2(2.5f, 1), new Vector2(5, 2), new Vector2(10, 4) };
    public int M_sizeState = (int)SizeState.normal;             // Keeps Track of size, int type to use as M_sizes index
    public float M_jellyBounciness = 0.8f;
    public enum AbilityState { normal = 0, jelly = 1, honey = 2, both = 3 };
    public AbilityState M_abilityState = AbilityState.normal;   // Keeps track of abilities the player has
    public static float M_TargetSize;
    public Animator M_BallAnimator;
    public static bool M_Growing = false;
    public static bool M_Shrinking = false;
    //Player HUD objects
    public Canvas M_playerHUD;
    public Image M_freshnessBiscuit;
    public Sprite[] M_freshnessBiscuitLevels = new Sprite[5];
    public bool[] M_biscuitBites = new bool[4];
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
    public bool M_isFreeFlying = false;

    public PauseManagerScript M_UIManager;
    public PrototypePlayerMovement M_PlayerMovement;
    public AudioSource M_musicPlayer;
    bool m_justUnpaused;

    [SerializeField] AudioClip[] m_biscuitClip;
    [SerializeField] float m_invulnerabilityPeriodSeconds = 2.0f;
    float m_invulnerabilityTimerSeconds = 2.0f;
    public AudioSource M_biscuitBreak;

    public Renderer M_Renderer;
    public Renderer M_2DRenderer;


    public Animator M_biscuitAnimator;

    public static bool M_Fluffed;
    public static bool M_Jellied;
    public int M_FruitCollected;
    public TextMeshProUGUI M_FruitUI;
    public TextMeshProUGUI M_FruitUIFin;

    public AudioSource M_GrowAudio;
    public AudioSource M_ShrinkAudio;
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

        ResetBiscuitBites();
        ResetAbilities();
        M_freshnessBiscuit.enabled = true;
        M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[0];
        M_freshnessBiscuit.GetComponent<Animator>().SetTrigger("Safe");

        M_transitionSprite.enabled = false;

        m_state = ArmadilloState.walk;

        M_walkingPlayer.transform.position = M_launchingPlayer.transform.position;
        M_walkingPlayer.SetActive(true);
        M_launchingPlayer.SetActive(false);
        M_freeFlyingPlayer.SetActive(false);
        M_sizeState = (int)SizeState.normal;
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        //M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        M_UIManager.Resume();

        Color StartColor = M_Renderer.material.color;
        Color StartColor2D = M_2DRenderer.material.color;
        StartCoroutine(ShowUIQuickly());
    }

    public IEnumerator ShowUIQuickly()
    {
        M_lifeText.text = M_lives.ToString();
        StartCoroutine(FadeIn(M_armadilloaf));
        StartCoroutine(FadeIn(M_lifeText));

        AudioClip clip = m_biscuitClip[UnityEngine.Random.Range(0, m_biscuitClip.Length)];
        M_biscuitBreak.PlayOneShot(clip);

        yield return new WaitForSeconds(2);
        StartCoroutine(FadeAway(M_armadilloaf));
        StartCoroutine(FadeAway(M_lifeText));
    }

    public IEnumerator FadeAway(Image a_image)
    {
            // loop over 1 second backwards
            for (float i = 3; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                a_image.color = new Color(1, 1, 1, i);
                yield return null;
            }
    }

    public IEnumerator FadeAway(TextMeshProUGUI a_text)
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            a_text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public IEnumerator FadeIn(Image a_image)
    {
        a_image.enabled = true;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            a_image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public IEnumerator FadeIn(TextMeshProUGUI a_text)
    {
        a_text.enabled = true;
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            a_text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public IEnumerator ChangePlayerColor(Color newColor, float duration)
    {
        float elapsedTime = 0;
        Color startColor = M_Renderer.material.color;

        while (elapsedTime < duration)
        {
            M_Renderer.material.color = Color.Lerp(startColor, newColor, elapsedTime / duration);
            M_freshnessBiscuit.color = Color.Lerp(startColor, newColor, elapsedTime / duration);

            M_2DRenderer.material.color = Color.Lerp(startColor, newColor, elapsedTime / duration);
            M_freshnessBiscuit.color = Color.Lerp(startColor, newColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        M_Renderer.material.color = newColor;
    }






    // Update is called once per frame
    void Update()
    {
        //print("fluffed" + M_Fluffed);
       

        M_FruitUI.text = M_FruitCollected.ToString();
        M_FruitUIFin.text = M_FruitCollected.ToString();
        m_invulnerabilityTimerSeconds += Time.deltaTime;
        M_BallAnimator.SetInteger("Size", M_sizeState);

        M_TargetSize = M_sizes[M_sizeState];

        if (M_sizeState == 2)
        {
            M_BallAnimator.SetTrigger("Big");
        }

        else if (M_sizeState == 0)
        {
            M_BallAnimator.SetTrigger("Small");
        }


        if (M_Jellied)
        {
            StartCoroutine(ChangePlayerColor(Color.magenta, 0.2f));

           
            M_Fluffed = false;
            print("pink ball material");
        }

        if (!M_Jellied && !M_Fluffed)
        {
            StartCoroutine(ChangePlayerColor(Color.white, 0.2f));
        }
        if (M_Fluffed)
        {
            StartCoroutine(ChangePlayerColor(Color.cyan, 0.2f));
          
        }
      

        if (M_takingDamage)
        {
            M_freshnessBiscuit.GetComponent<Animator>().SetTrigger("Damage");
            M_Rendering.IncreaseVignette();
            M_Rendering.DecreaseSaturation();

            if (M_musicPlayer.pitch > 0f)
            { 
            M_musicPlayer.pitch -= 0.001f;
            }
        }

        else
        {
            M_freshnessBiscuit.GetComponent<Animator>().SetTrigger("Safe");
            M_freshnessBiscuit.enabled = true;
            if (M_musicPlayer.pitch < 1.0f && !M_takingDamage)
            { 
            M_musicPlayer.pitch += 0.01f;
            }
            else if (M_musicPlayer.pitch > 1.0f)
            {
                M_musicPlayer.pitch = 1.0f;
            }
        }

        if (M_takingDamage && m_invulnerabilityTimerSeconds > m_invulnerabilityPeriodSeconds)
        {
            Debug.Log("ow");
            m_invulnerabilityTimerSeconds = 0.0f;
            M_freshnessBiscuit.enabled = true;
            M_hitPoints -= (1.0f * Time.deltaTime);
            Debug.Log(M_hitPoints);

            if (M_hitPoints <= 0)
            {
                M_transitionIn = true;
                M_takingDamage = false;
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[0];
                M_freshnessBiscuit.enabled = false;
                ResetBiscuitBites();
            }
            else if (M_freshnessBiscuit.sprite == M_freshnessBiscuitLevels[3])
            {
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[4];
                if (M_biscuitBites[3] == false)
                {
                    M_biscuitAnimator.Play("Base Layer.BiscuitAnimation", 0, 0);
                    M_biscuitBites[3] = true;
                }
            }
            else if (M_freshnessBiscuit.sprite == M_freshnessBiscuitLevels[2])
            {
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[3];
                if (M_biscuitBites[2] == false)
                {
                    M_biscuitAnimator.Play("Base Layer.BiscuitAnimation", 0, 0);
                    M_biscuitBites[2] = true;
                }
            }
            else if (M_freshnessBiscuit.sprite == M_freshnessBiscuitLevels[1])
            {
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[2];
                if (M_biscuitBites[1] == false)
                {
                    M_biscuitAnimator.Play("Base Layer.BiscuitAnimation", 0, 0);
                    M_biscuitBites[1] = true;
                }
            }
            else if (M_freshnessBiscuit.sprite == M_freshnessBiscuitLevels[0])
            {
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[1];
                if (M_biscuitBites[0] == false)
                { 
                M_biscuitAnimator.Play("Base Layer.BiscuitAnimation", 0, 0);
                M_biscuitBites[0] = true;
                }
            }
            else
            {
                M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[0];
            }


            AudioClip clip = m_biscuitClip[UnityEngine.Random.Range(0, m_biscuitClip.Length)];
            M_biscuitBreak.PlayOneShot(clip);

            M_lives--;
            M_lifeText.text = M_lives.ToString();

            if (M_lives == 0)
            {
                M_lives = 5;
                M_lifeText.text = M_lives.ToString();
                Respawn();
                M_transitionIn = true;
            }
            M_takingDamage = false;
        }

      

        // transition sprite starts growing after 5 deaths
        if (M_transitionIn)
        {
          M_transitionSprite.enabled = true;
          M_transitionSprite.transform.localScale += new Vector3(15.00f * Time.deltaTime, 15.00f * Time.deltaTime, 15.00f * Time.deltaTime);
          if (M_transitionSprite.transform.localScale.x >= 15.0f)
            {
                M_transitionIn = false;
                M_transitionOut = true;
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
          //   grow
           if (Input.GetKeyDown(KeyCode.Q) && M_sizeState ==0)
            {
               Grow();
            }
            // shrink
            if (Input.GetKeyDown(KeyCode.E) && M_sizeState > 0)
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
        if (( Input.GetMouseButtonDown(1)) && !M_isFreeFlying)
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
        if(M_sizeState==2)
        {
            M_sizeState = 1;
        }
        M_Rendering.ResetVignette();
        M_Rendering.RestoreSaturation();
        M_PlayerMovement.GetComponent<PrototypePlayerMovement>().TurnOffYellow();
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().TurnOffYellow();
        CustomController l_controller = M_walkingPlayer.GetComponent<CustomController>();
        l_controller.rb.isKinematic = true;
        M_launchingPlayer.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log(currentCheckpoint);
        Debug.Log(M_walkingPlayer.transform.position);
        M_walkingPlayer.transform.position = currentCheckpoint;
        M_launchingPlayer.transform.position = currentCheckpoint;
        M_launchingPlayer.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(currentCheckpoint);
        Debug.Log(M_walkingPlayer.transform.position);
        l_controller.rb.isKinematic = false;
        M_hitPoints = 5;
        M_freshnessBiscuit.enabled = false;
        StartLaunching();
        StartWalking();
        M_Fluffed = false;
        M_Jellied = false;
        ResetAbilities();

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
                Curl();
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
        M_walkingPlayer.GetComponent<CustomController>().SetMouseRotation(M_launchingPlayer.GetComponent<PlayerLaunchScript>().GetMouseRotation());
        M_walkingPlayer.GetComponent<CustomController>().SetRotation(M_launchCamera.transform.forward);

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

        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetSizeImmediate(M_sizes[M_sizeState], M_weights[M_sizeState]);
        
    }

    public void Curl()
    {
        M_walkingPlayer.GetComponent<Animator>().SetTrigger("Curl");
        M_BallAnimator.SetBool("Grow", false);
        M_BallAnimator.SetBool("Shrink", false);
    }
    public void StartLaunching()
    {
        

        M_additionalCamera.SetActive(false);
        M_walkingPlayer.SetActive(false);
        M_launchCamera.SetActive(true);

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
        m_state = ArmadilloState.launching;
        M_launchingPlayer.SetActive(true);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetMouseRotation(M_walkingPlayer.GetComponent<CustomController>().GetMouseRotation());
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetDirection(M_walkingPlayer.transform.forward);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        //M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
       
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


    public void Regenerate()
    {
        M_hitPoints = 5;
        M_freshnessBiscuit.sprite = M_freshnessBiscuitLevels[0];
        ResetBiscuitBites();
        M_Rendering.RestoreSaturation();
        M_Rendering.ResetVignette();
    }

    public void Grow()
    {
        M_BallAnimator.SetBool("Grow", true);
        M_BallAnimator.SetBool("Shrink", false);
        M_BallAnimator.SetBool("Small", false);
        M_Shrinking = false;
        M_Growing = true;
       
        
        M_sizeState++;
        if (M_sizeState > 2)
        {
            M_sizeState = 2;
        }
        M_BallAnimator.SetBool("Grow", true);
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        //M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        //StartCoroutine(DelayedCameraOffset());
        M_GrowAudio.Play();
        

    }


    IEnumerator DelayedCameraOffset()
    {
        yield return new WaitForSeconds(1);
       
            M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        

    }

    public void Shrink()
    {
        M_BallAnimator.SetBool("Grow", false);
        M_BallAnimator.SetBool("Shrink", true);
        M_Shrinking = true;
        
       
        M_sizeState--;
        if (M_sizeState < 0)
        {
            M_sizeState = 0;
        }
        M_walkingPlayer.GetComponent<PrototypePlayerMovement>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
        M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetValues(M_sizes[M_sizeState], M_weights[M_sizeState]);
       // M_launchingPlayer.GetComponent<PlayerLaunchScript>().SetCameraOffset(M_cameraOffsets[M_sizeState]);
        if(M_abilityState == AbilityState.jelly)
        {
            StartCoroutine(EjectionRoutine());
        }
        ResetAbilities();
        M_ShrinkAudio.Play();
        M_Growing = false;


    }

    public Quaternion M_JellyDecalRotation;
    public Transform M_JellyParent;
    public Vector3 M_JellyDecalPosition;
    IEnumerator EjectionRoutine()
    {
        M_EjectionPS1.Play();
        yield return new WaitForSeconds(1f);
        M_EjectionPS2.Play();
        yield return new WaitForSeconds(1f);
        Instantiate(M_JellyDecal, M_JellyParent.transform.position, M_JellyDecalRotation);
        print("splat");

    }

    //acquire property of bounciness with Jelly pick-up, called in PowerUp_SizeChanger (script)
    public void Jellify()
    {
        {
            M_launchingPlayer.GetComponent<SphereCollider>().material.bounciness = M_jellyBounciness;
            M_launchingPlayer.GetComponent<SphereCollider>().material.dynamicFriction = 0.6f;
            M_launchingPlayer.GetComponent<SphereCollider>().material.staticFriction = 0.6f;

            M_walkingPlayer.GetComponent<SphereCollider>().material.bounciness = M_jellyBounciness;
            M_Renderer.material.color = Color.magenta;
            M_2DRenderer.material.color = Color.magenta;
            M_freshnessBiscuit.color = Color.magenta;
            //or  material.SetColor(""_Color", new Vector 4 (1,1,1,1));
            M_PlayerMovement.m_jumpHeight = 8;
            M_abilityState = AbilityState.jelly;
            M_Jellied = true;
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
        M_Renderer.material.color = Color.white;
        M_2DRenderer.material.color = Color.white;
        // M_Renderer.material.SetColor("StartColor", new Vector4 (1, 1, 1, 1));
        // M_2DRenderer.material.SetColor("StartColor", new Vector4(1, 1, 1, 1));
        M_freshnessBiscuit.color = Color.white;
        StartCoroutine(ChangePlayerColor(Color.white, 0.2f));
        M_PlayerMovement.m_jumpHeight = 8;
        M_abilityState = AbilityState.normal;
        M_abilityState = 0;
        M_Jellied = false;
        M_Fluffed = false;
      
    }

    public void ResetBiscuitBites()
    {

        for (int i = 0; i < M_biscuitBites.Length; i++)
        {
            M_biscuitBites[i] = false;
        }
    }

    public bool isWalking()
    {
        return (m_state == ArmadilloState.walk);
    }
}
