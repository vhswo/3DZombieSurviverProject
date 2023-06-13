using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMent
{
    None,
    Walk,
    Run,
    Crounch
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float JumpPower;

    [SerializeField] AudioClip WalkAudio;
    [SerializeField] AudioClip RunAudio;

    private Rigidbody rigid;
    private Player _player;

    public float AboutMoveNoise;

    [SerializeField] float speed = 3f;
    float run = 5f;
    private bool IsGround;
    private Vector3 moveVec;
    private MoveMent playerMoveState;
    private MoveMent ExplayerMoveState;
    private float nowStateSpeed;

    Animator ani;

    public void Init(Player player)
    {
        IsGround = true;
        FromPlayerMoveStateToChageSpeed();
        JumpPower = 5f;

        _player = player;
        rigid = player.rigid;
        _player.m_Input.MoveEventListener += BaseMoveEventListener;

        GetPositon = transform.position;
        ani = GetComponent<Animator>();

    }

    /// <summary>
    ///  플레이어이동상태에 따른 스피드 변화
    /// </summary>
    public void FromPlayerMoveStateToChageSpeed()
    {
        if (ExplayerMoveState == playerMoveState) return;

        ExplayerMoveState = playerMoveState;

        switch (playerMoveState)
        {
            case MoveMent.Walk:
                nowStateSpeed = 0.4f;
                AboutMoveNoise = 3f;
                break;
            case MoveMent.Run:
                nowStateSpeed = 1.0f;
                AboutMoveNoise = 4f;
                break;
            case MoveMent.Crounch:
                nowStateSpeed = 0.3f;
                AboutMoveNoise = 0f;
                break;
            default:
                break;
        }

        _player.Animation(playerMoveState);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            IsGround = false;
        }
    }

    Vector3 GetPositon;
    Quaternion lastRo;
    
    private void FixedUpdate()
    {
        if (_player.IsDead()) return;
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 forwad = _player.m_MainCamera.transform.TransformDirection(Vector3.forward);
        forwad.y = 0f;
        forwad = forwad.normalized;

        Vector3 right = new Vector3(forwad.z, 0.0f, -forwad.x);
        Vector3 myDir = forwad * vertical  + right * horizontal;

        GetPositon = transform.position + myDir * 2f * Time.deltaTime;

          Quaternion ro = Quaternion.LookRotation(forwad);
          lastRo = ro;
          lastRo = Quaternion.Slerp(rigid.rotation, lastRo, 0.06f);
          rigid.MoveRotation(lastRo);
        rigid.MovePosition(GetPositon);
        _player.Animation(horizontal, vertical);

    }

    public bool CheckGround()
    {
        if(Physics.SphereCast(transform.position,0.2f,Vector3.down,out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.GetMask("Ground")) return true; 
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position , 0.2f);

       //Gizmos.color = Color.red;
       //Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 500f);
    }

    private void BaseMoveEventListener(float updown, float leftRight,bool run, bool crouch,bool jump)
    {
    }


}

