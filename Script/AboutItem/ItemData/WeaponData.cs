using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class WeaponData : EquipData
{
    [SerializeField] float fdamage;
    [SerializeField] float fWeaponSpeed; //무기 속도
    [SerializeField] float fEXP; //숙련도 경험치
    public float m_damage => fdamage;
    public float m_weaponSpeed => fWeaponSpeed;
    public float m_exp => fEXP;
}