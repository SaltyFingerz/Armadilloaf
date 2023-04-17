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
   private MeshRenderer m_Renderer;
    // Start is called before the first frame update
    void Start()
    {
        M_playerManager = FindObjectOfType<PlayerManagerScript>();
       m_JamAudio = GetComponent<AudioSource>();
        m_Renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator eatJam()
    {
        m_Renderer.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
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

                       StartCoroutine(eatJam());
                        return;
                    }
                case Effect.SizeDown:
                    {
                        M_playerManager.Shrink();

                        StartCoroutine(eatJam());
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
