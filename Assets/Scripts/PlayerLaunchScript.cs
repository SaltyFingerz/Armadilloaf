using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections;
using System.Drawing;
using EZCameraShake;

public class PlayerLaunchScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private GameObject M_arrow;
    private GameObject M_arrowMaximum;
    public GameObject M_Tutorial;
    public GameObject M_TuteWorld;
    public GameObject M_launchCamera;
    public GameObject M_playerManager;
    public UnityEngine.UI.Image M_fillImage;
    public RenderingScript M_RenderScript;
    public LaunchTrailScript M_TrailScript;
    public Canvas M_canvas;

    float m_rotationMouseY, m_rotationMouseX;
    public float m_mouseSensitivityX;
    public float m_mouseSensitivityY;
    float m_currentScroll;
    bool m_collisionEnter = false;
    bool m_collisionStay = false;

    Vector3 m_direction;
    int m_launchingStage = 0;
    float m_launchingPower;
    bool m_canShake = false;

    public int M_maxPower;
    public float M_minimumDirectionY, M_maximumDirectionY;
    public float M_angleChangeRadians;
    public float M_floorAngleChangeRadians;

    [SerializeField] float m_powerSizeStep = 1.0f;          // Determines how big is the scale difference in the arrow when choosing launching power.
    [SerializeField] float m_baseLength = 10.0f;            // Minimum lenght of the arrow.
    [SerializeField] private float m_minimumSpeed = 0.4f;   // Speed minimum limit before the player changes to walking player.

    Vector2 M_cameraOffset = new Vector2(14.0f, 8.0f);
    float m_cameraRotationY;

    public void Start()
    {
        // get objects
        m_rigidbody = GetComponent<Rigidbody>();
        M_arrowMaximum = this.gameObject.transform.GetChild(0).gameObject;
        M_arrow = this.gameObject.transform.GetChild(1).gameObject;

        // assign starting values
        m_direction = new Vector3(0.0f, 0.0f, 1.0f);
        M_fillImage.fillAmount = 0.0f;
        M_arrowMaximum.transform.localScale = new Vector3(5.4f, 5.4f, m_baseLength + m_powerSizeStep * M_maxPower);
        m_cameraRotationY = -Mathf.Cos(24f);

    }


    // Handle rigidbody physics
    public void FixedUpdate()
    {
        // if paused or free flying, don't update
        if (Time.timeScale < 0.1f || M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }

        if (isGrounded() && m_rigidbody.velocity.magnitude < m_minimumSpeed)
        {
            m_launchingStage = 0;
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

        Vector3 l_velocity = m_rigidbody.velocity;
        float l_angleChange = M_angleChangeRadians;
        float l_translationChange = 0.15f;
        

        if (isGrounded())
        {
            l_angleChange = M_floorAngleChangeRadians;
            l_translationChange = 1.3f;
        }

        Vector2 multiplier = new Vector2(l_playerHorizontalInput, l_playerVerticalInput);
        multiplier.Normalize();

        multiplier = multiplier * l_translationChange * Time.fixedDeltaTime;
        multiplier = new Vector2(1f + multiplier.x, 1f + multiplier.y);

        // Change direction based on input
        l_velocity = Vector3.RotateTowards(l_velocity, M_launchCamera.transform.right * l_playerHorizontalInput, l_angleChange * Time.fixedDeltaTime, 0.0f) * Mathf.Abs(multiplier.x);
        l_velocity = Vector3.RotateTowards(l_velocity, M_launchCamera.transform.forward * l_playerVerticalInput, l_angleChange * Time.fixedDeltaTime, 0.0f) * Mathf.Abs(multiplier.y);

        float l_maxSpeed = 80.0f;
        if (m_rigidbody.velocity.magnitude > l_maxSpeed)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * l_maxSpeed;
        }

        // normalize and apply changed direction
        m_rigidbody.velocity = l_velocity;

        // Calculate camera rotation
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;
        
        //rotate the camera and interpolate
        Quaternion l_rotation = Quaternion.LookRotation(l_desiredRotation);
        l_rotation = Quaternion.Lerp(M_launchCamera.transform.rotation, l_rotation, Time.fixedDeltaTime * 10.0f);

        //camera transform change
        M_launchCamera.transform.position = this.transform.position + new Vector3(-M_launchCamera.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y, -M_launchCamera.transform.forward.z * M_cameraOffset.x);
        M_launchCamera.transform.rotation = l_rotation;

    }

    void HandleLaunchInput()
    {
        //override player rotation
        if (Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.Space))
        {       
            LaunchingStart();
            return;
        }

        //m_rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        m_launchingPower += (m_currentScroll + Input.mouseScrollDelta.y);
        M_fillImage.fillAmount = m_launchingPower / M_maxPower;

        // clamp the power of the lauch between 0 and the limit
        if (m_launchingPower < 1)
        {
            m_launchingPower = 1;
        }
        else if (m_launchingPower > M_maxPower)
        {
            m_launchingPower = M_maxPower;
        }
        M_arrow.transform.localScale = new Vector3(5, 5, m_baseLength + m_powerSizeStep * m_launchingPower);
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

        m_currentScroll = Input.mouseScrollDelta.y;
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
        m_rigidbody.velocity = new Vector3(m_direction.x * m_launchingPower, m_direction.y * m_launchingPower, m_direction.z * m_launchingPower);
        //m_rigidbody.AddForce(new Vector3(m_direction.x * m_launchingPower * 100, m_direction.y * m_launchingPower * 100, m_direction.z * m_launchingPower * 100));
        m_rigidbody.freezeRotation = false;
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Launching");
        M_TrailScript.ActivateTrail();
        StartCoroutine(BlurDisableCooldown());
        if (m_canBlur)
        {
            M_RenderScript.EnableBlur();
        }
    }

    private void DirectionInput()
    {
        Vector3 l_desiredRotation = GetDesiredRotationFromMouseInput();
        m_direction = l_desiredRotation;

        //rotate the player after getting the updated direction, interpolate
        Quaternion l_rotation = Quaternion.LookRotation(new Vector3(l_desiredRotation.x, 0.0f, l_desiredRotation.z));
        m_rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, l_rotation, Time.fixedDeltaTime * 10.0f));

        // Calculate and clamp Y value
        //Vector3 l_axis = Vector3.Cross(l_result, Vector3.up);
        //if (l_axis == Vector3.zero) l_axis = Vector3.right;
        //Vector3 l_direction = Quaternion.AngleAxis(-m_rotationMouseY, l_axis) * l_result;
        //l_direction.Normalize();

        Quaternion yRotation = Quaternion.LookRotation(l_desiredRotation);

        //camera transform change
        M_launchCamera.transform.rotation = Quaternion.Lerp(M_launchCamera.transform.rotation, yRotation, Time.fixedDeltaTime * 10.0f);
        M_launchCamera.transform.position = this.transform.position + new Vector3(-M_launchCamera.transform.forward.x * M_cameraOffset.x, M_cameraOffset.y, -M_launchCamera.transform.forward.z * M_cameraOffset.x);
    }

    Vector3 GetDesiredRotationFromMouseInput()
    {
        // Mouse is moved, calculate camera rotation from the mouse position difference between frames
        float l_mouseX = -Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_mouseSensitivityX;
        //float l_mouseY = -Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_mouseSensitivityY;

        // Rotation using 2D vector rotation by angle formula
        Vector3 l_rotation;
        l_rotation.x = m_direction.x * Mathf.Cos(l_mouseX) - m_direction.z * Mathf.Sin(l_mouseX);
        l_rotation.y = m_cameraRotationY;
        l_rotation.z = m_direction.x * Mathf.Sin(l_mouseX) + m_direction.z * Mathf.Cos(l_mouseX);

        return l_rotation;
    }

    public void SetValues(float a_size, float a_mass)
    {
        GetComponent<Animator>().enabled = false;

        transform.localScale = new Vector3(a_size, a_size, a_size);
        m_rigidbody.mass = a_mass;
        GetComponent<Animator>().enabled = true;

    }
    private void OnTriggerEnter(Collider a_hit)
    {
        if ( a_hit.gameObject.CompareTag("Collectible"))
        {
            print("collectible +1");
            a_hit.gameObject.GetComponent<HoverScript>().StopParticles();
        }

        if (a_hit.gameObject.name.Contains("FirstLaunchZone"))
        {
            M_Tutorial.transform.GetChild(4).gameObject.SetActive(true);
            M_TuteWorld.transform.GetChild(0).gameObject.SetActive(false);

        }
    }

    private void OnTriggerStay(Collider a_hit)
    {
        if (a_hit.gameObject.CompareTag("Hazard") || a_hit.gameObject.CompareTag("Enemy"))
        {
            PlayerManagerScript m_playerManagerScript = M_playerManager.GetComponent<PlayerManagerScript>();
            m_playerManagerScript.M_takingDamage = true;
        }
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
        M_cameraOffset = a_offset;
    }

    public void SetDirection(Vector3 a_direction)
    {
        a_direction.y = 0.0f;
        m_direction = a_direction;
    }
}
    
