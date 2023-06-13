using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract  class Weapon : Equip
{
    public WeaponData weaponData { get; private set; }

    public Weapon(WeaponData data) : base(data)
    {
        weaponData = data;
    }
    protected abstract Weapon Clone();
}
