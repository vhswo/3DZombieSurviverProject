using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player[] Prefab; // 각플레이어 마다 생김새가 다를수가있다
    [SerializeField] PlayerData[] data; // 각플레이어마다 데이터가 다를수있다
    public List<Player> players; // 플레이어가 여러명있을수도 있다는 가정


    //세이브 파일 이 있을경우 먼저 생성 or 저장할시 따로 저장 int는 고유키
    //플레이어 ui 들고 있기

    public PlayerManager()
    {
        players = new();
    }

    public void CreatePlayer(int PrefabNum,int dataNumber)
    {
        Player player = Instantiate(Prefab[PrefabNum]);
        player.PlayerInit(data[dataNumber], dataNumber);
        players.Add(player);
    }

    public void OnPlayer(int playerNum,Vector3 Location)
    {
        players[playerNum].gameObject.transform.position = Location;
        players[playerNum].gameObject.SetActive(true);
    }
}
