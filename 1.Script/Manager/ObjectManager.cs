using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling<T>
{
    Queue<T> item = new();

    public int GetCount()
    {
        return item.Count;
    }

    public void Add(T item)
    {
        this.item.Enqueue(item);
    }

    public T Use()
    {
        if (item.Count <= 0) return default;

        return item.Dequeue();
    }

    public T ReferenceT()
    {
        return item.Peek();
    }
}

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager m_instance;
    public static ObjectManager instance => m_instance;

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
            if(transform.root != null)
                DontDestroyOnLoad(transform.root.gameObject);
            else
                DontDestroyOnLoad(gameObject);

        }
    }


    Dictionary<ObjectType, ObjectPooling<GameObject>> Obj = new();
    Dictionary<ObjectType, Transform> parants = new();

    public void CreateObj(ObjectType type)
    {
        string path = Application.dataPath;
        string deepPath = "/9.Resources/Resources/Objects/";
        string fullPath = path + deepPath;

        if (!parants.ContainsKey(type)) parants.Add(type, CreateSlot());

        GameObject obj = Instantiate(Resources.Load("Objects/" + type.ToString()) as GameObject, parants[type]);

        if(!Obj.ContainsKey(type)) Obj.Add(type, new());

        Obj[type].Add(obj);

        obj.SetActive(false);

        Transform CreateSlot()
        {
            GameObject parant = new GameObject(type.ToString());
            parant.transform.SetParent(transform);
            parant.transform.localPosition = Vector3.zero;

            return parant.transform;
        }
    }


    public GameObject GetObj(ObjectType type)
    {
        if(!Obj.ContainsKey(type) && Obj[type].GetCount() <= 0) CreateObj(type);

        return Obj[type].Use();
    }

    public GameObject AppearObjAfterSeconds(Vector3 pos, Vector3 ro,ObjectType type,float second)
    {
        if (!Obj.ContainsKey(type) || Obj[type].GetCount() <= 0) CreateObj(type);

        GameObject Object = Obj[type].Use();
        Object.SetActive(true);

        Object.transform.position = pos;
        Object.transform.rotation = Quaternion.FromToRotation(Vector3.up, ro);

        StartCoroutine(StartSecond(type, Object, second));

        return Object;
    }

    IEnumerator StartSecond(ObjectType type,GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);

        Obj[type].Add(obj);
        obj.SetActive(false);
    }

}
