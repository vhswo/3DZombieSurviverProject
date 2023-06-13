using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUI : InventoryUI
{
    public GameObject[] SlotPrefab;
    public Transform CreateFolder;



    public override void SetSlot()
    {
        for (int i = 0; i < inven.slots.Length; i++)
        {
            inven.slots[i].InvenSlotPrefab = SlotPrefab[i];
            SlotsUI.Add(SlotPrefab[i], inven.slots[i]);
        }


    }
    private void Start()
    {
        for (int i = 0; i < CreateFolder.childCount; i++)
        {
            inven.slots[i].icon = CreateFolder.GetChild(i).Find("Icon")?.GetComponent<Image>();
            inven.slots[i].AmmoType = CreateFolder.GetChild(i).Find("AmmoType")?.GetComponent<Text>();
            inven.slots[i].Weaponname = CreateFolder.GetChild(i).Find("Weaponname")?.GetComponent<Text>();
            inven.slots[i].Amount = CreateFolder.GetChild(i).Find("Amount")?.GetComponent<Text>();

            inven.slots[i].icon.type = Image.Type.Simple;
            inven.slots[i].icon.preserveAspect = true;
        }
    }

    public override void Observer(InvenSlot slot)
    {
        if(slot.slotItem == null)
        {
            slot.icon.sprite = null;
             slot.icon.color = new Color(1, 1, 1, 0);
            slot.AmmoType.text = string.Empty;
            slot.Weaponname.text = string.Empty;
            slot.Amount.text = string.Empty;
            return;
        }

        if (slot.slotItem != null)
        {
            slot.icon.sprite = slot.item.ItemName != string.Empty ? slot.item.icon : null;
            slot.icon.color = slot.item.ItemName != string.Empty ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
            slot.AmmoType.text = slot.item.bulletName != string.Empty ? slot.item.bulletName : string.Empty;
            slot.Weaponname.text = slot.item.ItemName != string.Empty ? slot.item.ItemName : string.Empty;
            slot.Amount.text = slot.item.Countable && slot.amount > 0 ? slot.amount.ToString() : string.Empty;

        }
    }

}
