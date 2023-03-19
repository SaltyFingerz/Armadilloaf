using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class RenderingScript : MonoBehaviour
{
    [SerializeField] private Volume M_PPVol;
    public GameObject M_Ball;
    public GameObject M_Walker;
    MotionBlur m_motionBlur;
    // Start is called before the first frame update
    void Start()
    {
        M_PPVol = gameObject.GetComponent<Volume>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
       
           
    }

    public void EnableBlur()
    {
        if (M_Ball.activeSelf && !M_Walker.activeSelf) //set the motion blur while ball
        {
            if (M_PPVol.profile.TryGet<MotionBlur>(out m_motionBlur))
            {
                m_motionBlur.intensity.value = 0.6f;
                print("blur enabled");
            }

        }
    }

    public void DisableBlur()
    {
       
            if (M_PPVol.profile.TryGet<MotionBlur>(out m_motionBlur))
            {
                m_motionBlur.intensity.value = 0.0f;
            print("blur disabled");
            }
       
    }
}
