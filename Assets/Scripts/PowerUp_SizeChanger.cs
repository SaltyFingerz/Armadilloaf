using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_SizeChanger : MonoBehaviour
{
    public enum Effect { SizeUp, SizeDown };
    public Effect M_myEffect;
    public PlayerManagerScript M_playerManager;
    public PrototypePlayerMovement M_prototypePlayerMovement;
    private AudioSource m_JamAudio;
    public enum Property { None, Jelly, Honey };
    public Property M_myProperty = Property.None;
   
    // Start is called before the first frame update
    void Start()
    {
        M_playerManager = FindObjectOfType<PlayerManagerScript>();
       m_JamAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WalkingPlayer" || other.gameObject.name == "ArmadilloBallPlayer")
        {
            m_JamAudio.Play();
            switch(M_myEffect)
            {
                case Effect.SizeUp:
                    {
                        M_playerManager.Grow();

                        if (M_myProperty == Property.Jelly)
                            M_playerManager.Jellify();
                        else if (M_myProperty == Property.Honey)
                        {
                            M_playerManager.Honify();
                        }
                        else if (M_myProperty == Property.None)
                        {
                            
                        }

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
