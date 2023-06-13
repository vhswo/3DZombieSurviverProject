using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

using UnityEngine.UI;

public interface HearNoise
{
    void Heared(Vector3 hearedNoisePos); // 소리가 난 곳을 가지고 온다
}

public interface IDamageable
{
    float GetDamage(float damage,float height = 0);
    bool IsDead();
}

public enum enemyState
{
    Wait,
    Patrol,
    Guard,
    Follow,
    Attack,
}

public class EnemyBehaviour : MonoBehaviour, HearNoise,IDamageable
{
    float hp = 100;
    float damamge = 20;
    NavMeshAgent agent;
    enemyState state = enemyState.Wait;
    enemyState exState = enemyState.Wait;
    public Transform[] patrolPos;
    int PatrolArea = 0;
    Vector3 target = Vector3.zero;
    Animator ani;
    [SerializeField] EnemyAttackRange attackRange;

    float MoveSpeed = 1f;
    float FollowSpeed = 3f;

    [SerializeField] float radius = 8f;
    [SerializeField] bool GizmoOn = true;
    [SerializeField] Material stateColor;

    bool Attacking = false;
    public bool IsDead() { return hp <= 0 ? true : false; }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        stateColor.color = Color.green;
    }

    public void Heared(Vector3 hearedNoisePos)
    {
        if (state == enemyState.Follow || state == enemyState.Attack) return; //공격중이거나 플레이어를 쫓아가고있으면 리턴

        state = enemyState.Guard;
        target = hearedNoisePos;
    }

    public float GetDamage(float damage, float height = 0)
    {
        if (hp <= 0) return hp;

        if (agent.height - height <= 0.12f) damage *= 10f;

        hp -= damage;

        Debug.Log($"hp : {hp}, damage : {damage}");
        if (hp <= 0)
        {
            Die();
        }

        return hp;
    }


    bool dieAniCheck;
    public void Die()
    {
        //초기화
        StopAgent();
        StopAllCoroutines();
        ani.SetTrigger("Die");
        dieAniCheck = true;
        stateColor.color = Color.gray;

        StartCoroutine(CheckEndDie());
    }

    IEnumerator CheckEndDie()
    {
        while (dieAniCheck)
        {
            if(ani.GetCurrentAnimatorStateInfo(0).IsName("Die") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
            {
                dieAniCheck = false;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        Destroy(this.gameObject);
    }

    public void Behaviour()
    {
        switch(state)
        {
            case enemyState.Wait:
                Wait();
                break;
            case enemyState.Patrol:
                Patrol();
                break;
            case enemyState.Guard:
                Guard();
                break;
            case enemyState.Follow:
                Follow();
                break;
            case enemyState.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    float WaitTime = 0f;
    float MaxWaitTime = 3f;
    public void Wait()
    {
        if(exState != state)
        {
            WaitTime = 0f;
            exState = state;
            stateColor.color = Color.green;
            ani.SetBool("Walk", false);
            ani.SetBool("Run", false);
        }

        WaitTime += Time.deltaTime;

        if(WaitTime >= MaxWaitTime)
        {
            state = enemyState.Patrol;
        }

    }

    public void Patrol()
    {
        if (patrolPos.Length <= 0) return;

        if (exState != state)
        {
            exState = state;
            stateColor.color = Color.green;
            agent.speed = MoveSpeed;

            ani.SetBool("Walk", true);
            ani.SetBool("Run", false);
        }

        agent.destination = patrolPos[PatrolArea].position;

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            state = enemyState.Wait;
            PatrolArea++;
            PatrolArea = PatrolArea % patrolPos.Length;
        }

    }

    float GuardTime = 5f;
    public void Guard()
    {
        if (exState != state)
        {
            WaitTime = 0;
            agent.speed = MoveSpeed;
            stateColor.color = Color.yellow;
            exState = state;
            StopAgent();
            ani.SetBool("Run", false);
            ani.SetBool("Walk", true);
        }

        if (Mathf.Abs(Vector3.Angle(transform.forward,target - transform.position)) > 20f)
        {
            Quaternion qu = Quaternion.LookRotation(target - transform.position);

            //Debug.DrawRay(transform.position, target - transform.position, Color.green);

            transform.rotation = Quaternion.Slerp(transform.rotation, qu, 2f * Time.deltaTime);
        }
        else
        {
            agent.speed = MoveSpeed;
            agent.destination = target;
            

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                WaitTime += Time.deltaTime;

                if (WaitTime >= GuardTime)
                {
                    state = enemyState.Wait;
                }
            }

        }
    }

    public bool AniTimeCheck(string name,float time)
    {
        return ani.GetCurrentAnimatorStateInfo(0).IsName(name) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > time;
    }

    bool findPlayer = true;
    public void Follow()
    {
        if (exState != state)
        {
            if(exState != enemyState.Attack)
            {
                ani.SetTrigger("FindPlayer");
                StopAgent();
                findPlayer = false;
            }
            agent.speed = FollowSpeed;
            exState = state;
            stateColor.color = Color.red;
            ani.SetBool("Walk", false);
            ani.SetBool("Run", false);
        }

        if (!findPlayer)
        {
            findPlayer = AniTimeCheck("FindPlayer", 0.8f);
            return;
        }

        if (!Search(1.5f)) return;

        ani.SetBool("Run", true);
        agent.destination = target;

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && canAttack)
        {
            state = enemyState.Attack;
        }
    }

    public void StopAgent()
    {
        agent.speed = 0f;
        agent.ResetPath();
        ani.SetBool("Walk", false);
        ani.SetBool("Run", false);
    }

    public void Attack()
    {
        StopAgent();
        exState = enemyState.Attack;

        Attacking = true;
      
        ani.SetTrigger("Attack");
    }

    public LayerMask LayerTarget;
    public void IsAttack()
    {
        Collider[] hit = attackRange.MeleeAttack(LayerTarget);

        if (hit.Length <= 0) return; 

        if(hit[0].TryGetComponent(out IDamageable player))
        {
            float hp = player.GetDamage(damamge);

            if (hp <= 0)
            {
                StopAgent();
                state = enemyState.Wait;
                return;
            }
        }
    }

    bool canAttack = false;
    public bool FrontView()
    {
        if (state == enemyState.Attack) return false;

        Collider[] SearchPlayer = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Player"));

        if (SearchPlayer.Length <= 0) return false;

        if(SearchPlayer[0].TryGetComponent(out IDamageable hp) && hp.IsDead()) return false;

        float viewAngle = 40f;

        if (state == enemyState.Follow) viewAngle = 20f;

        foreach(Collider col in SearchPlayer)
        {
            Vector3 PlayerDis = col.transform.position - transform.position;

            if (TargetToAngle(PlayerDis,viewAngle))
            {
                //CheckObstacle 이 true 면 장애물에 걸림,
                if (CheckObstacle(col.transform.position,radius) && state != enemyState.Follow) break;
                //타겟, 빨간색
                target = col.transform.position;
                state = enemyState.Follow;
                canAttack = true;
                return true;
            }
            else
            {
                if(state == enemyState.Follow)
                {
                    target = col.transform.position;
                    Quaternion qu = Quaternion.LookRotation(target - transform.position);

                    Debug.DrawRay(transform.position, target - transform.position, Color.green);

                    transform.rotation = Quaternion.Slerp(transform.rotation, qu, 2f * Time.deltaTime);
                }
                canAttack = false;
            }
        }

        return false;
    }

    bool TargetToAngle(Vector3 target,float Angle)
    {
        float dot = Vector3.Angle(target, transform.forward);
        if (Mathf.Abs(dot) <= Angle) return true;

        return false;
    }

    bool CheckObstacle(Vector3 target,float radius)
    {
        if(Physics.Raycast(transform.position,target - transform.position,radius,LayerMask.GetMask("Obstacle")))
        {
            return true;
        }
        if(Physics.Raycast(target, transform.position - target,radius,LayerMask.GetMask("Obstacle")))
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (!GizmoOn) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius * 1.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f,transform.forward * 10f);
        Gizmos.color = Color.red;
        float ra = (transform.eulerAngles.y + 40f) * Mathf.Deg2Rad;
        float le = (transform.eulerAngles.y - 40f) * Mathf.Deg2Rad;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, new Vector3(Mathf.Sin(ra),0f,Mathf.Cos(ra)) * 10f);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, new Vector3(Mathf.Sin(le), 0f, Mathf.Cos(le)) * 10f);
    }

    private void Update()
    {
        if (hp <= 0) return;
        if(Attacking)
        {
            if(ani.GetCurrentAnimatorStateInfo(0).IsName("Attack") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                Search();
                Attacking = false;
            }
            return;
        }

        FrontView();

        Behaviour();

    }

    public bool Search(float MultyRadius = 1f)
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius * MultyRadius, LayerTarget);

        if (col.Length <= 0)
        {
            state = enemyState.Wait;
            StopAgent();

            return false;
        }
        else
        {
            if (col[0].TryGetComponent(out IDamageable DeadCheck) && DeadCheck.IsDead()) return false; 
            state = enemyState.Follow;
            this.target = col[0].transform.position;
            //foreach (Collider target in col)
            //{
            //    this.target = target.transform.position;
            //}
            return true;
        }
    }

}

// FSM 좀 더 손보기 플레이어 찾은거와 노이즈 분리 어떻게 할지 생각