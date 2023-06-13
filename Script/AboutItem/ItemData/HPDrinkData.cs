using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "HPDrink Data", menuName = "Scriptable Object/HPDrink Data", order = 1)]
public class HPDrinkData : CountableData
{
    [SerializeField] float Healing;
    public float m_healing => Healing;
    public override ItemObject CreateItem() { return new HPDrink(this); }
}