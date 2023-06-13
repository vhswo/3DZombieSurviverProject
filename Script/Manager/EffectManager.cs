using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName
{
    GunFlash,
    BulletHit,
    GrenadaExplosive,
    Blood,
    None,
}

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_instance;
    public static EffectManager instance => m_instance;

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;

            if(transform.root != null)
            {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }

            Init();
        }
    }

    Dictionary<EffectName,ObjectPooling<GameObject>> Effect = new();
    GameObject file;

    private void Init()
    {
        file = new GameObject("Effects");
        file.transform.SetParent(transform);

        string path = "Effects/";

        for(EffectName i = EffectName.GunFlash; i < EffectName.None; i++)
        {
            GameObject FindEffect = Resources.Load(path + i.ToString()) as GameObject;
            FindEffect = Instantiate(FindEffect, file.transform);
            Effect.Add(i, new());
            Effect[i].Add(FindEffect);
            FindEffect.SetActive(false);
        }
    }

    public GameObject Copy(EffectName name,GameObject effect)
    {
        GameObject copy = Instantiate(effect, file.transform);
        copy.SetActive(false);

        return copy;
    }

    public void EffectShot(Vector3 pos,Vector3 normal,EffectName name)
    {
        if (Effect[name].GetCount() <= 1) Effect[name].Add(Copy(name,Effect[name].ReferenceT()));

        GameObject effect = Effect[name].Use();
        effect.transform.position = pos;
        effect.transform.rotation = Quaternion.Euler(normal);

        effect.SetActive(true);

        StartCoroutine(OverPoolingCoolime(name,effect));
    }


    IEnumerator OverPoolingCoolime(EffectName name,GameObject effect)
    {
        ParticleSystem particle = effect.GetComponentInChildren<ParticleSystem>();
        while(particle.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        effect.SetActive(false);
        Effect[name].Add(effect);
    }
    
    public void action()
    { 
    }
}
