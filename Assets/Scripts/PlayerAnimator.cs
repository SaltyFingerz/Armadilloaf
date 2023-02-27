using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PrototypePlayerMovement playerMovement;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PrototypePlayerMovement>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("backwards", true);
            playerAnimator.SetBool("left", false);
            playerAnimator.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("right", true);
            playerAnimator.SetBool("left", false);
            playerAnimator.SetBool("backwards", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("left", true);
            playerAnimator.SetBool("right", false);
            playerAnimator.SetBool("backwards", false);
        }
        else
        {
            playerAnimator.SetBool("left", false);
            playerAnimator.SetBool("right", false);
            playerAnimator.SetBool("backwards", false);
        }
    }
}
