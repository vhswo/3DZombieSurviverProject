using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickUI : MonoBehaviour
{
    public ActionBehaviour action;
    public PlayerInven inventory;
    public GameObject WeaponQuickSlot;

    private void Start()
    {
        inventory.SubScript += UpdateQuick;
        action.SubScriptQuick += UpdateQuickOnlyNum;

        inventory.QuickImage = WeaponQuickSlot.transform.Find("QuickImage").GetComponent<Image>();
        inventory.QuickNumber = WeaponQuickSlot.transform.Find("QuickNumber").GetComponent<Text>();
    }

    public void UpdateQuickOnlyNum(InvenSlot slot,int bullet)
    {
        UpdateQuickNum(slot,bullet);
    }

    public void UpdateQuick(InvenSlot slot = null,int bullets = 0)
    {
        inventory.QuickImage.sprite = slot != null ? slot.item.icon : null;
        inventory.QuickImage.color = slot != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        UpdateQuickNum(slot,bullets);

    }

    public void UpdateQuickNum(InvenSlot slot = null, int bullets = 0)
    {
        if (slot == null)
        {
            inventory.QuickNumber.text = string.Empty;
        }
        else
        {
            switch (slot.item.type)
            {
                case equipType.MainWeapon:
                case equipType.SubWeapon:
                    inventory.QuickNumber.text = slot.slotItem.Bullets.ToString() + " / " + bullets.ToString();
                    break;
                case equipType.Melee:
                    inventory.QuickNumber.text = string.Empty;
                    break;
                case equipType.Throw:
                case equipType.Potion:
                    inventory.QuickNumber.text = slot.amount.ToString();
                    break;
                default:
                    break;
            }
        }
    }

}
