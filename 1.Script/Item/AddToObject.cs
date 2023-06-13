using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시간되면 enum 자동생성 오브젝트 타입과 오브젝트의 이름은 같다
/// </summary>
public enum ObjectType
{
    None,
    Bullet,
    BulletHole,
    Grenade,
}


public class AddToObject : MonoBehaviour,HearNoise
{
    public ObjectType type;
    public LayerMask target;

    public void Heared(Vector3 hearedNoisePos)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, target);

        if (colliders.Length <= 0) return;

        foreach(Collider col in colliders)
        {
            if(col.TryGetComponent(out HearNoise send))
            {
                send.Heared(transform.position);
            }
        }
    }

    private void Awake()
    {
        if (type == ObjectType.None) Debug.LogWarning("오브젝트 타입을 선택하지 않은 객체 : " + this.gameObject);

        gameObject.name = type.ToString(); //혹시 몰라 대비
    }

    private void Update()
    {
        if (Input.GetKeyDown("n"))
        {
            Heared(transform.position);
            GameManager.Instance.AudioMgr.ShotSound(AUDIOCLIP.Car_ALRAM,transform.position);
        }
    }

}
