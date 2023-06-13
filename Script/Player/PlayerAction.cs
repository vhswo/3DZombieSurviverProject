using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYERSTATE
{
    NONE,
    RELOAD,
    ATTACK,
    THROWREADY,
    THROWING,
    OPENINGLID,
    DINKING
}

public enum ZOOM
{
    NONE,
    ZOOM
}
/*
public class PlayerAction : MonoBehaviour
{
    private Player _player;
    private PlayerAnimation aim;
    private ZOOM zoomState;

    private float m_UseItemTime;
    private bool Attacking;

    [Header("사격 관련")]
    [SerializeField] GameObject ShotHole; //탄흔
    private float MaxSpread; //최대 탄 퍼짐
    private float Stablilty; //안정성
    private float RestoreFromRecoilSpeed; // 현재 탄 퍼짐이 기본으로 돌아오는 속도
    private float CurrentSpread; // 현재 탄 퍼짐 정도
    private float StartCurrentSpread; // 현재 탄 퍼짐 정도
    private float currentSpreadVelocity; //실시간 탄 퍼짐 범위 조절
    private float SpreadVelocity; //실시간 탄 퍼짐 범위 조절
    Coroutine coroutine;

    [Header("탄 관련")]
    private Transform LookAim;
    private Vector3 DirectionPoint;
    private Vector2 RandomCoordinate; //x ,z        //랜덤 좌표

    ThrowingObject grander;

    public void Init(Player player)
    {
        _player = player;
        aim = _player.aim;
        zoomState = ZOOM.NONE;

        SetResetRebound(0.7f, 0.3f);
        _player.m_Input.QuickSlotChangeEventListener += ChangeQuickSlotEventListener;
        _player.m_Input.ActionEventListener += ActionEventListener;

        
    }

    /// <summary>
    /// 반동초기화
    /// </summary>
    public void ResetRebound()
    {
        if (zoomState == ZOOM.NONE) zoomState = ZOOM.ZOOM;
        else zoomState = ZOOM.NONE;

        switch (zoomState)
        {
            case ZOOM.NONE:
                _player.m_mainCine.gameObject.SetActive(true);
                _player.m_zoomCine.gameObject.SetActive(false);
                SetResetRebound(0.7f, 0.3f);
                break;
            case ZOOM.ZOOM:
                _player.m_mainCine.gameObject.SetActive(false);
                _player.m_zoomCine.gameObject.SetActive(true);
                SetResetRebound(0.3f, 0.0f);
                break;
            default:
                Debug.Log("반동 : 잘못된 접근");
                break;
        }
    }

    /// <summary>
    /// 반동 설정
    /// </summary>
    public void SetResetRebound(float maxSpread, float currentSpread)
    {
        MaxSpread = maxSpread;
        Stablilty = 0;
        StartCurrentSpread = currentSpread;
        RestoreFromRecoilSpeed = 3.0f;
        CurrentSpread = currentSpread;
    }

    /// <summary>
    /// 건의 반동
    /// </summary>
    public void Rebound(GunData data)
    {
        Stablilty +=  0.01f; // 반동 
        CurrentSpread += data.m_sensitvity; //탄 퍼짐 추가
        CurrentSpread = Mathf.Clamp(CurrentSpread, 0.0f, MaxSpread); // 최대 퍼짐 막음

        // 줌에 따른 반동 변경
        // 줌 재수정
        switch (zoomState)
        {
            case ZOOM.NONE:
                _player.m_mainCine.m_YAxis.Value += Stablilty; 
                break;
            case ZOOM.ZOOM:
                _player.m_zoomCine.m_YAxis.Value += Stablilty;
                break;
            default:
                break;
        }
    }



    private void ActionEventListener(bool reload,bool fire)
    {
        if (reload) { if (Reload()) Debug.Log("Reload"); }

        if (fire) { Attack(); Attacking = true; }
        else { if(Attacking) Attacking = false; }
    }


    /// <summary>
    /// 무기에 따른 다양한 어택
    /// </summary>
    public void Attack()
    {
        if (_player.m_PlayerState == PLAYERSTATE.THROWING || _player.m_PlayerState == PLAYERSTATE.DINKING
            || _player.m_PlayerState == PLAYERSTATE.ATTACK) return;
        ITEMTYPE nowType = inven.NowEquip;
        switch (nowType)
        {
            case ITEMTYPE.MAIN:
            case ITEMTYPE.SUBMAIN:
            case ITEMTYPE.SUB:
                Shoot(nowType);
                break;
            case ITEMTYPE.MELEE:
                Swing(nowType);
                break;
            case ITEMTYPE.THROW:
                ReadyThrow(nowType);
                break;
            case ITEMTYPE.POTION:
                ReadyHeal(nowType);
                break;
            default:
                break;
        }
    }

    public void ReadyHeal(ITEMTYPE type)
    {
        if (_player.m_PlayerState == PLAYERSTATE.OPENINGLID) return;
        m_UseItemTime = 0.0f;
        _player.PlayerStateChange(PLAYERSTATE.OPENINGLID);
        //캔따는중 ui on
        _player.m_UIMgr.IsUseItem(true);
    }

    public void OpeningLid(float time)
    {
        m_UseItemTime += time;
        _player.m_UIMgr.TimeCheck(m_UseItemTime, 1.5f);
        if (m_UseItemTime >= 1.5f)
        {
            _player.PlayerStateChange(PLAYERSTATE.DINKING);
            aim.AttackAnimation(ITEMTYPE.POTION);
            _player.m_UIMgr.IsUseItem(false);
            StartCoroutine(Drinking());
        }
    }

    IEnumerator Drinking()
    {
        yield return new WaitForSeconds(3.0f);

        _player.PlayerStateChange(PLAYERSTATE.NONE);
        (inven.GetNowEquip() as Countable).ChangeAmount(-1);
        _player.Healing((inven.m_equip[inven.NowEquip] as HPDrink).HPDrinkData.m_healing);
        
        inven.CountableEquipCountCheck(ITEMTYPE.POTION);
    }

    public void ReadyThrow(ITEMTYPE type)
    {
        if (_player.m_PlayerState == PLAYERSTATE.THROWREADY) return;

        m_UseItemTime = 0.0f; //시간초기화
        _player.m_UIMgr.IsUseItem(true);
        ThrowObject throwObject = inven.m_equip[type] as ThrowObject;
        ThrowObjectData data = throwObject.Data as ThrowObjectData;

        //변수명 그랜덜 -> 오브젝트로 바꾸고, OBJYTPE 변수 하나 만들어서 들어온 타입마다 바뀌도록 수정

        grander = GameManager.Instance.FindObj(OBJTYPE.GRANDER) as ThrowingObject;

        _player.PlayerStateChange(PLAYERSTATE.THROWREADY);

    }

    public bool IsThrowObj(float time)
    {
        m_UseItemTime += time;
        _player.m_UIMgr.TimeCheck(m_UseItemTime, grander.MaxBombTimer - 1f);

        if (m_UseItemTime >= grander.MaxBombTimer) Attacking = false;

        if (Attacking && m_UseItemTime < grander.MaxBombTimer - 1f) return false;

        return true;
    }

    public void ThrowObj()
    {
        ITEMTYPE type = inven.NowEquip;

        _player.m_UIMgr.IsUseItem(false);
        aim.AttackAnimation(type);

        _player.PlayerStateChange(PLAYERSTATE.THROWING);

        StartCoroutine(LateAnimation(type, m_UseItemTime));

    }

    IEnumerator LateAnimation(ITEMTYPE type, float time)
    {
        yield return new WaitForSeconds(0.8f);
        _player.PlayerStateChange(PLAYERSTATE.NONE);
        grander.ThrowObject(_player.m_Ray.linePos, _player.m_mainCine.m_YAxis.Value, time);

        (inven.m_equip[type] as Countable).ChangeAmount(-1);
        inven.CountableEquipCountCheck(type);
    }

    public void Swing(ITEMTYPE type)
    {
        float time;
        if (_player.m_PlayerState == PLAYERSTATE.ATTACK) return;

        _player.PlayerStateChange(PLAYERSTATE.ATTACK);

        MeleeWeapon meleeWeapon = inven.m_equip[type] as MeleeWeapon;

        time = aim.AttackAnimation(type);

         StartCoroutine(MeleeAttack(meleeWeapon,time));
    }

    IEnumerator MeleeAttack(MeleeWeapon melleWeapon,float time)
    {
        float lastTime = Time.time + time;
       // melleWeapon.Attack();
       // while (Time.time < lastTime)
      //  {
            yield return new WaitForSeconds(time); // 공격할때

       // }
        //melleWeapon.Off();
        _player.PlayerStateChange(PLAYERSTATE.NONE);
    }

    public void Shoot(ITEMTYPE type)
    {
        Gun gun = inven.m_equip[type] as Gun;
        GunData data = gun.Data as GunData;

        Vector3 FirePos = gun.Shoot();

        if (FirePos == Vector3.zero) return;
        if (!_player.m_PlayerState.Equals(PLAYERSTATE.NONE)) return;

        if (gun.RemainAmmo <= 0)
        {
            _player.m_audioSource.PlayOneShot(GameManager.Instance.FindAudio(AUDIOCLIP.Gun_EMPTYFIRE));
            StartCoroutine(ShootDeley(0.2f));
            
            return;
        }
        
        float shootSpeed = 0.2f;
        
        StartCoroutine(ShootDeley(shootSpeed));
        gun.Shot();
        _player.Noise(data.m_Noise);

        StartCoroutine(FireBullet(gun,data, FirePos));
        Rebound(data);
        _player.m_audioSource.PlayOneShot(GameManager.Instance.FindAudio(data.fireAudio));
        inven.UpdateQuickAmmoUI(data.m_compatibleAmmo.id); //퀵슬롯 UI
        
    }


    /// <summary>
    /// 발사될 총알의 궤적
    /// </summary>
    public void BulletDirectionPoint()
    {
        RandomCoordinate.x = Random.Range(0.0f, CurrentSpread);
        RandomCoordinate.y = Random.Range(0.0f, CurrentSpread);

        DirectionPoint = new Vector3();
        DirectionPoint.z += RandomCoordinate.x;
        DirectionPoint.y += RandomCoordinate.y;
    }

    IEnumerator FireBullet(Gun gun,GunData data, Vector3 pos)
    {
       // float damage = data.m_damage;
        LookAim = _player.m_MainCamera;

        BulletDirectionPoint();

        Bullet bullet = GameManager.Instance.FindObj(OBJTYPE.BULLET) as Bullet;

        //bullet.SetBullet(damage, pos, 1,LookAim, DirectionPoint);

       // gun.fireEffect.Play();

        yield return new WaitForSeconds(1.5f);


        bullet.PutBullet(); // 오브젝트 off
        GameManager.Instance.m_GbjMgr.m_Objects[OBJTYPE.BULLET].AddObj(bullet);
    }

    IEnumerator ShootDeley(float waitTime)
    {
        _player.PlayerStateChange(PLAYERSTATE.ATTACK);

        yield return new WaitForSeconds(waitTime);

        _player.PlayerStateChange(PLAYERSTATE.NONE);
     }


    /// <summary>
    /// 퀵슬롯 변경
    /// </summary>
    private void ChangeQuickSlotEventListener(bool one, bool two, bool three, bool four, bool five, bool six)
    {
        if (_player.m_PlayerState == PLAYERSTATE.THROWING || _player.m_PlayerState == PLAYERSTATE.DINKING) return;

        if (one) SwapQuick(ITEMTYPE.MAIN);
        else if (two) SwapQuick(ITEMTYPE.SUBMAIN);
        else if (three) SwapQuick(ITEMTYPE.SUB);
        else if (four) SwapQuick(ITEMTYPE.MELEE);
        else if (five) SwapQuick(ITEMTYPE.THROW);
        else if (six) SwapQuick(ITEMTYPE.POTION);
        else return;
    }

    /// <summary>
    /// 퀵슬롯 스왑
    /// </summary>
    public void SwapQuick(ITEMTYPE QuickNum)
    {
        if (!inven.ChangeNowEquip(QuickNum)) return; // 무기 바꾸기 실패시 밑에 실행 x

        switch (_player.m_PlayerState)
        {
            case PLAYERSTATE.RELOAD:
                StopCoroutine(coroutine);
                ReloadCancleOrFinish();
                break;
           //case PLAYERSTATE.OPENINGLID:
           //    StopCoroutine(coroutine);
           //    IsFinishDrink();
           //    //캔슬
           //    break;
            default:
                break;
        }
    }

    public bool Reload()
    {
        ITEMTYPE nowEquipType = inven.NowEquip;
        if(nowEquipType != ITEMTYPE.NONE && nowEquipType < ITEMTYPE.MELEE)
        {
            Gun gun = inven.m_equip[nowEquipType] as Gun;
            GunData data = gun.Data as GunData;
            int gunMaxAmmo = data._maxLoder;
            int competibleAmmoId = data.m_compatibleAmmo.id;

            int InInvenCompetibleAmmo = inven.FindAmmo(competibleAmmoId); //인벤토리의 총알

            if (gun.RemainAmmo >= gunMaxAmmo || InInvenCompetibleAmmo <= 0 || _player.m_PlayerState == PLAYERSTATE.RELOAD) return false; //장전된 총알이 최대장전이랑 같거나 크고 or 인벤토리의 총알이 0이면 리턴

            _player.PlayerStateChange(PLAYERSTATE.RELOAD);
            int needAmmo = gunMaxAmmo - gun.RemainAmmo; //최대장전가능 수 - 현재 장전중인 수
            int canReloadAmmo = InInvenCompetibleAmmo - needAmmo <= 0 ? InInvenCompetibleAmmo : needAmmo;  // 인벤토리안의 총알 - 장전가능한 총알 수

            _player.m_UIMgr.ReloadingUI(true);
            aim.Reloading(true);//장전 애니메이션
            _player.m_audioSource.PlayOneShot(GameManager.Instance.FindAudio(data.reloadAudio));
            coroutine = StartCoroutine(Reloading(gun,canReloadAmmo,competibleAmmoId));
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 건속도랑 스킬속도 변경하기
    /// </summary>
    IEnumerator Reloading(Gun gun, int canReloadAmmo, int competibleAmmoId)
    {
        float reloadSpeed = (gun.Data as GunData).m_reloadSpeed;
        yield return new WaitForSeconds(aim.AnimationSpeed(reloadSpeed));

        gun.Reload(canReloadAmmo); //장전
        inven.ChangeCountableItemNumber(inven.FindSameItemInventoryIndex(competibleAmmoId, 0), -canReloadAmmo); //위에서 총알을 찾아서 있다는 가정하에 진행, 인벤에서 총알 빼기
        inven.UpdateQuickAmmoUI(competibleAmmoId);

        ReloadCancleOrFinish();
    }

    /// <summary>
    /// 장전이 되었거나 장전이 취소되었을 경우
    /// </summary>
    public void ReloadCancleOrFinish()
    {
        _player.m_UIMgr.ReloadingUI(false);
        aim.Reloading(false);
        _player.PlayerStateChange(PLAYERSTATE.NONE);
    }

    private void FixedUpdate()
    {
        if (CurrentSpread > (StartCurrentSpread + 0.0001)) // 0.001은 불필요한 업뎃을 줄이기 위한것
            CurrentSpread = Mathf.SmoothDamp(CurrentSpread, StartCurrentSpread, ref currentSpreadVelocity, RestoreFromRecoilSpeed);

        if (Stablilty > 0.0001f)
            Stablilty = Mathf.SmoothDamp(Stablilty, 0.0f, ref SpreadVelocity, RestoreFromRecoilSpeed);

        if(_player.m_PlayerState == PLAYERSTATE.THROWREADY)
        {
            if (IsThrowObj(Time.deltaTime))
            {
                _player.PlayerStateChange(PLAYERSTATE.NONE);
                ThrowObj();
            }
        }

        if(_player.m_PlayerState == PLAYERSTATE.OPENINGLID)
        {
            OpeningLid(Time.deltaTime);
        }
    }
}
*/