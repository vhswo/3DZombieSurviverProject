using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zombie Data", menuName = "Scriptable Object/Zombie Data", order = 3)]
public class ZombieData : ScriptableObject
{
    public int iNumber;
    public int iHP;
    public int iDamage;
    public float AttackRange;
    public string sName;

    public Zombie CreateZombie()
    {
        return new Zombie(this);
    }
}
