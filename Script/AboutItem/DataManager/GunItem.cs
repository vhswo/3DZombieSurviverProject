using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : NoneCountableItem
{
    [SerializeField] Transform firePos; // �ѱ� ��ġ

    public Transform _firePos { get{ return firePos; } }
}
