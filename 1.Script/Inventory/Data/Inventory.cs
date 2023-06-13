using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    ItemInven,
    EquipInven,
}

[CreateAssetMenu(menuName = "Inventory", fileName = "playerInventory")]
public class Inventory : ScriptableObject
{
    public InventoryType InvenType;
    public InvenSlot[] slots = new InvenSlot[0];

    /// <summary>
    /// 아이템을 정상적으로 넣으면 0이상의 숫자를 리턴, 그외 -1 리턴
    /// </summary>
    public int AddItem(AddToItem item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].AddItem(item)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 슬롯에 있는 아이템을 변경
    /// </summary>
    public void ChangeItem(CopyItem item,int amount)
    {
        foreach(InvenSlot slot in slots)
        {
            if(slot.type == item.data.type)
            {
                slot.UpdateSlot(item, amount);
                break;
            }
        }
    }

    public int GetBullet(string name,int max)
    {
        int find = 0;
        foreach(InvenSlot slot in slots)
        {
            if (slot.item != null && slot.item.ItemName == name)
            {
                find = slot.amount <= max ? slot.amount : max;
                slot.amount -= find;
                slot.UpdateSlot(slot.slotItem, slot.amount);
                if (find >= max) break;
            }
        }

        return find;
    }
}
