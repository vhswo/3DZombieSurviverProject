using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Countable: ItemObject
{
    public CountableData cData { get; private set; }
    public int Amount { get; private set; } = 0;
    public Countable(CountableData data) : base(data)
    {
        cData = data;
    }

    /// <summary>
    /// 맥스값을 초과하면 양수를 반환
    /// </summary>
    public int SetAmount(int amount)
    {
        int sum = Amount + amount;
        int overValue = sum - cData.maxCount;
        Amount = overValue > 0 ? cData.maxCount : sum;

        return overValue;
    }

    public void Remain(int amount)
    {
        Amount = amount;
    }

    public int Init(int amount)
    {
        Amount = amount;

        return Amount;
    }

    // 삭제할것
    public void ChangeAmount(int amount)
    {
        Amount += amount;
    }

}
