using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] string characterName;
    [SerializeField] float HPMAX;
    [SerializeField] int speed;
    [SerializeField] int level;

    public string _characterName => characterName;
    public float _HPMAX => HPMAX;
    public int _speed => speed;
    public int _level => level;
}
