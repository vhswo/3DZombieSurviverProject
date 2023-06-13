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
    [Tooltip("�ִϸ��̼� Ÿ�� ������,��,�и� ��")]
    public WeaponType WeaponType;

    [Tooltip("�ִϸ��̼� Ÿ���� ����� �׼��� �Ұ�����")]
    public float WeaponNum;
}
