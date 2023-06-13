using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//activate는 인터페이스로 하는게 어떤지

public class Detecting : MonoBehaviour
{
    //예를 들면, 아이템 감지, 적 감지, 기능(문,차소리내기) 감지,소리 감지
    //아이템은 전부
    //기능, 적 은 하나

    [SerializeField] LayerMask detectTarget; //감지할 타겟 설정
    [SerializeField] float detectRange;     // 감지할 범위

    public Collider[] Detect() // 픽스드업데이트로
    {
        Vector3 Pos = transform.position + Vector3.up * 0.5f;

        Collider[] Targets = Physics.OverlapSphere(Pos, detectRange, detectTarget);

        if (Targets.Length == 0) return null;

        return Targets;
    }
}
