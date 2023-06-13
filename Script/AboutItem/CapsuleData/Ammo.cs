using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Countable
{
    public AmmoData AmmoData { get; private set; }
    public Ammo(AmmoData data) : base(data)
    {
        AmmoData = data;
    }
}
