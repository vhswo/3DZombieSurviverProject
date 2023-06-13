using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionBehaviour : MonoBehaviour
{
    public PlayerInven playerInven;
    InvenSlot equipmenet = null;
    public Action<InvenSlot, int> SubScriptQuick;
    public Animator ani;
    Player player;
    
    [Header("카메라")]
    public Camera mainCamera;
    Cam cam;

    public Vector3 aimPivotOffset;
    public Vector3 aimCamOffset;

    //사격 수치
    float missRatio = 0.01f; // 미스률
    float bounds = 1.5f; // 반동
    float rateOfFire = 0.3f; // 연사속도 만큼 대기
    float Delaytime = 0f;
    bool Aim = false;
    bool reload = true;

    //애니메이션 관련
    Transform spine;
    Transform hips;
    Transform chest;

    Vector3 rootLocal;
    Vector3 spineLocal;
    Vector3 hipsLocal;
    Vector3 chestLocal;

    float upAngle = 0f;
    float rightAngle = 5f;

    float NoiseRadius = 0f;

    [SerializeField] GameObject attackRangeObj;
    EnemyAttackRange attackRange;
    public void Start()
    {
        ani = GetComponent<Animator>();
        cam = mainCamera.GetComponent<Cam>();
        player = GetComponent<Player>();
        playerInven.SubScriptToAction += UpdateEquip;
        attackRange = attackRangeObj.GetComponent<EnemyAttackRange>();

        hips = ani.GetBoneTransform(HumanBodyBones.Hips);
        spine = ani.GetBoneTransform(HumanBodyBones.Spine);
        chest = ani.GetBoneTransform(HumanBodyBones.Chest);

        rootLocal = hips.parent == transform ? Vector3.zero : hips.parent.transform.localEulerAngles;
        hipsLocal = hips.localEulerAngles;
        spineLocal = spine.localEulerAngles;
        chestLocal = chest.localEulerAngles;
    }

    public void UpdateEquip(InvenSlot slot)
    {
        equipmenet = slot;
    }

    private void Update()
    {
        if(Activate() || player.m_IsDead) return;

        AimManagement();

        string quickKey = QuickKey();

        if (quickKey != string.Empty)
        {
            int key = int.Parse(quickKey) - 1;
            playerInven.ChangeWeapon(key);
        }

        if (equipmenet == null) return; //현재 무기가 없다면 리턴한다

        //점프중이 아닌것도 추가하기
        if(Input.GetButtonDown("r") && reload &&(equipmenet.type == equipType.MainWeapon || equipmenet.type == equipType.SubWeapon))
        {
            CanReload();
        }

        //점프중 아닌거 체크
        if(Input.GetButton("LeftMouseClick"))
        {
            AttackTypeCheck();
        }
        if(!Activating && equipmenet.type == equipType.Throw && Input.GetButtonUp("LeftMouseClick"))
        {
            Activating = true;

            ani.SetTrigger("Throw");

            StartCoroutine(ThrowObj());
        }

    }

    IEnumerator ThrowObj()
    {
        while(Activating)
        {
            yield return new WaitForSeconds(0.1f);
        }

        GameObject throwObj = ObjectManager.instance.AppearObjAfterSeconds(FirePoint.position, FirePoint.forward, ObjectType.Grenade, 4f);

        Rigidbody throwRigid = throwObj.GetComponent<Rigidbody>();
        throwRigid.velocity = Vector3.zero;

        throwRigid.AddForce(dir, ForceMode.Impulse);

        equipmenet.amount -= 1;

        if (equipmenet.amount <= 0)
        {
            playerInven.ChangeWeapon((int)equipmenet.type);
        }
        else
        {
            SubScriptQuick.Invoke(equipmenet, equipmenet.amount);
        }

        StartCoroutine(FireThrow(throwObj, 4f));

    }
    [SerializeField] LayerMask throwFireTarget;
    [SerializeField] Vector3 fireSize;
    
    IEnumerator FireThrow(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        EffectManager.instance.EffectShot(obj.transform.position, obj.transform.position.normalized, EffectName.GrenadaExplosive);

        Collider[] col = Physics.OverlapSphere(obj.transform.position, 8f);
        Noise(obj.transform.position, 20f);
        fireSize = obj.transform.position;
        if (col.Length <= 0) yield break;

        foreach(Collider target in col)
        {
            RaycastHit hit;
            Vector3 direction = target.transform.position - obj.transform.position;
            float dis = direction.magnitude;
            bool obtacle = false;
            if (Physics.Raycast(target.transform.position,direction,out  hit, ~throwFireTarget))
            {
                obtacle = true;
            }
            direction = obj.transform.position - target.transform.position;

            if (Physics.Raycast(obj.transform.position, direction, out hit, dis, ~throwFireTarget))
            {
                obtacle = true;
            }

            if(obtacle == false && target.TryGetComponent(out IDamageable damage))
            {
                damage.GetDamage(200);
            }
        }
    }

    bool Activating = false;
    public bool Activate()
    {
        if(Activating)
        {
            if ((ani.GetCurrentAnimatorStateInfo(2).IsName("Melee") && ani.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.8f) ||
                 (ani.GetCurrentAnimatorStateInfo(2).IsName("Throw") && ani.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.8f) ||
                     (ani.GetCurrentAnimatorStateInfo(2).IsName("Potion") && ani.GetCurrentAnimatorStateInfo(2).normalizedTime > 0.8f))
            {
                Activating = false;
            }
            return true;
        }
        return false;
    }

    public void AimManagement()
    {
        if (Input.GetButtonDown("RightMouseClick"))
        {
            Aim = !Aim;

            if (Aim)
                cam.SetTargetOffset(aimPivotOffset, aimCamOffset);
            else
                cam.ResetTargetOffset();
        }
    }

    /// <summary>
    /// 여기서 부턴 어택관련
    /// </summary>
    public void AttackTypeCheck()
    {
        switch(equipmenet.type)
        {
            case equipType.MainWeapon:
            case equipType.SubWeapon:
                if (reload && Delaytime + rateOfFire < Time.realtimeSinceStartup) CanAttackFromGun();
                break;
            case equipType.Melee:
                if (!Activating)
                {
                    Activating = true;
                    ani.SetTrigger("MeleeAttack");
                    attackRangeObj.SetActive(true);
                }
                break;
            case equipType.Throw:
                ThrowSomething();
                break;
            case equipType.Potion:
                if (player.m_HP != 100 && !Activating)
                {
                    Activating = true;
                    ChangeStatus();
                }
                break;
            default:
                break;
        }
    }
    //포션
    public void ChangeStatus()
    {
        //애니메이션
        ani.SetTrigger("Drink");

        //애니메이션 에뮬레이터 시간 후 밑에 힐링ㅇ하기
        StartCoroutine(Dringking());
    }

    IEnumerator Dringking()
    {
        while(Activating)
        {
            yield return new WaitForSeconds(0.1f);
        }

        equipmenet.amount -= 1;
        player.Healing(equipmenet.item.damage);

        if(equipmenet.amount <= 0)
        {
            //장비창 비우기, equment 비우기, 퀵슬롯 비우기, 
            playerInven.ChangeWeapon((int)equipmenet.type);
        }
        else
        {
            //퀵슬롯 비우기
            SubScriptQuick.Invoke(equipmenet, equipmenet.amount);
        }
    }

    [SerializeField] LineRenderer _Line;
    [SerializeField] Transform FirePoint;

    //[SerializeField] float Power;
    //private void DrawPath(Vector3 direction, float v0, float angle,float time, float step)
    //{
    //    step = Mathf.Max(0.01f, step);
    //
    //    _Line.positionCount = (int)(time / step) + 2;
    //    int count = 0;
    //    for (float i = 0; i < time; i += step)
    //    {
    //        float x = v0 * i * Mathf.Cos(angle);
    //        float y = v0 * i * Mathf.Sign(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
    //        _Line.SetPosition(count, FirePoint.position + direction * x + Vector3.up * y);
    //        count++;
    //    }
    //    float xfinal = v0 * time * Mathf.Cos(angle);
    //    float yfinal = v0 * time * Mathf.Sign(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
    //    _Line.SetPosition(count, FirePoint.position + direction * xfinal + Vector3.up * yfinal);
    //}
    //
    //private float QuadraticEquation(float a, float b, float c, float sign)
    //{
    //    return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); //t = -b +-sqrt(pow(b) - 4ac) / 2a
    //}

    // private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle,out float time)
    // {
    //     // float Tx = targetPos.x;
    //     // float Ty = targetPos.y;
    //     // float g = -Physics.gravity.y;
    //     //
    //     //float b = Mathf.Sqrt(2 * g * h);
    //     //float a = (-0.5f * g);
    //     //float c = -Ty;
    //     //
    //     //
    //     //float tplus = QuadraticEquation(a, b, c, 1);
    //     //float tmin = QuadraticEquation(a, b, c, -1);
    //     //time = tplus > tmin ? tplus : tmin;
    //     //
    //     //
    //     //angle = Mathf.Atan(b * time / Tx); // cot0 = x /  (sqrt(2 * g* h) * t)
    //     //
    //     //    v0 = b / Mathf.Sin(angle); // v0 = sqrt(2 * g * h) / sin0
    //     // Debug.Log($"{angle}");
    //     
    //
    //     float g = -Physics.gravity.y;
    //     float a = (-0.5f * g);
    //     float V0x = Power * Mathf.Cos(cam.angleV);
    //     float V0y = Power * Mathf.Sin(cam.angleV); //b
    //     float Y0 = (FirePoint.position + FirePoint.forward).y;
    //     float height = 0f- Y0; //c
    //     float time1 = QuadraticEquation(a, V0y, -height, 1);
    //     float time2 = QuadraticEquation(a, V0y, -height, -1);
    //     time = time1 > time2 ? time1 : time2;
    //     
    //     angle = V0y;
    //     float Vy = V0y - g * time;
    //     v0 = Mathf.Sqrt(Mathf.Pow(V0x,2) + Mathf.Pow(Vy,2));
    //     Powor = v0;
    //     //
    // }   

    // [SerializeField] float Powor;

    Vector3 dir;
    public void ThrowSomething()
    {

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
         Vector3 direction;
          RaycastHit hit;
          if(Physics.Raycast(ray, out hit,16f, ~LayerMask.GetMask("Player")))
          {
              direction = hit.point - FirePoint.position;
          }
          else
         {
             direction = ray.origin + ray.direction * 16f - FirePoint.position;
        
         }
        
         Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        dir = groundDirection;
        // float _Step = 0.1f;
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
        
         float height = targetPos.y + targetPos.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        dir.y = height;


       //Rigidbody rigid= ball.GetComponent<Rigidbody>();
       //rigid.transform.position = FirePoint.position + FirePoint.forward;
       //rigid.useGravity = false;
       //rigid.velocity = Vector3.zero;
       //ball.GetComponent<SphereCollider>().enabled = false;

        // float angle;
        // float v0;
        // float time;
        // CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);
        //
        //DrawPath(groundDirection.normalized, v0, angle, time, _Step);
        //
        //StopAllCoroutines();
        // StartCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time));
        //
        // IEnumerator Coroutine_Movement(Vector3 direction,float v0,float angle,float time)
        //   {
        //       float t = 0;
        //       while(t < time)
        //       {
        //           float x = v0 * t * Mathf.Cos(angle); //x 값 구하기
        //           float y = v0 * t * Mathf.Sin(angle) -0.5f * - Physics.gravity.y  * Mathf.Pow(t,2); //y값 구하기
        //         ball.transform.position = FirePoint.position + direction * x + Vector3.up * y;
        //           t += Time.deltaTime;
        //           yield return null;
        //       }
        //   }
        //



        // x = x axis position
        // y = y axis position
        // v0 = initial velocuty
        // t = time
        // 0 = initial Angle
        // h = MaxHeight
        // g= Gravity




        //x = v0 * t * cos0
        //xt = vo0 * cos0 * pow(t)
        //y = v0 * t * sin0 - 1/2 * g * pow(t)


        // t = ?
        // t = x / (v0 * cos0)

        //v0 = ?
        // y = v0 * (x / (v * cos0)) * sin0 - 1/2 * g * pow(x / (v * cos0))
        // y = x * tan0 - 1/2 * g * ( pow(x) / pow(v0) * pow(cos) )
        // ((x * tan0 - y) * 2 * pow(cos0)) / pow(x) * g     =    1/ pow(v0)

        // v0 = sqrt( ( pow(x) * g ) / 2 * x * sin0 * cos0 - 2 * y * pow(cos0) )

        //세타 = ?

        // h = pow(v0 * sin0) / (2 * g)
        //sin0 = sqrt(2 * g * h) / v0;
        //세타 = arcsin(sqrt(2 * g * h) / v0)


        //x,y,h 를 알고
        //v0,세타,t 를 모른다

        //h = pow(v0 * sin0) / (2 * g)
        //v0 = sqrt(2 * g * h) / sin0
        //x = v0 * t * cos0 , x = sqrt(2 * g * h) / sin0 * t *cos0
        // = cos0 / sin0 = x / (sqrt(2 * g * h) * t)
        //  cos0 = x / (sqrt(2 * g * h) * t)

        //y = v0 * t * sin0 - 1/2 * g * pow(t) = 0 = -1/2 * g * pow(t) + v0 * sin0 * t -y
        // = 0 = -1/2 * g * pow(t) + sqrt(2 * g * h) / sin0 * sin0 * t -y
        // = 0 = -1/2 * g * pow(t) + sqrt(2 * g * h) * t -y;

        // a = -1 / 2 * g;
        // b = sqrt(2 * g * h)
        // c = -y

        // t = (-b +- sqrt(pow(b) - 4ac)) / 2a
        // t = (-sqrt(2 * g * h) +- sqrt(2 * g * h - 4 * (-1/2 *g) * -y )) / (2 * -1/2 * g)
        // t = -sqrt(2 * g* h) +- sqrt(2 * g * h - 2 * g * y) / -g
        // t = -sqrt(2 * g* h) +- sqrt(2 * g * (h -y)) / -g

        //tmin = -sqrt(2 * g *h) - sqrt(2 * g * (h-y)) / -g
        //tplus = -sqrt(2 * g * h) + sqrt(2 * g * (h - y)) / -g
        // t = Max(tmin,tplus);
        // t를 구함
        // cos0 = x / (sqrt(2 * g * h) * t) => 0 = arccot(x / sqrt(2 g h) * t)
        // 세타 구함
        // v0 = sqrt(2 *g *h) / sin0;
        // v0 구함



    }

    //Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    //{
    //    float gravity = Physics.gravity.magnitude;
    //    float angle = initialAngle * Mathf.Deg2Rad;
    //
    //    Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
    //    Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);
    //
    //    float distance = Vector3.Distance(planarTarget, planarPosition);
    //    float yOffset = currentPos.y - targetPos.y;
    //
    //    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
    //
    //    Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
    //
    //    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
    //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
    //
    //    return finalVelocity;
    //}

    //근접 공격
    public void MeleeAttack()
    {
        if (!attackRangeObj.activeSelf) return;

        LayerMask target = LayerMask.GetMask("Enemy");

        Collider[] enemy = attackRange.MeleeAttack(target);

        if (enemy.Length <= 0) return;

        foreach (Collider col in enemy)
        {
            if(col.TryGetComponent(out IDamageable damage))
            {
                damage.GetDamage(equipmenet.item.damage);
            }
        }
        attackRangeObj.SetActive(false);
    }

    //사격
    public bool CanAttackFromGun()
    {
        if (equipmenet.slotItem.Bullets <= 0)
        {
            GameManager.Instance.AudioMgr.ShotSound(AUDIOCLIP.Gun_EMPTYFIRE, transform.position);
            return false;
        }
        Delaytime = Time.realtimeSinceStartup;

        ShootBullet();

        return true;
    }

    /// <summary>
    /// 추가 해야할것,  타겟 레이어에 따라 튕김이펙트, 홀 다르게
    /// </summary>
    public void ShootBullet()
    {
        if(equipmenet.slotItem.firePos  == null) equipmenet.slotItem.firePos = playerInven.HaveWeapon.Find(equipmenet.item.ItemName).GetComponent<AddToItem>().firePos; //들고있는 총구의 위치를 찾아준다

        Vector3 start = equipmenet.slotItem.firePos.position;
        Vector3 target;
        EffectManager.instance.EffectShot(start, equipmenet.slotItem.firePos.forward * 0.2f ,EffectName.GunFlash);
        GameManager.Instance.AudioMgr.ShotSound(AUDIOCLIP.Rifle_FIRE, transform.position);
        Noise(transform.position, 10f);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 500f, ~LayerMask.GetMask("Player")))
        {
            target = hit.point;
            Vector3 missRatios = Camera.main.transform.forward * UnityEngine.Random.Range(-missRatio, missRatio);

            target += missRatios;

            EffectManager.instance.EffectShot(hit.point, hit.normal, EffectName.BulletHit);

            if (hit.transform.TryGetComponent(out IDamageable enemy))
            {
                enemy.GetDamage(equipmenet.item.damage,hit.point.y);
                EffectManager.instance.EffectShot(hit.point, hit.normal, EffectName.Blood);
            }
            else
            {
                ObjectManager.instance.AppearObjAfterSeconds(hit.point, hit.normal, ObjectType.BulletHole, 3f);
            }

        }
        equipmenet.slotItem.Bullets -= 1;
        SubScriptQuick.Invoke(equipmenet, playerInven.GetBulletNum(equipmenet.item.bulletName));
        cam.bounds += this.bounds;
    }


    /// <summary>
    /// 퀵슬롯 키 관련
    /// </summary>
    public string QuickKey()
    {
        string key;

        if (Input.GetButtonDown("1")) { key = "1"; }
        else if (Input.GetButtonDown("2")) { key = "2"; }
        else if (Input.GetButtonDown("3")) { key = "3"; }
        else if (Input.GetButtonDown("4")) { key = "4"; }
        else if (Input.GetButtonDown("5")) { key = "5"; }
        else if (Input.GetButtonDown("6")) { key = "6"; }
        else if (Input.GetButtonDown("TakeOffWeapon")) { key = "-1"; }
        else { key = string.Empty; }

        return key;
    }

    /// <summary>
    /// 여기서 부턴 리로드 관련
    /// </summary>
    public void CanReload()
    {
        if (equipmenet.slotItem.Bullets < equipmenet.item.maxBullet && playerInven.GetBulletNum(equipmenet.item.bulletName) > 0)
        {
            StartCoroutine(CheckReloadAni());
        }
    }

    public void Reload()
    {
        int findBullet = equipmenet.item.maxBullet - equipmenet.slotItem.Bullets;
        equipmenet.slotItem.Bullets += playerInven.GetBullet(equipmenet.item.bulletName, findBullet);
        SubScriptQuick.Invoke(playerInven.equipInven.slots[(int)equipmenet.type], playerInven.GetBulletNum(equipmenet.item.bulletName));
        reload = true;
    }

    IEnumerator CheckReloadAni()
    {
        ani.SetTrigger("Reloading");
        GameManager.Instance.AudioMgr.ShotSound(AUDIOCLIP.Pistol_RELOAD, transform.position);
        reload = false;
        yield return new WaitForSeconds(0.5f);

        while (ani.GetCurrentAnimatorStateInfo(2).IsName("Reload") && ani.GetCurrentAnimatorStateInfo(2).normalizedTime < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Reload();
    }

    /// <summary>
    /// 에임 가슴 굽힘 관련 애니메이션 뼈대
    /// </summary>

    private void OnAnimatorIK(int layerIndex)
    {
        if (equipmenet == null || !(equipmenet.type != equipType.MainWeapon || equipmenet.type != equipType.SubWeapon)) return;

        Quaternion targetTot = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        targetTot *= Quaternion.Euler(rootLocal);
        targetTot *= Quaternion.Euler(hipsLocal);
        targetTot *= Quaternion.Euler(spineLocal);

        ani.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(hips.rotation) * targetTot);

        float xcamRot = Quaternion.LookRotation(Camera.main.transform.forward).eulerAngles.x;

        targetTot = Quaternion.AngleAxis(xcamRot + rightAngle, transform.right);

        targetTot *= Quaternion.AngleAxis(upAngle, transform.up);

        targetTot *= spine.rotation;
        targetTot *= Quaternion.Euler(chestLocal);
        ani.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Inverse(spine.rotation) * targetTot);
    }

    private void OnDrawGizmos()
    {
        if (fireSize != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(fireSize, 8f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(fireSize, 16f);
        }

        Gizmos.DrawWireSphere(transform.position, NoiseRadius);


    }

    void Noise(Vector3 pos,float radius)
    {
        NoiseRadius = radius;

        Collider[] cols = Physics.OverlapSphere(pos, radius);

        if(cols.Length <=0) return;

        foreach (Collider col in cols)
        {
            if(col.TryGetComponent(out HearNoise hear))
            {
                hear.Heared(pos);
            }
        }
    }
}
