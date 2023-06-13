using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum equipType
{
    MainWeapon,
    SubWeapon =2,
    Melee,
    Throw,
    Potion,
    None,
}

[Serializable]
public class InvenSlot
{
    public equipType type = equipType.None;

    public CopyItem slotItem = null;

    public int amount; // 슬롯에 있는 아이템 개수

    //아이템 데이터들은 여기서 가져간다
    public ItemData1 item
    {
        get { return slotItem == null ? null : slotItem.data; }
    } 

    [NonSerialized] public Image icon;
    [NonSerialized]public Text Weaponname;
    [NonSerialized]public Text AmmoType;
    [NonSerialized] public Text Amount;

    [NonSerialized] public Inventory parantInven;
    [NonSerialized] public GameObject InvenSlotPrefab;
    [NonSerialized]public Action<InvenSlot> Subscript;

    public InvenSlot() { }

    /// <summary>
    ///  빈 슬롯을 자동으로 찾아주고 아이템을 넣어준다
    /// </summary>
    public bool AddItem(AddToItem data)
    {
        CopyItem tmp = data.DeepCopy();
        
        //슬롯에 타입이 있고, 들어온것이 타입과 다르가면 리턴 
        if (type != equipType.None && type != tmp.data.type) return false;
        if(slotItem == null)
        {
            return UpdateSlot(tmp, tmp.HaveAmount);
        }

        else if(this.item.Countable && this.item.ItemName == tmp.data.ItemName)
        {
            return UpdateSlot(tmp, this.amount + tmp.HaveAmount);
        }

        return false;
    }

    public bool UpdateSlot(CopyItem data, int amount = 1)
    {
        this.amount = amount;

        slotItem = this.amount <= 0 ? null : data;

        Subscript?.Invoke(this);

        return true;
    }



}
