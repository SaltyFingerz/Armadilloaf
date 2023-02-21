using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_SizeChanger : MonoBehaviour
{
    public enum Effect { SizeUp, SizeDown };
    public Effect myEffect;
    public PrototypePlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindObjectOfType<PrototypePlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SizePlayerUp()
    {

        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WalkingPlayer" || other.gameObject.name == "ArmadilloBallPlayer")
        {
            switch(myEffect)
            {
                case Effect.SizeUp:
                    {
                        playerMovement.Grow();
                        Destroy(this.gameObject);
                        return;
                    }
                case Effect.SizeDown:
                    {
                        playerMovement.Shrink();
                        Destroy(this.gameObject);
                        return;
                    }
                default:
                    {
                        Debug.Log("Error! Did you forget to change the object's effect?");
                        return;
                    }
            }
        }
    }
}
