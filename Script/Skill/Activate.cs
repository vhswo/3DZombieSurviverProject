using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivateObjType
{
    door,
    car,
    detectiveObj
}

public abstract class Activate : MonoBehaviour
{
    [SerializeField] ActivateObjType type;

    public string ObjState { get; private set; }
    public abstract void activate(Vector3 Pushdir);  // 수정대상

    public ActivateObjType GetActivateType()
    {
        return type;
    }

    public void SetObjState(string state)
    {
        ObjState = state;
    }

}
