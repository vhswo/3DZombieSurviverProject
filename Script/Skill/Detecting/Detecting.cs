using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//activate�� �������̽��� �ϴ°� ���

public class Detecting : MonoBehaviour
{
    //���� ���, ������ ����, �� ����, ���(��,���Ҹ�����) ����,�Ҹ� ����
    //�������� ����
    //���, �� �� �ϳ�

    [SerializeField] LayerMask detectTarget; //������ Ÿ�� ����
    [SerializeField] float detectRange;     // ������ ����

    public Collider[] Detect() // �Ƚ��������Ʈ��
    {
        Vector3 Pos = transform.position + Vector3.up * 0.5f;

        Collider[] Targets = Physics.OverlapSphere(Pos, detectRange, detectTarget);

        if (Targets.Length == 0) return null;

        return Targets;
    }
}
