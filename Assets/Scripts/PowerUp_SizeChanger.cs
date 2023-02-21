using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_SizeChanger : MonoBehaviour
{
    public enum Effect { SizeUp, SizeDown };
    public Effect M_myEffect;
    public PlayerManagerScript M_playerManager;

    // Start is called before the first frame update
    void Start()
    {
        M_playerManager = FindObjectOfType<PlayerManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WalkingPlayer" || other.gameObject.name == "ArmadilloBallPlayer")
        {
            switch(M_myEffect)
            {
                case Effect.SizeUp:
                    {
                        M_playerManager.Grow();
                        Destroy(this.gameObject);
                        return;
                    }
                case Effect.SizeDown:
                    {
                        M_playerManager.Shrink();
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
