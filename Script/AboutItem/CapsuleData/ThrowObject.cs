using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum THROWOBJECT
{
    THROWOBJECT_GREDER,
    THROWOBJECT_ALRAM,
    THROWOBJECT_LIGHT
}

public class ThrowObject : Countable
{
    public ThrowObjectData m_data { get; private set; }

    public ThrowObject(ThrowObjectData data) : base(data)
    {
        m_data = data;
    }
}
