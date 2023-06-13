using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponItem : Item
{
    [SerializeField] Transform HitRange;

    public Transform _HitRange => HitRange;

}
