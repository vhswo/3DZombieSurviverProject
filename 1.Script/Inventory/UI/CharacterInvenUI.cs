using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInvenUI : InventoryUI
{
    public GameObject SlotPrefab;
    public Transform CreateFolder;

    public override void SetSlot()
    {
        SlotPrefab.SetActive(true);

        for(int i = 0; i < inven.slots.Length; i++)
        {
            GameObject obj = Instantiate(SlotPrefab, CreateFolder);

            inven.slots[i].InvenSlotPrefab = obj;
            SlotsUI.Add(obj, inven.slots[i]);
            obj.SetActive(false);
        }
        SlotPrefab.SetActive(false);

    }
    private void Start()
    {
        for (int i = 0; i < CreateFolder.childCount; i++)
        {
            inven.slots[i].icon = CreateFolder.GetChild(i).Find("Icon")?.GetComponent<Image>();
            inven.slots[i].Weaponname = CreateFolder.GetChild(i).Find("Weaponname")?.GetComponent<Text>();
            inven.slots[i].Amount = CreateFolder.GetChild(i).Find("Amount")?.GetComponent<Text>();

            inven.slots[i].icon.type = Image.Type.Simple;
            inven.slots[i].icon.preserveAspect = true;
        }
    }

    public override void Observer(InvenSlot slot)
    {
            slot.icon.sprite = slot.slotItem != null ? slot.item.icon : null;
            slot.icon.color = slot.slotItem != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
            slot.Weaponname.text = slot.slotItem != null ? slot.item.ItemName : string.Empty;
            slot.Amount.text = slot.slotItem != null && slot.item.Countable ? slot.amount.ToString() : string.Empty;
            slot.InvenSlotPrefab.SetActive(slot.slotItem != null ? true : false);
        
    }

}
