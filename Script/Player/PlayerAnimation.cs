using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player _player;
    private Animator anim;

    Dictionary<string, int> AnimationKey = new();

    /// <summary>
    /// 애니메이션 키를 만들어준다
    /// </summary>
    public void KeySetting(string anikey)
    {
        AnimationKey.Add(anikey,anikey.GetHashCode());
    }


    public void FloatAni(string hash,float value)
    {
        if (!AnimationKey.ContainsKey(hash)) KeySetting(hash);

        anim.SetFloat(AnimationKey[hash], value);
    }

    public void BoolAni(string hash, bool value)
    {
        if (!AnimationKey.ContainsKey(hash)) KeySetting(hash);

        anim.SetBool(AnimationKey[hash], value);
    }

    public void IntAni(string hash, int value)
    {
        if (!AnimationKey.ContainsKey(hash)) KeySetting(hash);

        anim.SetInteger(AnimationKey[hash], value);
    }

    public void TriggerAni(string hash)
    {
        if (!AnimationKey.ContainsKey(hash)) KeySetting(hash);

        anim.SetTrigger(AnimationKey[hash]);
    }














    public void Init(Player player)
    {
        anim = GetComponent<Animator>();
        _player = player;
    }


    /// <summary>
    /// 스피드 : 총의 장전 속도 <br/>
    /// 스킬   : 스킬로 속도를 조절할수있다 최소 0.5 최대 1 이다<br/>
    /// 공식   : 애니메이션 속도 -> 2 - 스킬속도 로 항상 1 ~ 1.5 를 나타낸다<br/>
    ///          장전속도        -> 총의 스피드 와 스킬속도를 곱한다 스피드 * 0.5 ~ 1
    /// </summary>
    public float AnimationSpeed(float speed, float skill = 1)
    {
        float resultSpeed = 2.0f - skill;
        anim.SetFloat("ReloadSpeed", resultSpeed);

        return resultSpeed * speed;
    }

    public void SetPlayerMoveState(MoveMent moveState)
    {
        switch (moveState)
        {
            case MoveMent.Run:
                anim.SetTrigger("Run");
                break;
            case MoveMent.Crounch:
                anim.SetTrigger("Crouch");
                break;
            case MoveMent.Walk:
                anim.SetTrigger("Walk");
                break;
            default:
                break;
        }
    }
    public void MoveMentAnimation(float x, float z)
    {
        anim.SetFloat("LeftRight", x);
        anim.SetFloat("UpDown", z);
    }

    public void JumpAnimation()
    {
        anim.SetTrigger("Jump");
    }
    public void Reloading(bool IsOn)
    {
        anim.SetBool("RifleReload",IsOn);
    }

    public void QuickAnimation(ITEMTYPE index)
    {
        //애니메이션 넣을지 고민중
        switch(index)
        {
            case ITEMTYPE.MAIN:
            case ITEMTYPE.SUBMAIN:
                anim.SetTrigger("Rifle");
                break;
            case ITEMTYPE.SUB:
                anim.SetTrigger("Gun"); // 피스톨로 바꾸기
                break;
            default:
                anim.SetTrigger("Peace");
                break;
        }
    }

    /// <summary>
    /// 애니메이션 공격 관련작동 및 애니메이션 총 플레이타임을 가져다 준다
    /// </summary>
    public float AttackAnimation(ITEMTYPE index)
    {
        RuntimeAnimatorController Getruntime;
        Getruntime = anim.runtimeAnimatorController;
        string nowAni ="";
        switch (index)
        {
            case ITEMTYPE.MAIN:
            case ITEMTYPE.SUBMAIN:
                anim.SetTrigger("RifleAttack");
                break;
            case ITEMTYPE.SUB:
                anim.SetTrigger("PistolAttack");
                break;
            case ITEMTYPE.MELEE:
                nowAni = "MeleeAttack";
                anim.SetTrigger("MeleeAttack");
                break;
            case ITEMTYPE.THROW:
                anim.SetTrigger("Throw");
                break;
            case ITEMTYPE.POTION:
                anim.SetTrigger("Drink");
                break;
            default:
                anim.SetTrigger("Peace");
                break;
        }

        for (int i = 0; i < Getruntime.animationClips.Length; i++)
        {
            if (Getruntime.animationClips[i].name == nowAni)
            {
                return Getruntime.animationClips[i].length;
            }
        }
        return 0;
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }

    public void ReStart()
    {
        anim.SetTrigger("Resurrection");
    }

}
