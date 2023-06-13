using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activate
{
    float StartAngleY; //���� ���� ���� �ޱ�y��
    float MoveAngleY;  //���� ���� y��
    float MaxY;        // ����Ǿ��� y��

    float pos;          // ���� x or z ��
    float targetPos;    // ���� �񱳵� Ÿ���� x or z ��

    float limit;        //�ִ��������

    float DoorMoveVelocity;        
    bool IsOpenning;        // ���� ���� ����

    private void Awake()
    {
        StartAngleY = transform.rotation.eulerAngles.y;
        MoveAngleY = StartAngleY;
        DoorMoveVelocity = 0f;

        SetObjState("����");

        if (StartAngleY == 90) pos = transform.position.x;
        else pos = transform.position.z;

        IsOpenning = false;
    }

    public override void activate(Vector3 Pushdir)
    {
        if (IsOpenning) return;
        
        if(StartAngleY != MoveAngleY)
        {
            CloseDoor();
        }
        else
        {
            OpenTheDoor(Pushdir);
        }

        StartCoroutine(MoveDoor());
    }

    public void CloseDoor()
    {
        MaxY = StartAngleY; // ���� ������ ������

        SetObjState("����");
    }

    /// <summary>
    /// ���� �� ������ ����Ͽ� ���� ���� �������� �����Ѵ�
    /// </summary>
    /// <param name="Pushdir"></param>
    public void OpenTheDoor(Vector3 Pushdir)
    {
        if (StartAngleY == 90) targetPos = Pushdir.x;
        else targetPos = Pushdir.z;

        //�÷��̾ �� �������� ���� ������
        if (pos > targetPos)  MaxY = StartAngleY + 90f;
        else  MaxY = StartAngleY - 90f;

        SetObjState("�ݱ�");
    }

    IEnumerator MoveDoor()
    {
        IsOpenning = true;

        while (true)
        {
            limit = MaxY - MoveAngleY;

            if (Mathf.Abs(limit) < 1)
            {
                IsOpenning = false;
                MoveAngleY = MaxY;
            }
            else
            {
                MoveAngleY = Mathf.SmoothDamp(MoveAngleY, MaxY, ref DoorMoveVelocity, 0.1f);
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, MoveAngleY, 0));

            if (IsOpenning) yield return new WaitForSeconds(0.1f);
            else yield break;

        }

    }
}
