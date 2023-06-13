using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum GUNTYTE
{
    NONE,
    RIFLE,
    PISTOL,
}

[CreateAssetMenu(fileName = "Gun Data", menuName = "Scriptable Object/Gun Data", order = 1)]
public class GunData : ItemData
{
    [SerializeField] int MaxLoder;   // 최대탄창
    [SerializeField] float sensitvity; // 반동
    [SerializeField] float reloadSpeed; // 재장전 속도
    [SerializeField] float FireDeley; //다음 총알을 발사할수있는 딜레이
    [SerializeField] float Noise;
    [SerializeField] ItemData compatibleAmmo; // Ammo의 id를 넣어준다
    [SerializeField] ParticleSystem fireEffect;
    [SerializeField] GUNTYTE gunType;
    [SerializeField] AUDIOCLIP FireAudio;
    [SerializeField] AUDIOCLIP ReloadAudio;
    public int _maxLoder { get { return MaxLoder; } }
    public float m_sensitvity { get { return sensitvity; } }
    public float m_reloadSpeed { get { return reloadSpeed; } }
    public float _fireDeley { get { return FireDeley; } }
    public float m_Noise { get { return Noise; } }
    public ItemData m_compatibleAmmo { get { return compatibleAmmo; } }
    public GUNTYTE m_gunType { get { return gunType; } }
    public AUDIOCLIP fireAudio => FireAudio;
    public AUDIOCLIP reloadAudio => ReloadAudio;

    public override ItemObject CreateItem() { return new Gun(this); }
}

