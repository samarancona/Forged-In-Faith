using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public enum Itemtype {
        coin,
        medkit,
        HealthPosion,

    }

    public Itemtype ItemType;
    public int Amount;
}
