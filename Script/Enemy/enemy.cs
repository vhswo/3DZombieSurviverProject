using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] ZombieData data;
    [SerializeField] ParticleSystem GetHitEffect;
    public Zombie zombie;
    private NavMeshAgent Agent;
    private Animator aim;
    private Transform Target;
    public Rigidbody rigid { get; private set; } //Ʈ������ �����Ǻ��� ������ٵ� �������� ����� �ξ� �����ȴ�
    private FieldOfView FOV;

    private ENEMYSTATE state;
    private float distance;
    private bool m_IsDead;


    private void Start()
    {
        zombie = data.CreateZombie();
        FOV = new FieldOfView();
        Agent = GetComponent<NavMeshAgent>();
        aim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        GetHitEffect.transform.position += Vector3.up;
    }
    public float GetDamage(float damage,float height = 0)
    {
        //zombie.HP -= damage;
        //Debug.Log("hp" + zombie.HP);
        //GetHitEffect.Play();
        //if (zombie.HP <= 0)
        //{
        //    Dead();
        //}
        //
        return zombie.HP;
    }

    public void SetZombie()
    {
        state = ENEMYSTATE.NONE;
        Target = null;
        m_IsDead = false;
    }

    public void SetLocation(Vector3 pos)
    {
        transform.position = pos;


    }

    /// <summary>
    /// �Ҹ��� ���� �ϸ� Ʈ���� �ߵ�
    /// </summary>
  //   private void OnTriggerEnter(Collider other)
  //  {
  //      if (m_IsDead) return;
  //
  //      switch (other.tag)
  //      {
  //          case "Noise":
  //              if (state != ENEMYSTATE.NONE) return;
  //              Target = other.transform.root;
  //              state = ENEMYSTATE.FOLLOWNOISE;
  //              break;
  //          case "bullet":
  //              Vector3 rectVec = transform.position - other.transform.position;  // ���� ��ġ���� �ǰ� ��ġ�� ���� ���ۿ� ���� ���ϱ�
  //              OnDamage(rectVec); // �˹�
  //              break;
  //          case "MeleeWeapon":
  //              break;
  //          default:
  //              break;
  //      }
  //
  //   }

    public void Dead()
    {
        m_IsDead = true;
        // ���� �ִϸ��̼�

        StartCoroutine(ZombieDie());
    }

    IEnumerator ZombieDie()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
       // GameManager.Instance.m_enemyManagerment.Zombies[zombie.Data.iNumber].AddObj(this);


    }
    public void OnDamage(Vector3 rectVec)
    {
        rectVec = rectVec.normalized;
        rigid.AddForce(rectVec * 5, ForceMode.Impulse);

        StartCoroutine(Push());
    }

    IEnumerator Push()
    {
        yield return new WaitForSeconds(0.1f);
        rigid.isKinematic = true;
        rigid.isKinematic = false;
    }

    public void SearchPlayer()
    {
        switch (state)
        {
            case ENEMYSTATE.NONE:
            case ENEMYSTATE.FOLLOWNOISE:
                Transform EnterTarget = FOV.CheckPlayer(transform);
                if (EnterTarget == null) return;
                if (EnterTarget.CompareTag("Player"))
                {
                   // aim.SetBool("Run", true);
                    Target = EnterTarget;
                    var dir = (Target.transform.position - rigid.position).normalized;
                    transform.rotation = Quaternion.LookRotation(dir);
                    state = ENEMYSTATE.FOLLOWPLAYER;
                }
                break;
            case ENEMYSTATE.FOLLOWPLAYER:
            case ENEMYSTATE.ATTACK:
            case ENEMYSTATE.DIE:
                break;
            default:
                break;

        }
    }


  //  private void FixedUpdate()
  //  {
  //      if (m_IsDead || state == ENEMYSTATE.ATTACK) return;
  //    // if (GameManager.Instance.IsOnTheSafeHouse)
  //    // {
  //    //     StopEnemy();
  //    //     return;
  //    // }
  //
  //
  //      SearchPlayer(); //�÷��̾ ã�´�
  //
  //      if (Target != null)
  //      {   //�÷��̾ �Ҹ��� �Ѱ� �ִ�
  //          aim.SetBool("Run", true); 
  //          Agent.isStopped = false;
  //          Agent.SetDestination(Target.position); //Ÿ���� ���� ��ġ�� ����
  //          distance = Vector3.Distance(rigid.position, Target.position); //����� ��ü or �÷��̾� ���� �Ÿ�
  //
  //          if (distance < zombie.Data.AttackRange) Arrive(); // �������� ����
  //
  //      }
  //
  //  }

    /// <summary>
    /// �Ҹ��� ���������� ����
    /// </summary>
    public void Arrive()
    {
        switch (state)
        {
            case ENEMYSTATE.FOLLOWPLAYER:
              //  StartCoroutine(Attack());
                break;
            case ENEMYSTATE.FOLLOWNOISE:
                StopEnemy();
                //�¿� ���Ǵ�  �ִϸ��̼� ����
                break;
            default:
                break;
        }
    }

    public void StopEnemy()
    {
        if (state == ENEMYSTATE.NONE) return;

        var dir = (Target.position - rigid.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        aim.SetBool("Run", false);
        Target = null;
        Agent.isStopped = true;
        state = ENEMYSTATE.NONE;
    }

   // IEnumerator Attack()
   // {
   //     Player player = Target.GetComponent<Player>(); //���ӸŴ����� ���� ���´�
   //    
   //     StopEnemy();
   //
   //     if (player.m_IsDead)
   //     {
   //         yield break;
   //     }
   //
   //     state = ENEMYSTATE.ATTACK;
   //     aim.SetTrigger("Hit");
   //     RightHand.ResetHit();
   //     yield return new WaitForSeconds(1.0f);
   //     RightHand.GetComponent<BoxCollider>().enabled = true;
   //     yield return new WaitForSeconds(1.5f);
   //
   //     RightHand.GetComponent<BoxCollider>().enabled = false;
   //     Agent.isStopped = false;
   //     Target = player.transform;
   //     state = ENEMYSTATE.FOLLOWPLAYER;
   // }
}
