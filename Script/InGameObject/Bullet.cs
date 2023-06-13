using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : InGameObjects
{
    [SerializeField] Rigidbody rigid;
    public float m_damage { get; private set; }
    private Ray ray;
    private RaycastHit rayHit;
    private LayerMask layerMask;
    public int NowPenetrate { get; private set; } //현재관통
    private int m_MaxPenetrate;

    public void SetBullet(float damage,Vector3 pos, int MaxPenetrate,Transform direction,Vector3 DirectionPoint)
    {
        m_damage = damage;
        NowPenetrate = 0;
        m_MaxPenetrate = MaxPenetrate;
        transform.position = pos;
        transform.rotation = direction.rotation;

        layerMask = ~LayerMask.GetMask("Bullet");
        gameObject.SetActive(true);
        rigid.velocity = direction.forward * 20 + DirectionPoint;

    }

    public void PutBullet()
    {
        if(gameObject.activeSelf) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        NowPenetrate++; //관통 능력을 만들지 말지 고민중

        if(other.GetComponent<IDamageable>() is IDamageable target )
        {
            target.GetDamage(m_damage);
        }
        //else
          //  Crush();

        if (NowPenetrate >= m_MaxPenetrate)
            gameObject.SetActive(false);
    }

  //  private void Crush()
  //  {
  //     if (Physics.Raycast(rigid.position - transform.forward, transform.forward, out rayHit, 5.0f,layerMask))
  //      {
  //          (GameManager.Instance.FindObj(OBJTYPE.BULLETHOLE) as BulletHole).OnBulletHole(rayHit.point, rayHit.normal);
  //
  //      }
  //  }
}
