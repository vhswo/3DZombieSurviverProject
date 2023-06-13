using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerInput m_Input { get; private set; }
    public PlayerAnimation aim { get; private set; }
    public PlayerMove m_Move { get; private set; }
    //public PlayerAction m_action { get; private set; }
    public AudioSource m_audioSource { get; private set; }

    public PlayerInven playerInven;

    ActivateObjDetect acti;

    [SerializeField] PlayerUI UIMgr;
    [SerializeField] CinemachineFreeLook Cinecamera;
    [SerializeField] CinemachineFreeLook Zoomcamera;

    public Vector3 lastSafeZone;

    public Raycast m_Ray = null;
    public Transform m_MainCamera;
    public Rigidbody rigid;

    //노이즈
    [SerializeField] Noise noise;

    public PlayerData data;

    public CinemachineFreeLook m_mainCine => Cinecamera;
    public CinemachineFreeLook m_zoomCine => Zoomcamera;
    public PlayerUI m_UIMgr => UIMgr;

    public float m_MaxHP { get; private set; }
    public float m_HP { get; private set; }
    public float m_speed { get; private set; }
    public float m_exp { get; private set; }
    public float m_level { get; private set; }
    public bool m_IsDead { get; private set; }
    public PLAYERSTATE m_PlayerState { get; private set; }
    public int m_playerNum { get; private set; }
    public bool InHouse { get; private set; }  //하우스 체크, 하우스일 경우 노이즈 발생을 막는다
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    private void Start()
    {

        m_Input = GetComponent<PlayerInput>();
        aim = GetComponent<PlayerAnimation>();
        m_Move = GetComponent<PlayerMove>();
        m_audioSource = GetComponent<AudioSource>();
        m_MainCamera = Camera.main.transform;
        rigid = GetComponent<Rigidbody>();
        acti = GetComponent<ActivateObjDetect>();
        playerInven = GetComponent<PlayerInven>();

        m_Ray = new Raycast();

        m_Move.Init(this);
        m_Input.Init(this);
        aim.Init(this);
        m_Ray.Init(this);

        m_MaxHP = data._HPMAX;
        m_HP = m_MaxHP;
        m_speed = data._speed;
        m_level = data._level;
        m_exp = 0;
        m_IsDead = false;
        m_PlayerState = PLAYERSTATE.NONE;
        lastSafeZone = GameManager.Instance.savePoint[0].position;
        InHouse = true;

        m_UIMgr.Callinven();
        m_UIMgr.CallEquip();
    }

    public void PlayerInit(PlayerData data,int playerNum)
    {
    }

    private void OnDrawGizmos()
    {
        if(m_Ray != null)
        {
            for(int i = 0; i < m_Ray.linePos.Count; i++)
            {
                Gizmos.DrawSphere(m_Ray.linePos[i], 0.1f);
            }
        }
    }
    public void PlayerStateChange(PLAYERSTATE playerState)
    {
        m_PlayerState = playerState;
    }

    /// <summary>
    /// 걷거나,총쓰면 발동되는 소리
    /// </summary>
    /// <param name="noiseSize"></param>
    public void Noise(float noiseSize)
    {
        noise.SetNoise(noiseSize);
    }
    public bool IsDead() { return m_IsDead; }
    public float GetDamage(float damage, float height = 0)
    {
        if (m_IsDead) return m_HP;

        m_HP -= damage;
        m_UIMgr.HPUI(m_HP, m_MaxHP);

        if (m_HP <= 0)
        {
            m_IsDead = true;
            aim.Die();
            m_UIMgr.GameOverUI(true);
        }

        return m_HP;
    }

    /// <summary>
    /// GameOverUI에서 사용중
    /// </summary>
    public void ReStart()
    {
        m_UIMgr.GameOverUI(false);

        m_HP = m_MaxHP;
        m_UIMgr.HPUI(m_HP, m_MaxHP);

        m_IsDead = false;

        transform.position = lastSafeZone;

        aim.ReStart();
    }

    public void Healing(float heal)
    {
        m_HP += heal;
        m_HP = m_HP > 100 ? m_MaxHP: m_HP;
        m_UIMgr.HPUI(m_HP, m_MaxHP);
    }

    /// <summary>
    /// 이동 에 필요한 값 전달
    /// </summary>
    /// <param name="moveSpeed"></param>
    public void Animation(float x, float z)
    {
        aim.MoveMentAnimation(x,z);
    }

    /// <summary>
    /// 현재 이동상태변경
    /// </summary>
    /// <param name="moveState"></param>
    public void Animation(MoveMent moveState)
    {
        aim.SetPlayerMoveState(moveState);
    }

    public void InHousePlayer(bool Is)
    {
        InHouse = Is;
    }


    private void Update()
    {
        if (m_IsDead) return;

        m_Ray.ray(m_MainCamera, m_PlayerState);

        if(Input.GetKeyDown("i"))
        {
            m_UIMgr.Callinven();
        }

        if(Input.GetKeyDown("e"))
        {
            m_UIMgr.CallEquip();
        }

    }

}
