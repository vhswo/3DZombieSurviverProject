using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AUDIOCLIP
{
    Rifle_FIRE = 0,
    Rifle_RELOAD,
    Pistol_FIRE,
    Pistol_RELOAD,
    Gun_EMPTYFIRE,
    Car_ALRAM,
    Player_MOVE,
    Player_RUN,
    Zombie_IDLE,
    Zombie_RUN,
    END
}

public class AudioManager : MonoBehaviour
{
    //스크립터블 오브젝트?
    //3D 사운드 찾아보기

    public List<AudioClip> audioes = new();

    //생성자 vs 함수호출

    public AudioSource[] soundBox;

    public AudioClip FindAudio(AUDIOCLIP audio)
    {
        return audioes[(int)audio];
    }

    public void ShotSound(AUDIOCLIP audio,Vector3 pos)
    {
        foreach(AudioSource source in soundBox)
        {
            if (source.clip == audioes[(int)audio] && source.isPlaying)
            {
                source.Stop();
                source.Play();
                return;
            }

            if (source.isPlaying) continue;

            source.transform.position = pos;
            source.clip = audioes[(int)audio];
            source.Stop();
            source.Play();
            return;
        }

        AudioSource.PlayClipAtPoint(audioes[(int)audio], transform.position);
    }
}
