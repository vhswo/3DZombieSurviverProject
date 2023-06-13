using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThrowingObject : InGameObjects
{
    [SerializeField] float m_radius;
    [SerializeField] Rigidbody rigid;
    public float m_damage; 
    public LayerMask m_TargetLayer;
    public float MaxBombTimer;
    public ParticleSystem m_bombParticle;

    // ����κ� ���� �� �θ�� ����� �ڽ����� ����ź,����ź�� �����

    public void ThrowObject(List<Vector3> LinePonit,float height,float time)
    {
        gameObject.SetActive(true);
        m_TargetLayer = LayerMask.GetMask("Player") + LayerMask.GetMask("Enemy");

        rigid.position = LinePonit[0];

        Vector3 velo = ThrowVelo(LinePonit, height);
        transform.rotation = Quaternion.LookRotation(velo);

        rigid.velocity = velo;

        StartCoroutine(BombCheck(time));

    }

    Vector3 ThrowVelo(List<Vector3> LinePonit, float height)
    {
        Vector3 start = LinePonit[0]; // ���� ����
        Vector3 target = LinePonit[LinePonit.Count - 1]; //��������

        float time = 1.0f + height * 0.5f;

        Vector3 distance = target - start; // �ѱ��� x / t = �ӵ�
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;


        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        //�ӵ� ���
        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;

        result *= Vxz;
        result.y = Vy;

        return result;
    }

    IEnumerator BombCheck(float Bombtime)
    {
        yield return new WaitForSeconds(MaxBombTimer - Bombtime);

        Explode();
    }

    private void Explode()
    {

        Collider[] Targets = Physics.OverlapSphere(transform.position, m_radius, m_TargetLayer);

        foreach(Collider targets in Targets)
        {
            Debug.Log("�ݸ��� : " + targets);
            //�Ÿ�����Ͽ� �� �ٸ� ������ ������ �ֵ��� ������ ����

            if(targets.GetComponent<IDamageable>() is IDamageable target)
            {
                Debug.Log("taget : " + target);
                target.GetDamage(m_damage);
            }
        }

        m_bombParticle.Play();

        StartCoroutine(IsEndParticle());

    }

    IEnumerator IsEndParticle()
    {
        while (m_bombParticle.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
        GameManager.Instance.m_GbjMgr.m_Objects[OBJTYPE.GRANDER].AddObj(this);
    }

}