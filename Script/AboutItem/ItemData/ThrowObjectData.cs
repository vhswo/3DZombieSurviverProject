using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



[CreateAssetMenu(fileName = "ThrowObjectData", menuName = "Scriptable Object/ThrowObjectData", order = 4)]
public class ThrowObjectData : CountableData
{
    [SerializeField] float Damage;
    [SerializeField] float BombRange;
    [SerializeField] float BombMaxTimer;
    [SerializeField] THROWOBJECT ThrowType;

    public float m_BombMaxTimer => BombMaxTimer;
    public float m_Damage => Damage;
    public override ItemObject CreateItem() { return new ThrowObject(this); }
}