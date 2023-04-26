using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
//using System.Drawing;
using EZCameraShake;

public class PlayerLaunchScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private GameObject M_arrow;
    private GameObject M_arrowMaximum;
    public GameObject M_Tutorial;

    public GameObject M_launchCamera;
    public GameObject M_playerManager;
    public GameObject M_Trail;
    public GameObject M_Water;
    public GameObject M_FreshBiscuit;

    public GameObject M_CurlPrompt;
    public GameObject M_AimPrompt;


    public UnityEngine.UI.Image M_fillImage;
    public RenderingScript M_RenderScript;
    public LaunchTrailScript M_TrailScript;
    
    public Canvas M_canvas;
    public GameObject[] M_BigCans;
    public GameObject[] M_Cereals;
    float m_rotationMouseY = 0.0f, m_rotationMouseX = 0.0f;
    public float m_mouseSensitivityX;
    public float m_mouseSensitivityY;
    bool m_collisionEnter = false;
    bool m_collisionStay = false;
    public GameObject M_FinishUI;
    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;
    bool m_canShake = false;

    private float alphaYellow;
    private UnityEngine.UI.Image m_Yellow;

    public int M_maxPower;
    public float M_minimumDirectionY, M_maximumDirectionY;

    [SerializeField] float m_powerSizeStep = 1.0f;          // Determines how big is the scale difference in the arrow when choosing launching power.
    [SerializeField] float m_baseLength = 10.0f;            // Minimum lenght of the arrow.
    [SerializeField] private float m_minimumSpeed = 0.4f;   // Speed minimum limit before the player changes to walking player.
    [SerializeField] private AudioSource m_launchSound;
    [SerializeField] AudioClip[] m_sLaunchSounds;
    Vector2 M_cameraOffset = new Vector2(14.0f, 8.0f);
    float m_cameraRotationY;

    [SerializeField] private float m_powerUpSpeed;
    [SerializeField] private float m_powerDownSpeed;

    [SerializeField] AudioClip[] m_drownClip;
    public AudioSource M_DrawnAudio;

    [SerializeField] AudioClip[] m_waterDrops;
    public AudioSource M_WaterDrop;

    private bool m_canDrown = true;
    bool m_powerGoingUp = true;
    public ParticleSystem M_ImpactVFX;
    Renderer m_renderer;
    public void Start()
    {
        // get objects
        m_rigidbody = GetComponent<Rigidbody>();
        M_arrowMaximum = this.gameObject.transform.GetChild(0).gameObject;
        M_arrow = this.gameObject.transform.GetChild(1).gameObject;
        m_renderer = GetComponent<Renderer>();

        // assign starting values
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
        M_fillImage.fillAmount = 0.0f;
        M_arrowMaximum.transform.localScale = new Vector3(5.4f, 5.4f, m_baseLength + m_powerSizeStep * M_maxPower);
        m_cameraRotationY = -Mathf.Cos(24f);
        M_BigCans = GameObject.FindGameObjectsWithTag("Big Can");
        M_Cereals = GameObject.FindGameObjectsWithTag("Cereal");

        m_Yellow = M_Water.GetComponent<UnityEngine.UI.Image>();
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
    }


    // Handle rigidbody physics
    public void FixedUpdate()
    {
        //print("velocity" + m_rigidbody.velocity.magnitude);
        // if paused or free flying, don't update
        if (Time.timeScale < 0.1f || M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            if (m_launchingStage == 0)
            {
                // player can roll when AWSD is pressed and was not rolling
                LaunchingStart();
            }
        }

        else if (isGrounded() && m_rigidbody.velocity.magnitude < m_minimumSpeed)
        {
            // if no key was pressed and player is slow, stop rolling
            print("relaunch");
            m_launchingStage = 0;
        }

        else
        {
            //continue rolling
             m_launchingStage = 1;
        }

        if(!isGrounded()) //activate trail particle system when ball is in the air
        {
            M_TrailScript.ActivateTrail(); 
        }

       

        // Manage launching stage
        switch (m_launchingStage)
        {
            case 0:
                DirectionInput();
                break;

            case 1:
                RollingDirectionInput();
                break;

            default:
                break;
        }
    }

    // Handle key inputs
    public void Update()
    {

        // if paused or free flying, don't update
        if (Time.timeScale < 0.1f || M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        // Manage launching stage
        if (m_launchingStage == 0)
        {
            HandleLaunchInput();
        }
        if (PlayerManagerScript.M_Fluffed)
        {
            Fluffing();
        }
        else
        {
            Defluff();
        }

        //control rate of particle system trail depending on speed of ball
        //produces an error - wip.
        /*
        if(m_rigidbody.velocity.magnitude < 1)
        {
            M_TrailScript.SlowDownTrail();

        }

        else if (m_rigidbody.velocity.magnitude >= 1 && m_rigidbody.velocity.magnitude <= 3)
        {
            M_TrailScript.MediumTrail();
        }

        else if(m_rigidbody.velocity.magnitude >3)
        {
            M_TrailScript.SpeedUpTrail();
        }
        */

    }

    void RollingDirectionInput()
    {
        //get player input
        float l_playerVerticalInput = Input.GetAxis("Vertical");
        float l_playerHorizontalInput = Input.GetAxis("Horizontal");

        float l_multiplier = 30.0f;
        float l_angleChange = 160.0f;

        if (isGrounded())
        {
            l_multiplier = 60.0f;
            l_angleChange = 160.0f;
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f)
        {
            l_multiplier = 0.0f;
        }

            Vector3 l_direction = m_rigidbody.velocity;
        l_direction.y = 0.0f;
        l_direction.Normalize();

        l_direction = Vector3.RotateTowards(l_direction, M_launchCamera.transform.right * l_playerHorizontalInput, l_angleChange * Time.fixedDeltaTime, 0.0f);
        l_direction = Vector3.RotateTowards(l_direction, M_launchCamera.transform.forward * l_playerVerticalInput, l_angleChange * Time.fixedDeltaTime, 0.0f);

        m_rigidbody.AddForce(l_direction * l_multiplier);

        float l_maxSpeed = 80.0f;
        if (m_rigidbody.velocity.magnitude > l_maxSpeed)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * l_maxSpeed;
        }


        // Calculate camera rotation
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;

        //camera transform change
        HandleCameraInput(l_desiredRotation);

    }

    void HandleLaunchInput()
    {
        // holding LMB -> bar moving
        if (Input.GetMouseButton(0))
        {
            // power going up
            if (m_powerGoingUp)
            {
                if (m_launchingPower > M_maxPower)
                {
                    m_launchingPower = M_maxPower;
                    m_powerGoingUp = false;
                }

                m_launchingPower += m_powerUpSpeed * Time.deltaTime;

            }
            // power going down
            else
            {
                m_launchingPower -= m_powerDownSpeed * Time.deltaTime;
                if (m_launchingPower < 0.5f)
                {
                    m_powerGoingUp = true;
                    m_launchingPower = 0.5f;
                }

            }

            // visual indicators
            M_fillImage.fillAmount = m_launchingPower / M_maxPower;
            M_arrow.transform.localScale = new Vector3(5, 5, m_baseLength + m_powerSizeStep * m_launchingPower);

        }

        //override player rotation
        if (Input.GetMouseButtonUp(0))
        {
            LaunchingStart();
            return;
        }

    }

    public void StopGrowingAnimation() //this is an animation event called at the end of the growing animation, to play the static idle animation instead 
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Grow", false);
    }
    public void Reset()
    {
        m_rigidbody.freezeRotation = false;
        m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        M_arrow.SetActive(true);
        M_arrowMaximum.SetActive(true);
        M_arrow.transform.localScale = new Vector3(5f, 5f, 5f);
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);

        m_launchingStage = 0;
        m_launchingPower = 0;
        M_canvas.enabled = true;
    }
    private void LaunchingStart()
    {
        M_canvas.enabled = false;
        m_direction.Normalize();
        m_launchingStage++;
        m_rigidbody.constraints = RigidbodyConstraints.None;
      //  m_rigidbody.freezeRotation = true;
        M_arrow.SetActive(false);
        M_arrowMaximum.SetActive(false);
        m_launchingPower *= 3.0f;
       // m_rigidbody.velocity = new Vector3(-m_direction.x * m_launchingPower, 0.0f * m_launchingPower, -m_direction.z * m_launchingPower);
        m_rigidbody.AddForce(new Vector3(-m_direction.x * m_launchingPower * 100, 0  , -m_direction.z * m_launchingPower * 100));
       // m_rigidbody.AddForce(-m_direction * m_launchingPower, ForceMode.Impulse);
        m_rigidbody.freezeRotation = false;

        m_launchingPower = 0.0f;
        M_fillImage.fillAmount = m_launchingPower / M_maxPower;

        UpdateTrailRotation();

        // m_launchSound.Play();
        AudioClip clip = m_sLaunchSounds[UnityEngine.Random.Range(0, m_sLaunchSounds.Length)];
        m_launchSound.PlayOneShot(clip);

        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Launching");
        M_TrailScript.ActivateTrail();
        StartCoroutine(BlurDisableCooldown());
        if (m_canBlur)
        {
            M_RenderScript.EnableBlur();
        }
    }

    public void UpdateTrailRotation()
    {
        Vector3 eulerRotation = new Vector3(M_Trail.transform.eulerAngles.x, transform.eulerAngles.y -180, M_Trail.transform.eulerAngles.z);

        M_Trail.transform.rotation = Quaternion.Euler(eulerRotation);
    }

    private void DirectionInput()
    {
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;

        //rotate the player after getting the updated direction, interpolate
        Quaternion l_rotation = Quaternion.LookRotation(new Vector3(-l_desiredRotation.x, 0.0f, -l_desiredRotation.z));
        m_rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, l_rotation, Time.fixedDeltaTime * 10.0f));

        //camera transform change
        HandleCameraInput(l_desiredRotation);
    }

    Vector3 GetDesiredRotationFromMouseInput()
    {
        // Mouse is moved, calculate camera rotation from the mouse position difference between frames
        float l_mouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivityX;
        m_rotationMouseX += l_mouseX;
        m_rotationMouseY += Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivityY;
        m_rotationMouseY = Mathf.Clamp(m_rotationMouseY, 85.0f, 170.0f);

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_rotation;
        l_rotation.x = m_direction.x * Mathf.Cos(l_mouseX) - m_direction.z * Mathf.Sin(l_mouseX);
        l_rotation.y = m_cameraRotationY;
        l_rotation.z = m_direction.x * Mathf.Sin(l_mouseX) + m_direction.z * Mathf.Cos(l_mouseX);

        return l_rotation;
    }

    void HandleCameraInput(Vector3 a_rotation)
    {
        Vector3 l_axis = Vector3.Cross(a_rotation, Vector3.up);
        if (l_axis == Vector3.zero) l_axis = Vector3.right;
        Vector3 l_direction = Quaternion.AngleAxis(-m_rotationMouseY, l_axis) * a_rotation;
        Vector3 l_directionRotation = Quaternion.AngleAxis(-m_rotationMouseY - 24.0f, l_axis) * a_rotation;

        m_cameraRotationY = Mathf.Lerp(m_cameraRotationY, l_direction.y, Time.fixedDeltaTime * 5.0f);

        l_direction.Normalize();

        Quaternion l_rotationFinal = Quaternion.LookRotation(l_directionRotation);

        //camera transform change
        M_launchCamera.transform.rotation = Quaternion.Lerp(M_launchCamera.transform.rotation, l_rotationFinal, Time.fixedDeltaTime * 10.0f);
        M_launchCamera.transform.position = this.transform.position + new Vector3(-M_launchCamera.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y * (-m_cameraRotationY), -M_launchCamera.transform.forward.z * M_cameraOffset.x);

    }

    public void SetValues(float a_size, float a_mass)
    {
        GetComponent<Animator>().enabled = false;

        transform.localScale = new Vector3(a_size, a_size, a_size);
        m_rigidbody.mass = a_mass;
        GetComponent<Animator>().enabled = true;

       
            switch (M_playerManager.GetComponent<PlayerManagerScript>().M_sizeState)
            {
                case (int)PlayerManagerScript.SizeState.big:
                m_launchSound.pitch = 0.5f;
               /* foreach (GameObject can in M_BigCans)
                {
                    can.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                foreach (GameObject cereal in M_Cereals)
                {
                    cereal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                } */
                    break;

                case (int)PlayerManagerScript.SizeState.normal:
                m_launchSound.pitch = 1f;
               /* foreach (GameObject can in M_BigCans)
                {
                    can.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                foreach (GameObject cereal in M_Cereals)
                {
                    cereal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                } */
                break;

                case (int)PlayerManagerScript.SizeState.small:
                m_launchSound.pitch = 1.5f;
              /*  foreach (GameObject can in M_BigCans)
                {
                    can.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
                foreach (GameObject cereal in M_Cereals)
                {
                    cereal.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                } */
                break;

                default:
                    Debug.Log("Error! Did you forget to set a size state?");
                    break;
            }
        

    }
    private void OnTriggerEnter(Collider a_hit)
    {
        if ( a_hit.gameObject.CompareTag("Collectible"))
        {
            print("collectible +1");
            a_hit.gameObject.GetComponent<HoverScript>().PlayPickupSound();
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();
        }

        if (a_hit.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_CurlPrompt.SetActive(false);
            M_AimPrompt.SetActive(true);

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

        if (a_hit.gameObject.name.Contains("Water"))
        {
            AudioClip clip = m_waterDrops[UnityEngine.Random.Range(0, m_waterDrops.Length)];
            M_WaterDrop.PlayOneShot(clip);
        }

        if (a_hit.gameObject.name.Contains("Finish"))
        {
            Time.timeScale = 0f;
            M_FinishUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

    }

    private void OnTriggerExit(Collider a_hit)
    {
        if (a_hit.gameObject.name.Contains("Water")) //deactivate yellow overlay
        {
            print("exit water");
            StopCoroutine(YellowScreen());
            // M_Water.SetActive(false);
            m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
        }
       
    }

    IEnumerator waitForDrown()
    {
        yield return new WaitForSeconds(3);
        m_canDrown = true;
    }

    public void DrownSound()
    {
        AudioClip clip = m_drownClip[UnityEngine.Random.Range(0, m_drownClip.Length)];
        M_DrawnAudio.PlayOneShot(clip);
        m_canDrown = false;
        StartCoroutine(waitForDrown());
    }

    private void OnTriggerStay(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Hazard") || a_hit.gameObject.CompareTag("Enemy"))
        {
            PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            if (m_playerManagerScript.M_sizeState != 2)
            {
                m_playerManagerScript.M_takingDamage = true;
            }


            if (a_hit.gameObject.name.Contains("Water") && m_canDrown)
            {
                print("drown sound");
                DrownSound();
            }

            if (a_hit.gameObject.CompareTag("Enemy") && m_playerManagerScript.M_sizeState != 2)
            {
                Fluffing();

            }

        }
         if (a_hit.gameObject.CompareTag("Hazard") && a_hit.gameObject.name.Contains("Water"))
        {

            StartCoroutine(YellowScreen()); //activate yellow overlay when under honey water
           
        }
       
    }

    public void TurnOffYellow()
    {
        StopCoroutine(YellowScreen());
        // M_Water.SetActive(false);
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0f);
    }

    IEnumerator YellowScreen() //yellow overlay activator
    {
        M_Water.SetActive(true);
        while (alphaYellow < 0.66f)
        {

            m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, alphaYellow);
            alphaYellow += 0.01f * Time.deltaTime;
            yield return null;

        }
        m_Yellow.color = new Color(m_Yellow.color.r, m_Yellow.color.g, m_Yellow.color.b, 0.66f);
        yield return null;
    }

    private void Fluffing()
    {
        PlayerManagerScript.M_Fluffed = true;
        m_renderer.material.color = Color.cyan;
        M_FreshBiscuit.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
        GetComponent<SphereCollider>().material.bounciness = 0f;
        StartCoroutine(resetFluff());
    }

    public  void Defluff()
    {
        m_renderer.material.color = Color.white;
        M_FreshBiscuit.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        GetComponent<SphereCollider>().material.bounciness = 0.2f;
        PlayerManagerScript.M_Fluffed = false;
    }

    IEnumerator resetFluff()
    {
        yield return new WaitForSeconds(10);
        Defluff();
    }
    void OnCollisionStay(Collision a_hit)
    {
        if (!m_canBlur)
        {
            M_RenderScript.DisableBlur();
        }

        if (a_hit != null)
        {
            m_collisionStay = true;
        }

        if (a_hit.gameObject.CompareTag("Hazard") || a_hit.gameObject.CompareTag("Enemy"))
        {
            PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            m_playerManagerScript.M_takingDamage = true;
        }
    }

    public void OnCollisionExit(Collision a_hit)
    {
        m_collisionStay = false;
        M_playerManager.GetComponent<PlayerManagerScript>().M_takingDamage = false;
        StartCoroutine(CollisionCooldown());
    }


    private void OnCollisionEnter(Collision a_hit)
    {
        m_collisionEnter = true;

       

        if (m_collisionEnter & m_canShake)
        {
            StartCoroutine(ShakeCooldown());
        }
        if (!m_canBlur)
        {
            M_RenderScript.DisableBlur();
        }
    }

    public bool isGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(this.transform.position, Vector3.down, out hit, this.transform.lossyScale.y + 1f);
    }

     IEnumerator CollisionCooldown()
    {
        if (!m_collisionStay)
        {
            yield return new WaitForSeconds(0.2f);
            if(!m_collisionStay)
            {
                m_collisionEnter = false;
                m_canShake = true;
            }
        }
        
    }

    IEnumerator ShakeCooldown()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Landing");
        M_ImpactVFX.Play();
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, .1f);
        M_TrailScript.DeactivateTrail();
        m_canShake = false;
        yield return new WaitForSeconds(0.5f);
   

    }

    bool m_canBlur = false;
    IEnumerator BlurDisableCooldown()
    {
        m_canBlur = true;
        yield return new WaitForSeconds(3f);
        m_canBlur = false;
    }

    
    public void SetCameraOffset(Vector2 a_offset)
    {
       /* Vector2 OffsetIncrement = new Vector2 (0.5f, 0.5f);
        if (M_cameraOffset.x < a_offset.x)
        {
            M_cameraOffset += OffsetIncrement * Time.deltaTime*100;
        }
        else if (M_cameraOffset.x > a_offset.x)
        {
            M_cameraOffset -= OffsetIncrement * Time.deltaTime*100;
        }*/
       M_cameraOffset = a_offset;
 
    }

    public void SetDirection(Vector3 a_direction)
    {
       
        m_rigidbody.isKinematic = true;
        M_launchCamera.transform.rotation = Quaternion.LookRotation(a_direction);
        M_launchCamera.transform.position = this.transform.position + new Vector3(-M_launchCamera.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y * m_rotationMouseY, -M_launchCamera.transform.forward.z * M_cameraOffset.x);
        m_direction = -a_direction;
        this.transform.rotation = Quaternion.LookRotation(a_direction);
        m_rigidbody.isKinematic = false;
    }
    public void SetMouseRotation(float a_rotation)
    {
        a_rotation += 85.0f;
        m_rotationMouseY = Mathf.Abs(a_rotation - 120.0f) + 85;
    }

    public Vector2 GetMouseRotation()
    {
        return new Vector2(m_rotationMouseX/m_mouseSensitivityX, m_rotationMouseY);
    }
}
    
