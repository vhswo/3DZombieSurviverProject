using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : NoneCountableItem
{
    [SerializeField] Transform firePos; // ÃÑ±¸ À§Ä¡

    public Transform _firePos { get{ return firePos; } }
}
