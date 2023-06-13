using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] string LeftRightName = "Horizontal";
    [SerializeField] string UpDownName = "Vertical";
    [SerializeField] string CrouchName = "Crouch";
    [SerializeField] string JumpName = "Jump";
    [SerializeField] string RunName = "Run";

    [SerializeField] string CameraLeftRightName = "Mouse X";
    [SerializeField] string CameraUpDownName = "Mouse Y";
    [SerializeField] string GetItemKeyName = "GetItem";
    [SerializeField] string ShowQuickName = "ShowQuick";
    [SerializeField] string ActivateName = "activate";
    public bool Test { get; private set; }
    public event System.Action<bool> TestListener;

    public float UpDown { get; private set; }
    public float LeftRight { get; private set; }
    public float CameraUpDown { get; private set; }
    public float CameraLeftRight { get; private set; }
    public bool Jump { get; private set; }
    public bool Run { get; private set; }
    public bool Crouch { get; private set; }
    public bool GetItem { get; private set; }
    public bool ShowQuick { get; private set; }
    public bool Activate { get; private set; }

    public event System.Action<float, float,bool,bool,bool> MoveEventListener;
    public event System.Action<bool,bool> ActionEventListener;
    public event System.Action<bool,bool>UIKeyEventListener;
    public event System.Action<bool,bool,bool,bool,bool,bool> QuickSlotChangeEventListener;

    Player _player;
    public void Init(Player player)
    {
        _player = player;  
    }

    void Update()
    {
        if (_player.m_IsDead) return;
        LeftRight   = Input.GetAxis(LeftRightName);
        UpDown      = Input.GetAxis(UpDownName);
        Crouch      = Input.GetButtonDown(CrouchName);
        Jump        = Input.GetButtonDown(JumpName);
        Run         = Input.GetButton(RunName);

        CameraUpDown = Input.GetAxis(CameraUpDownName);
        CameraLeftRight = Input.GetAxis(CameraLeftRightName);
        GetItem = Input.GetButtonDown(GetItemKeyName);
        ShowQuick = Input.GetButtonDown(ShowQuickName);
        Activate = Input.GetButtonDown(ActivateName);

        MoveEventListener?.Invoke(UpDown, LeftRight,Run,Crouch,Jump);


        TestListener?.Invoke(Test);
    }
}
