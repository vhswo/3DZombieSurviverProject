using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{

    public float ViewRadius;  // �Ҹ��� �þ��� ��
    public float ViewAngle;  //�ִ� ����
    public LayerMask TargetMask;   // ���� Ÿ��
    public LayerMask ObstacleMask;  // ���ع�

    public FieldOfView()
    {
        ViewRadius = 4.0f;
        ViewAngle = 120.0f;
        TargetMask = LayerMask.GetMask("Player");
        ObstacleMask = LayerMask.GetMask("Wall");
    }

    /// <summary>
    /// ���Ȱ����� �ٲ��ֱ�
    /// </summary>
    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    /// <summary>
    /// �÷��̾ ���� �����ִ��� üũ
    /// </summary>
    public Transform CheckPlayer(Transform transform)
    {
        Vector3 Pos = transform.position + Vector3.up * 0.5f;

        //���Ϸ� ��ü�Ұ� ã�� ������ Ʈ������ �Ű����� ������� ����
        float NowViewAngle = transform.eulerAngles.y; // ���� ������Ʈ�� �ٶ󺸴� ����
        float half = ViewAngle * 0.5f;
        Vector3 lookDir = transform.forward;//AngleToDir(NowViewAngle);
        Vector3 RightlookDir = AngleToDir(NowViewAngle + half);
        Vector3 LeftlookDir = AngleToDir(NowViewAngle - half);
        Collider[] Targets = Physics.OverlapSphere(Pos, ViewRadius, TargetMask);

        if (Targets.Length == 0) return null;
        foreach (Collider EnemyColli in Targets) // Ȥ�ó� ���߿� �÷��̾ ��������ų� AI ���Ḧ �־������ �� ����� ��ü üũ
        {

            Vector3 targetPos = EnemyColli.transform.position; // ���ȿ� �ִ� �ݸ���
            Vector3 targetDir = (targetPos - Pos).normalized;  // Ÿ���� �������� ���� Ÿ�Ͽ��� ������ ���� ��ֶ�����
            float targetAngle = Vector3.Angle(lookDir, targetDir);

            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(Pos, targetDir, ViewRadius, ObstacleMask)) //�߰��� ��ֹ��� ������� ����
            {
                return EnemyColli.transform;
            }
        }

        return null;
    }
}

