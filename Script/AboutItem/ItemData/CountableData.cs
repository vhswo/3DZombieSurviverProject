using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CountableData : ItemData
{
    [SerializeField] int m_MaxCount;
    public int maxCount { get { return m_MaxCount; } }
}