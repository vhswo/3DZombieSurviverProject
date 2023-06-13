using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ItemData,firePos,bulletsNum,HaveAmount
/// </summary>
public class CopyItem
{
    public ItemData1 data;
    public Transform firePos;
    public int Bullets;
    public int HaveAmount;
}

public class AddToItem : MonoBehaviour
{
    [SerializeField] ItemData1 data;
    public Transform firePos;
    public int Bullets = 0;
    public int HaveAmount = 1;

    public ItemData1 GetData => data;

    public CopyItem DeepCopy()
    {
        CopyItem tmp = new();
        tmp.data = data;
        tmp.Bullets = Bullets;
        firePos = null;
        tmp.HaveAmount = HaveAmount;

        return tmp;
    }

}
