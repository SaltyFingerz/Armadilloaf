using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpritePicker : MonoBehaviour
{
    public Sprite[] M_sprites = new Sprite[4];
    Image M_image;

    // Start is called before the first frame update
    void Start()
    {
        M_image = GetComponent<Image>();
    }

    public void SetRandomSprite()
    {
        int l_randomNumber = Random.Range(0, 4);
        M_image.sprite = M_sprites[l_randomNumber];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
