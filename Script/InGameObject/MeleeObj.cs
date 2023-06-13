using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeObj : InGameObjects
{
    public float m_damage { get; private set; }

    public int MaxHit;

    public void SetMelee(float damage)
    {
        m_damage = damage;
    }

    public void ResetHit()
    {
        MaxHit = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (MaxHit >= 1) return;
        if(other.GetComponent<IDamageable>() is IDamageable target )
        {
            MaxHit++;
            target.GetDamage(m_damage);
        }
    }
}
