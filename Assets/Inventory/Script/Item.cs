using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public enum Itemtype {
        coin,
        medkit,
        HealthPosion,

    }

    public Itemtype itemType;
    public int Amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case Itemtype.coin:                 return Item_Assets.Instance.CoinSprite;
            case Itemtype.HealthPosion:         return Item_Assets.Instance.HealthPosionSprite;
            case Itemtype.medkit:               return Item_Assets.Instance.Medkit;


        }
    }
}
