using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{

    public float ViewRadius;  // 소리나 시야의 각
    public float ViewAngle;  //최대 범위
    public LayerMask TargetMask;   // 따라갈 타겟
    public LayerMask ObstacleMask;  // 방해물

    public FieldOfView()
    {
        ViewRadius = 4.0f;
        ViewAngle = 120.0f;
        TargetMask = LayerMask.GetMask("Player");
        ObstacleMask = LayerMask.GetMask("Wall");
    }

    /// <summary>
    /// 라디안값으로 바꿔주기
    /// </summary>
    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    /// <summary>
    /// 플레이어가 범위 내에있는지 체크
    /// </summary>
    public Transform CheckPlayer(Transform transform)
    {
        Vector3 Pos = transform.position + Vector3.up * 0.5f;

        //오일러 대체할거 찾기 있으면 트랜스폼 매개변수 리지드로 변경
        float NowViewAngle = transform.eulerAngles.y; // 현재 오브젝트가 바라보는 각도
        float half = ViewAngle * 0.5f;
        Vector3 lookDir = transform.forward;//AngleToDir(NowViewAngle);
        Vector3 RightlookDir = AngleToDir(NowViewAngle + half);
        Vector3 LeftlookDir = AngleToDir(NowViewAngle - half);
        Collider[] Targets = Physics.OverlapSphere(Pos, ViewRadius, TargetMask);

        if (Targets.Length == 0) return null;
        foreach (Collider EnemyColli in Targets) // 혹시나 나중에 플레이어가 여럿생기거나 AI 동료를 넣었을경우 를 대비해 전체 체크
        {

            Vector3 targetPos = EnemyColli.transform.position; // 원안에 있던 콜리더
            Vector3 targetDir = (targetPos - Pos).normalized;  // 타켓이 내쪽으로 오니 타켓에서 포스를 빼고 노멀라이즈
            float targetAngle = Vector3.Angle(lookDir, targetDir);

            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(Pos, targetDir, ViewRadius, ObstacleMask)) //중간에 장애물이 잇을경우 못봄
            {
                return EnemyColli.transform;
            }
        }

        return null;
    }
}

