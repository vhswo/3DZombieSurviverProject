using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Data", menuName = "Scriptable Object/Ammo Data", order = 1)]
public class AmmoData : CountableData
{
    public override ItemObject CreateItem() { return new Ammo(this); }
}