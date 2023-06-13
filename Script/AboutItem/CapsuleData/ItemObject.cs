using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ItemObject
{
    public ItemData Data { get; private set; }
    public ITEMTYPE type;
    //타입도 여기 넣는게 어떨지 아이템 주울때 다 설정하기
    public ItemObject(ItemData data) => Data = data;

}
