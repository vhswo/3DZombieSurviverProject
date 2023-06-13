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

    //게임매니저 전체적으로 수정
    //다른 매니저를 통해 받는게 아닌 게임 매니저를 통해 다른 매니저에 접근


    //        //
    // 사 운 드 //
    //        //
    public AudioClip FindAudio(AUDIOCLIP audio)
    {
        return AudioMgr.audioes[(int)audio];
    }

    //          //
    // 이 펙 트 //
    //          //
   // public ParticleSystem FindParticle(EFFECTTYPE type, Transform parant)
   // {
   //     return EffectMgr.GetEffectPrefab(type, parant);
   // }

    //          //
    // 오브젝트 //
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
