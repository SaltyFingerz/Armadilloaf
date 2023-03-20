using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PrototypePlayerMovement playerMovement;
    public Animator playerAnimator;
    public GameObject M_playerManager;
    private SpriteRenderer m_spriteR;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PrototypePlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        m_spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (M_playerManager.GetComponent<PlayerManagerScript>().M_isFreeFlying)
        {
            return;
        }
        HandleInput();
    }
    public void HandleInput()
    {
            if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("forwards", false);
                    m_spriteR.flipX = true;

            }
                else if (Input.GetKey(KeyCode.D))
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("forwards", false);
                m_spriteR.flipX = false;
            }
                else
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("forwards", false);
                
            }
            }
            else if (Input.GetKey(KeyCode.D))
            {
            m_spriteR.flipX = false;
            if (Input.GetKey(KeyCode.S))
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("forwards", false);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
                else
                {
                    playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", false);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
            m_spriteR.flipX = true;
            if (Input.GetKey(KeyCode.S))
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("forwards", false);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
                else
                {
                    playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", false);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.A))
                {
                m_spriteR.flipX = true;
                playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                m_spriteR.flipX = false;
                playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
                else
                {
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
            }
            else
            {
                playerAnimator.SetBool("left", false);
                playerAnimator.SetBool("right", false);
                playerAnimator.SetBool("backwards", false);
                playerAnimator.SetBool("forwards", false);
            }
        }
}
