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
  //  [SerializeField] enemy[] ZombiesPrefab;  //���� ������
  //  public Dictionary<int, ObjPooling<enemy>> Zombies =new(); // �� ���� ��ü���� �����,��ųʸ��� Ű, ť��
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
    /// �Ű����� = ���° ������, ���° �������� �� ��������
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
