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
                  

            }
                else if (Input.GetKey(KeyCode.D))
                {
                    playerAnimator.SetBool("backwards", true);
                    playerAnimator.SetBool("left", false);
                    playerAnimator.SetBool("right", true);
                    playerAnimator.SetBool("forwards", false);
           
                //render shader to texture and flip it here
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
               
                playerAnimator.SetBool("left", true);
                    playerAnimator.SetBool("right", false);
                    playerAnimator.SetBool("backwards", false);
                    playerAnimator.SetBool("forwards", true);
                }
                else if (Input.GetKey(KeyCode.D))
                {
            
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
