using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum ENEMYSTATE
{
    NONE,
    FOLLOWNOISE,
    FOLLOWPLAYER,
    ATTACK,
    DIE
}

public class enemyManager : MonoBehaviour
{
  //  [SerializeField] enemy[] ZombiesPrefab;  //좀비 프리팹
  //  public Dictionary<int, ObjPooling<enemy>> Zombies =new(); // 각 좀비 개체마다 만들기,딕셔너리로 키, 큐값
  // 
  //  public void Init()
  //  {
  //      for(int i = 0; i < ZombiesPrefab.Length; i++)
  //      {
  //          if (!Zombies.ContainsKey(i)) Zombies[i] = new ObjPooling<enemy>();
  //
  //      }
  //  }
  //
  //  ///<summary>
    /// 매개뱐수 = 몇번째 데이터, 몇번째 프리팹을 몇 마리생성
    ///</summary>
  //  public void CreateZombie(int PrefabNumber, int num)
  //  {
  //      for(int i = 0; i < num; i++)
  //      {
  //          enemy MakingZombie = Instantiate(ZombiesPrefab[PrefabNumber]);
  //
  //          Zombies[PrefabNumber].AddObj(MakingZombie);
  //          MakingZombie.gameObject.SetActive(false);
  //      }
  //  }
}
