using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieMuncherScript : MonoBehaviour
{
    public PlayerManagerScript M_pManager;
    public Sprite M_fiveHealth;
    public Sprite M_fourHealth;
    public Sprite M_threeHealth;
    public Sprite M_twoHealth;
    public Sprite M_oneHealth;
    private Image m_spImage;
    // Start is called before the first frame update
    void Start()
    {
        m_spImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (M_pManager.M_lives == 5)
        {
            m_spImage.sprite = M_fiveHealth;
        }
        else if (M_pManager.M_lives == 4)
        {
            m_spImage.sprite = M_fourHealth;
        }
        else if (M_pManager.M_lives == 3)
        {
            m_spImage.sprite = M_threeHealth;
        }
        else if (M_pManager.M_lives == 2)
        {
            m_spImage.sprite = M_twoHealth;
        }
        else if (M_pManager.M_lives == 1)
        {
            m_spImage.sprite = M_oneHealth;
        }
    }
}
