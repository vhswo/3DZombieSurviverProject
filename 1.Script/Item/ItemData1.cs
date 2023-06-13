using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "PluggableAI/Items", fileName = "Item")]
public class ItemData1 : ScriptableObject
{
    public string ItemName;
    public string bulletName;
    public equipType type;
    public Sprite icon;
    public bool IsInven;
    public bool IsEquip;
    public bool Countable;
    public int maxBullet;
    public float damage;
    [Tooltip("애니메이션 타입 라이플,건,밀리 등")]
    public WeaponType WeaponType;

    [Tooltip("애니메이션 타입의 몇번쨰 액션을 할것인지")]
    public float WeaponNum;
}
