using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            return;
        }
        Destroy(gameObject);
    }

    public SpriteUIManager m_imgMgrUI;

    [SerializeField] GameObjectManager GbjMgr;

    [SerializeField] Respawn RandomLocation;
    public Transform[] savePoint;

    public GameObjectManager m_GbjMgr => GbjMgr;
    public AudioManager AudioMgr { get; private set; }
    public EffectManager EffectMgr { get; private set; }


    private void Start()
    {
        AudioMgr    = GetComponent<AudioManager>();
        EffectMgr   = GetComponent<EffectManager>();

        m_imgMgrUI = GetComponent<SpriteUIManager>();

        GbjMgr.init();
        m_imgMgrUI.init();
    }

    //���ӸŴ��� ��ü������ ����
    //�ٸ� �Ŵ����� ���� �޴°� �ƴ� ���� �Ŵ����� ���� �ٸ� �Ŵ����� ����


    //        //
    // �� �� �� //
    //        //
    public AudioClip FindAudio(AUDIOCLIP audio)
    {
        return AudioMgr.audioes[(int)audio];
    }

    //          //
    // �� �� Ʈ //
    //          //
   // public ParticleSystem FindParticle(EFFECTTYPE type, Transform parant)
   // {
   //     return EffectMgr.GetEffectPrefab(type, parant);
   // }

    //          //
    // ������Ʈ //
    //          //
    //public InGameObjects FindObj(OBJTYPE obj)
    //{
    //    InGameObjects _obj;
    //
    //    do {
    //         _obj = m_GbjMgr.m_Objects[obj].GetObj();
    //
    //        if (_obj == null)  m_GbjMgr.CreateObj(obj);
    //        else break;
    //
    //    } while (true);
    //    
    //    return _obj;
    //}

}
