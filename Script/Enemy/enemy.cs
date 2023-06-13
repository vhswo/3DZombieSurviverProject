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
    public Rigidbody rigid { get; private set; } //트랜스폼 포지션보다 리지드바디 포지션이 비용이 훨씬 절감된다
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
    /// 소리가 접근 하면 트리거 발동
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
  //              Vector3 rectVec = transform.position - other.transform.position;  // 현재 위치에서 피격 위치를 빼서 반작용 방향 구하기
  //              OnDamage(rectVec); // 넉백
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
        // 죽음 애니메이션

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
  //      SearchPlayer(); //플레이어를 찾는다
  //
  //      if (Target != null)
  //      {   //플레이어나 소리를 쫓고 있다
  //          aim.SetBool("Run", true); 
  //          Agent.isStopped = false;
  //          Agent.SetDestination(Target.position); //타겟의 현재 위치를 전달
  //          distance = Vector3.Distance(rigid.position, Target.position); //좀비와 물체 or 플레이어 와의 거리
  //
  //          if (distance < zombie.Data.AttackRange) Arrive(); // 목적지에 도착
  //
  //      }
  //
  //  }

    /// <summary>
    /// 소리의 도착지점에 도착
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
                //좌우 살피는  애니메이션 동작
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
   //     Player player = Target.GetComponent<Player>(); //게임매니저를 통해 들고온다
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
