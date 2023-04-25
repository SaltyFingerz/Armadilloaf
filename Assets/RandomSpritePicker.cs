using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpritePicker : MonoBehaviour
{
    public Sprite[] M_sprites = new Sprite[4];
    Image m_image;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
    }

    public void PickRandomSprite()
    {
        int random = Random.Range(0, 4);
        m_image.sprite = M_sprites[random];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
