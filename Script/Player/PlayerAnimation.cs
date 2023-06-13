using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Player _player;
    private Animator anim;

    Dictionary<string, int> AnimationKey = new();

    /// <summary>
    /// �ִϸ��̼� Ű�� ������ش�
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
    /// ���ǵ� : ���� ���� �ӵ� <br/>
    /// ��ų   : ��ų�� �ӵ��� �����Ҽ��ִ� �ּ� 0.5 �ִ� 1 �̴�<br/>
    /// ����   : �ִϸ��̼� �ӵ� -> 2 - ��ų�ӵ� �� �׻� 1 ~ 1.5 �� ��Ÿ����<br/>
    ///          �����ӵ�        -> ���� ���ǵ� �� ��ų�ӵ��� ���Ѵ� ���ǵ� * 0.5 ~ 1
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
        //�ִϸ��̼� ������ �����
        switch(index)
        {
            case ITEMTYPE.MAIN:
            case ITEMTYPE.SUBMAIN:
                anim.SetTrigger("Rifle");
                break;
            case ITEMTYPE.SUB:
                anim.SetTrigger("Gun"); // �ǽ���� �ٲٱ�
                break;
            default:
                anim.SetTrigger("Peace");
                break;
        }
    }

    /// <summary>
    /// �ִϸ��̼� ���� �����۵� �� �ִϸ��̼� �� �÷���Ÿ���� ������ �ش�
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
