using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ItemObject
{
    public GunData gunData { get; private set; }
    public Gun(GunData data) : base(data) { gunData = data; }

    public Transform firePos { get; private set; } //�ѱ� (�Ѿ��� �߻�Ǵ� ��ġ)

    float lastFire; //���� ���������� �� �ð�
    public int RemainAmmo { get; private set; } =0 ;// ���� �Ѿ��� ��

    public void Init(Transform pos)
    {
        lastFire = 0;
        RemainAmmo = 0;
        firePos = pos;
    }

    public void SetFirePos(Transform firepos)
    {
        firePos = firepos;
    }

    /// <summary>
    /// ���� false �� �Ѿ��� ���ٴ°�
    /// </summary>
    /// <returns></returns>
    public bool Shot()
    {
        RemainAmmo--;
        return true;
    }

    public void Reload(int ammo)
   {
        RemainAmmo += ammo;
   }
   public int AmmoId()
   {
        return (Data as GunData).m_compatibleAmmo.id;
   }

    /// <summary>
    /// ���� ��� �Ѿ��� ���ư� ��ġ�� �˷��ش�
    /// </summary>
    public Vector3 Shoot()
    {
        if (RemainAmmo <= 0 || Time.time < lastFire + gunData._fireDeley) return Vector3.zero;

        lastFire = Time.time;

        RemainAmmo--;

        //�Ҹ�, ����Ʈ
        return firePos.position;
        //�ѱ��� �������� ������ �Ǹ�, �߻簡 �����ѰŰ�, 
        //��ȯ�� ���������� �Ѿ� ����, ����Ʈ �߻�
    }
}
