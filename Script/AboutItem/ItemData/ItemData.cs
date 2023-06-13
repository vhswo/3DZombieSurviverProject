using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ITEMTYPE
{
    MAIN,
    SUBMAIN,
    SUB,
    MELEE,
    THROW,
    POTION,
    PARTS,
    AMMO,
    NONE
}


public abstract class ItemData : ScriptableObject
{
    public int id;
    public string ItemName;
    public ITEMTYPE type;
    public equipType equipType;
    public string ItemUIName;
    public Sprite Image;

    public abstract ItemObject CreateItem();
    //아이템 아이콘
    //아이템 프리팹
}