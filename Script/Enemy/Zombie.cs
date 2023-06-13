using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie
{
    public float HP;
    public ZombieData Data { get; private set; }
    public Zombie(ZombieData data)
    {
        Data = data;
        HP = data.iHP;
    }

}
