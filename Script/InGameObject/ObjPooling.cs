using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjPooling<T>  where T : MonoBehaviour
{
    public Queue<T> m_PoolingObjs { get; private set; } = new();


    public void AddObj(T prefab)
    {
        m_PoolingObjs.Enqueue(prefab);
    }

    public T GetObj()
    {
        if (0 >= m_PoolingObjs.Count) return null;

        return m_PoolingObjs.Dequeue();
    }
}

