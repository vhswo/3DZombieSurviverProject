using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ItemObject
{
    public GunData gunData { get; private set; }
    public Gun(GunData data) : base(data) { gunData = data; }

    public Transform firePos { get; private set; } //총구 (총알이 발사되는 위치)

    float lastFire; //총을 마지막으로 쏜 시간
    public int RemainAmmo { get; private set; } =0 ;// 남은 총알의 수

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
    /// 리턴 false 는 총알이 없다는것
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
    /// 총을 쏘면 총알이 날아갈 위치를 알려준다
    /// </summary>
    public Vector3 Shoot()
    {
        if (RemainAmmo <= 0 || Time.time < lastFire + gunData._fireDeley) return Vector3.zero;

        lastFire = Time.time;

        RemainAmmo--;

        //소리, 이펙트
        return firePos.position;
        //총기의 포지션을 보내게 되면, 발사가 성공한거고, 
        //반환된 포지션으로 총알 생성, 이펙트 발사
    }
}
