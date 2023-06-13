using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountableItem : Item
{

    [SerializeField] int Amount;
    public int _amount => Amount;

    public void CheckAmount(int amount)
    {
        Amount = amount;
        if (Amount <= 0) gameObject.SetActive(false);
    }

}
