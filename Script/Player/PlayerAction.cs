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

    [Header("��� ����")]
    [SerializeField] GameObject ShotHole; //ź��
    private float MaxSpread; //�ִ� ź ����
    private float Stablilty; //������
    private float RestoreFromRecoilSpeed; // ���� ź ������ �⺻���� ���ƿ��� �ӵ�
    private float CurrentSpread; // ���� ź ���� ����
    private float StartCurrentSpread; // ���� ź ���� ����
    private float currentSpreadVelocity; //�ǽð� ź ���� ���� ����
    private float SpreadVelocity; //�ǽð� ź ���� ���� ����
    Coroutine coroutine;

    [Header("ź ����")]
    private Transform LookAim;
    private Vector3 DirectionPoint;
    private Vector2 RandomCoordinate; //x ,z        //���� ��ǥ

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
    /// �ݵ��ʱ�ȭ
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
                Debug.Log("�ݵ� : �߸��� ����");
                break;
        }
    }

    /// <summary>
    /// �ݵ� ����
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
    /// ���� �ݵ�
    /// </summary>
    public void Rebound(GunData data)
    {
        Stablilty +=  0.01f; // �ݵ� 
        CurrentSpread += data.m_sensitvity; //ź ���� �߰�
        CurrentSpread = Mathf.Clamp(CurrentSpread, 0.0f, MaxSpread); // �ִ� ���� ����

        // �ܿ� ���� �ݵ� ����
        // �� �����
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
    /// ���⿡ ���� �پ��� ����
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
        //ĵ������ ui on
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

        m_UseItemTime = 0.0f; //�ð��ʱ�ȭ
        _player.m_UIMgr.IsUseItem(true);
        ThrowObject throwObject = inven.m_equip[type] as ThrowObject;
        ThrowObjectData data = throwObject.Data as ThrowObjectData;

        //������ �׷��� -> ������Ʈ�� �ٲٰ�, OBJYTPE ���� �ϳ� ���� ���� Ÿ�Ը��� �ٲ�� ����

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
            yield return new WaitForSeconds(time); // �����Ҷ�

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
        inven.UpdateQuickAmmoUI(data.m_compatibleAmmo.id); //������ UI
        
    }


    /// <summary>
    /// �߻�� �Ѿ��� ����
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


        bullet.PutBullet(); // ������Ʈ off
        GameManager.Instance.m_GbjMgr.m_Objects[OBJTYPE.BULLET].AddObj(bullet);
    }

    IEnumerator ShootDeley(float waitTime)
    {
        _player.PlayerStateChange(PLAYERSTATE.ATTACK);

        yield return new WaitForSeconds(waitTime);

        _player.PlayerStateChange(PLAYERSTATE.NONE);
     }


    /// <summary>
    /// ������ ����
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
    /// ������ ����
    /// </summary>
    public void SwapQuick(ITEMTYPE QuickNum)
    {
        if (!inven.ChangeNowEquip(QuickNum)) return; // ���� �ٲٱ� ���н� �ؿ� ���� x

        switch (_player.m_PlayerState)
        {
            case PLAYERSTATE.RELOAD:
                StopCoroutine(coroutine);
                ReloadCancleOrFinish();
                break;
           //case PLAYERSTATE.OPENINGLID:
           //    StopCoroutine(coroutine);
           //    IsFinishDrink();
           //    //ĵ��
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

            int InInvenCompetibleAmmo = inven.FindAmmo(competibleAmmoId); //�κ��丮�� �Ѿ�

            if (gun.RemainAmmo >= gunMaxAmmo || InInvenCompetibleAmmo <= 0 || _player.m_PlayerState == PLAYERSTATE.RELOAD) return false; //������ �Ѿ��� �ִ������̶� ���ų� ũ�� or �κ��丮�� �Ѿ��� 0�̸� ����

            _player.PlayerStateChange(PLAYERSTATE.RELOAD);
            int needAmmo = gunMaxAmmo - gun.RemainAmmo; //�ִ��������� �� - ���� �������� ��
            int canReloadAmmo = InInvenCompetibleAmmo - needAmmo <= 0 ? InInvenCompetibleAmmo : needAmmo;  // �κ��丮���� �Ѿ� - ���������� �Ѿ� ��

            _player.m_UIMgr.ReloadingUI(true);
            aim.Reloading(true);//���� �ִϸ��̼�
            _player.m_audioSource.PlayOneShot(GameManager.Instance.FindAudio(data.reloadAudio));
            coroutine = StartCoroutine(Reloading(gun,canReloadAmmo,competibleAmmoId));
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// �Ǽӵ��� ��ų�ӵ� �����ϱ�
    /// </summary>
    IEnumerator Reloading(Gun gun, int canReloadAmmo, int competibleAmmoId)
    {
        float reloadSpeed = (gun.Data as GunData).m_reloadSpeed;
        yield return new WaitForSeconds(aim.AnimationSpeed(reloadSpeed));

        gun.Reload(canReloadAmmo); //����
        inven.ChangeCountableItemNumber(inven.FindSameItemInventoryIndex(competibleAmmoId, 0), -canReloadAmmo); //������ �Ѿ��� ã�Ƽ� �ִٴ� �����Ͽ� ����, �κ����� �Ѿ� ����
        inven.UpdateQuickAmmoUI(competibleAmmoId);

        ReloadCancleOrFinish();
    }

    /// <summary>
    /// ������ �Ǿ��ų� ������ ��ҵǾ��� ���
    /// </summary>
    public void ReloadCancleOrFinish()
    {
        _player.m_UIMgr.ReloadingUI(false);
        aim.Reloading(false);
        _player.PlayerStateChange(PLAYERSTATE.NONE);
    }

    private void FixedUpdate()
    {
        if (CurrentSpread > (StartCurrentSpread + 0.0001)) // 0.001�� ���ʿ��� ������ ���̱� ���Ѱ�
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