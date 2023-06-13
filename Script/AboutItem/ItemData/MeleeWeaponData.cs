using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MeleeWeaponData Data", menuName = "Scriptable Object/MeleeWeaponData Data", order = 1)]
public class MeleeWeaponData : WeaponData
{

    public override ItemObject CreateItem() { return new MeleeWeapon(this); }
}

