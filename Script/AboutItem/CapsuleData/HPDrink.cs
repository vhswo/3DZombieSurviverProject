using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDrink : Countable
{
    public HPDrinkData HPDrinkData { get; private set; }
    public HPDrink(HPDrinkData data) : base(data)
    {
        HPDrinkData = data;
    }
}
