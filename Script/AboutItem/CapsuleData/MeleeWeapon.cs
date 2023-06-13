using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : ItemObject
{
    public Transform firePos;
    public MeleeWeaponData meleeWeaponData { get; private set; }
    public MeleeWeapon(MeleeWeaponData data) : base(data) { meleeWeaponData = data; }
    
    public void Init(Transform Pos)
    {
      firePos = Pos;
    }

    public Vector3 pos()
    {
        return firePos.position;
    }
}
