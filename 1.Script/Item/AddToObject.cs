using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ð��Ǹ� enum �ڵ����� ������Ʈ Ÿ�԰� ������Ʈ�� �̸��� ����
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
        if (type == ObjectType.None) Debug.LogWarning("������Ʈ Ÿ���� �������� ���� ��ü : " + this.gameObject);

        gameObject.name = type.ToString(); //Ȥ�� ���� ���
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
