using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum WeaponType
{
    Rifle,
    Pistol,
    Melee,
    Throw,
    Food,
}


public class PlayerInven : MonoBehaviour
{
    public Inventory itemInven;
    public Inventory equipInven;
    public Transform HaveWeapon;

    //액션관련
    public Action<InvenSlot> SubScriptToAction;

    //퀵슬롯 관련
    public int EquipSelect = -1;
    public Action<InvenSlot, int> SubScript;
    public Image QuickImage;
    public Text QuickNumber;

    //애니메이션 관련
    public Animator ani;
    string WeaponType = "WeaponType";
    string WeaponNum = "WeaponNum";
    string Cover = "Cover";

    //총알 임시박스
    Dictionary<string, int> Bullets = new();


    private void Start()
    {
        ani = GetComponent<Animator>();

    }

    public void GetItem(AddToItem item)
    {
        int Check = -1;

        if (item.GetData.IsEquip)
        {
            //빈 슬롯 찾거나 같은 아이템 개수 더하기
            //현재 밖에 있는 오브젝트와 연동되니 파인드로 현재 HaveWaepon으로 찾아서 슬롯에 넝기
            Check = equipInven.AddItem(item);
            if(Check != -1)
            {
                if(EquipSelect == - 1) EquipSelect = Check;

                UpdateEquipment();
                return;
            }

            //빈스롯을 찾지못하고, 인벤 아이템이 아니다
            if(Check == -1 && !item.GetData.IsInven)
            {
                InvenSlot slot = equipInven.slots[EquipSelect].type == item.GetData.type ? equipInven.slots[EquipSelect] : equipInven.slots[(int)item.GetData.type];
                slot.UpdateSlot(item.DeepCopy(), item.HaveAmount); // 버리고 넣었고

                if(slot == equipInven.slots[EquipSelect])
                {
                    HaveWeapon.Find(item.GetData.ItemName).gameObject.SetActive(false);
                    SubScriptToAction?.Invoke(null);
                    //무기 체인지 업데이트
                    UpdateEquipment();
                }

                return;
            }
        }

        //체크가 -1 이면 인벤토리도 체크한다
        if (Check != -1 || !item.GetData.IsInven) return;

        Check = itemInven.AddItem(item);

        if (Check == -1) return; //인벤토리에 자리가 없다


        //UpdateInventory();

        if (item.GetData.ItemName == "5.56mm" || item.GetData.ItemName == "9mm")
        {
            if (!Bullets.ContainsKey(item.GetData.ItemName)) Bullets.Add(item.GetData.ItemName, 0);
            Bullets[item.GetData.ItemName] += item.HaveAmount;

            if(EquipSelect != -1 && (equipInven.slots[EquipSelect].type == equipType.MainWeapon || equipInven.slots[EquipSelect].type == equipType.SubWeapon))
                SubScript?.Invoke(equipInven.slots[EquipSelect], GetBulletNum(equipInven.slots[EquipSelect].item.bulletName));
        }

    }

    public int GetBullet(string name,int max)
    {
        int allBullet = Bullets.ContainsKey(name) ? (Bullets[name] < max ? Bullets[name] : max) : 0;

        if (allBullet != 0) Bullets[name] -= allBullet;

        SubScript?.Invoke(equipInven.slots[EquipSelect], GetBulletNum(equipInven.slots[EquipSelect].item.bulletName));


        return itemInven.GetBullet(name, allBullet);
    }


    public int GetBulletNum(string name)
    {
        return Bullets.ContainsKey(name) ? Bullets[name] : 0;
    }

    public void ChangeWeapon(int QuickNum)
    {
        if (QuickNum != -1 && equipInven.slots[QuickNum].slotItem == null) return;
        
        if(EquipSelect != -1)
        {
            HaveWeapon.Find(equipInven.slots[EquipSelect].item.ItemName).gameObject.SetActive(false);
            if (equipInven.slots[EquipSelect].amount == 0) equipInven.ChangeItem(equipInven.slots[EquipSelect].slotItem, 0);
        }
        EquipSelect = EquipSelect == QuickNum ? -1 : QuickNum;

        UpdateEquipment();

    }

    public void UpdateEquipment()
    {
        InvenSlot SelectSlot = EquipSelect != -1 ? equipInven.slots[EquipSelect] : null;

        float type = EquipSelect != -1 ? (float)SelectSlot.item.WeaponType : -1.0f;
        float num = EquipSelect != -1 ? SelectSlot.item.WeaponNum : -1.0f;
        bool cover = EquipSelect != -1 && (SelectSlot.type == equipType.MainWeapon || SelectSlot.type == equipType.SubWeapon) ? true : false;

        ani.SetFloat(WeaponType, type);
        ani.SetFloat(WeaponNum, num);
        ani.SetBool(Cover, cover);

        if (EquipSelect != -1) HaveWeapon.Find(SelectSlot.item.ItemName).gameObject.SetActive(true);

        SubScriptToAction?.Invoke(EquipSelect != -1 ? equipInven.slots[EquipSelect] : null); //액션비헤비어 현재 장착장비

        SubScript?.Invoke(EquipSelect != -1 ? SelectSlot : null, EquipSelect != -1 ? GetBulletNum(SelectSlot.item.bulletName) : 0); //퀵
    }


    //체인지 웨펀이 아닌 아이템 사용 후의 상태를 하나 만들기 

    //6개를 가지고 있어도 하나만 사용하면 사라짐
}

