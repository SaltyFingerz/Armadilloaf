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
    Vignette m_Vignette;
    ColorAdjustments m_ColorAd;
    DepthOfField m_DoF;
    public float M_blurAmount = 0f;
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
                m_motionBlur.intensity.value = M_blurAmount;
            }

        }
    }

    public void DisableBlur()
    {
       
            if (M_PPVol.profile.TryGet<MotionBlur>(out m_motionBlur))
            {
                m_motionBlur.intensity.value = 0.0f;
            }
       
    }

    public void IncreaseVignette()
    {
       if( M_PPVol.profile.TryGet<Vignette>(out m_Vignette))
       {
            m_Vignette.intensity.value += 0.035f * Time.deltaTime;
       }
    }

    public void ResetVignette()
    {
        if (M_PPVol.profile.TryGet<Vignette>(out m_Vignette))
        {
            m_Vignette.intensity.value = 0.15f;
        }
    }

    public void DecreaseSaturation()
    {
        if (M_PPVol.profile.TryGet<ColorAdjustments>(out m_ColorAd))
        {
            m_ColorAd.saturation.value -= 5f * Time.deltaTime;
        }
    }
    public void RestoreSaturation()
    {
        if (M_PPVol.profile.TryGet<ColorAdjustments>(out m_ColorAd))
        {
            m_ColorAd.saturation.value = 26;
        }

    }

    public void BlurBackground()
    {
        if (M_PPVol.profile.TryGet<DepthOfField>(out m_DoF))
        {
           
                m_DoF.focalLength.value = 120;
           
           
               
        }
    }

    public void UnBlurBackground()
    {
        if (M_PPVol.profile.TryGet<DepthOfField>(out m_DoF))
        {
            m_DoF.focalLength.value = 53;
        }
    }

}
