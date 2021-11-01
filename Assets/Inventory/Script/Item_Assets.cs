using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Assets : MonoBehaviour
{
    public static Item_Assets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite HealthPosionSprite;
    public Sprite CoinSprite;
    public Sprite Medkit;

}
