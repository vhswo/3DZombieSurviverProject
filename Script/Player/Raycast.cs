using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

//시네머신의 트랜스 좌표, y 값 테스트
public class Raycast
{
    private  RaycastHit hit;
    private float Maxdistance;

    private Player _player;
    //ui 로 보내주기
   //private Text ItemName;
   //private Image ItemTagImage;

    public LayerMask layerMask;


    public void Init(Player player)
    {
        _player = player;

        Maxdistance = 14f;

        layerMask = ~LayerMask.GetMask("Player");
    }

    public void ray(Transform Camera, PLAYERSTATE state)
    {
        switch (state)
        {
            case PLAYERSTATE.THROWREADY:
               // ThrowRangeRay(Camera);
                break;
            default:
                ObjCheckRay(Camera);
                break;
        }
    }

    public List<Vector3> linePos = new();

/*    public void ThrowRangeRay(Transform Camera)
    {
        Vector3 dir = Camera.forward;
        dir.y = 0;
        dir.Normalize();

        Vector3 start = Camera.position + ((Camera.forward - new Vector3(0,Camera.forward.y,0)) * 3f);
        
        Vector3 target = start + dir * Maxdistance * _player.m_mainCine.m_YAxis.Value;
        Vector3 Maxdis = target;

        //Debug.DrawRay(start, target, Color.red);
        if (Physics.Raycast(start, dir, out hit, Maxdistance, layerMask))
        {
            target = hit.point;
        }

        if (target == Maxdis)
        {
            //Debug.DrawRay(target, target + Vector3.down * Maxdistance, Color.blue);
            if (Physics.Raycast(target,Vector3.down,out hit, Maxdistance,layerMask))
            {
                target = hit.point;
            }
        }

        linePos.Clear();
        float interval = 1/10f;
        for(int i = 0; i< 10; i++)
        {
            Vector3 point =  Vector3.Slerp(start, target, i * interval);
            linePos.Add(point);
        }
    }
*/

    public void ResetAim()
    {
        _player.m_UIMgr.SetRayHitUI(false);
        _player.m_UIMgr.CaughtOnAim(string.Empty);
    }

    public void ObjCheckRay(Transform Camera)
    {
        bool OnTarget = true;

/*
        if(nowRayTag != exRayTag)
        {
            //오브젝트의 정보를 들고 오는 레이가 변경됬음을 감지

            switch (exRayTag)
            {
                case "Item":
                    _player.m_UIMgr.SetRayHitUI(false); 
                    _player.m_UIMgr.CaughtOnAim("Baisc");
                    break;
                case "Enemy":
                    break;
                case "CanNoiseObj":
                    break;
                case "ActivateObj":
                    break;
                default:
                    break;
            }

            exRayTag = nowRayTag;
            //현재 켜져있는 레이의 정보를 체크 (레이가 벗어난 걸 감지)
        }
*/
        //Debug.DrawLine(Camera.position, Camera.position + Camera.forward * Maxdistance, Color.blue);
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, Maxdistance, layerMask))
        {
            GameObject hitObj = hit.collider.gameObject;

            string objTag = hitObj.tag;
            switch (objTag)
            {
                case "Item":
                    //ui는 ui매니저로 보내고, 키는 활성화로 하기 키 관리는 따로 
                    _player.m_UIMgr.SetRayHitUI(true, hitObj.name);
                    _player.m_UIMgr.CaughtOnAim("Item");
                    if (_player.m_Input.GetItem) //f 키를 누름
                    {
                        if (hitObj.TryGetComponent(out AddToItem data))
                        {
                            _player.playerInven.GetItem(data);
                        }
                    }
                    return;
                case "Enemy":
                    _player.m_UIMgr.CaughtOnAim("Enemy");
                    return;
                case "CanNoiseObj":
                    //현재 사용안함 나중에 사용할때 전체적인 수정
                    _player.m_UIMgr.SetRayHitUI(true, hitObj.name);
                    if (_player.m_Input.GetItem) //f 키를 누름
                    {
                        if (hitObj.TryGetComponent(out Car car))
                        {
                            car.CallNoise();
                        }
                    }
                    return;
                case "ActivateObj":
                    //물체 트리거에 온오프 달고, 트리거에서 온오프만 정해준다 enter은 온, exit는 오프
                    //온일때만 여기 들어올수 있다

                    if (hitObj.TryGetComponent(out Activate act))
                    {
                        _player.m_UIMgr.ActivateKeyUIActive(true);
                        _player.m_UIMgr.ActivateKeyTextUI(act.ObjState);
                        if (_player.m_Input.Activate)
                        {
                            _player.m_UIMgr.ActivateKeyUIActive(false);
                            //act.activate();
                        }
                    }
                    return;
                default:
                    OnTarget = false;
                    break;
            }
        }
        else
        {
            OnTarget = false;
        }

        if(OnTarget == false)
        {
            ResetAim();
        }

    }
}

