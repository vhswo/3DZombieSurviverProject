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
    //Ÿ�Ե� ���� �ִ°� ��� ������ �ֿﶧ �� �����ϱ�
    public ItemObject(ItemData data) => Data = data;

}
