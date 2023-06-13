using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activate
{
    float StartAngleY; //시작 문의 시작 앵글y값
    float MoveAngleY;  //현재 문의 y값
    float MaxY;        // 변경되야할 y값

    float pos;          // 문의 x or z 값
    float targetPos;    // 문과 비교될 타겟의 x or z 값

    float limit;        //최대오차범위

    float DoorMoveVelocity;        
    bool IsOpenning;        // 현재 문의 상태

    private void Awake()
    {
        StartAngleY = transform.rotation.eulerAngles.y;
        MoveAngleY = StartAngleY;
        DoorMoveVelocity = 0f;

        SetObjState("열기");

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
        MaxY = StartAngleY; // 원래 값으로 돌리기

        SetObjState("열기");
    }

    /// <summary>
    /// 문을 민 방향을 계산하여 문이 어디로 열리지를 생각한다
    /// </summary>
    /// <param name="Pushdir"></param>
    public void OpenTheDoor(Vector3 Pushdir)
    {
        if (StartAngleY == 90) targetPos = Pushdir.x;
        else targetPos = Pushdir.z;

        //플레이어가 민 방향으로 문이 열리기
        if (pos > targetPos)  MaxY = StartAngleY + 90f;
        else  MaxY = StartAngleY - 90f;

        SetObjState("닫기");
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
