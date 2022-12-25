using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ¹Ø¿¨±³¾°
/// </summary>
public class LevelBackground : BasePanel
{
    private Image image;
    public Sprite Sprite
    {
        get
        {
            if(image == null)
                image = GetComponent<Image>();
            return  image.sprite;
        }
        set 
        {
            if (image == null)
                image = GetComponent<Image>();
            image.sprite = value;
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
