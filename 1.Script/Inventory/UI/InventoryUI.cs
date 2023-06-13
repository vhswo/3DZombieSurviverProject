using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inven;
    public Dictionary<GameObject, InvenSlot> SlotsUI = new();

    private void Awake()
    {
        for(int i = 0; i < inven.slots.Length; i++)
        {
            inven.slots[i].parantInven = inven;
            inven.slots[i].Subscript += Observer;
        }

        SetSlot();
    }

    public virtual void SetSlot()   { }

    public virtual void Observer(InvenSlot slot) { }
}