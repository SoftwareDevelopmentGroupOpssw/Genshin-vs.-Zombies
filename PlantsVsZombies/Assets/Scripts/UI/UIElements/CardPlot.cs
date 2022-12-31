using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPlot : MonoBehaviour
{
    [SerializeField]
    private Text energyCost;
    private Image image;
    public void SetEnergyCost(int value) => energyCost.text = value.ToString();
    public Sprite Sprite { get => image.sprite; set => image.sprite = value; }
    public Color SpriteColor { get => image.color; set => image.color = value; }

    void Awake()
    {
        image = GetComponent<Image>();
    }
}
