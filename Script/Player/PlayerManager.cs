using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] Player[] Prefab; // ���÷��̾� ���� ������� �ٸ������ִ�
    [SerializeField] PlayerData[] data; // ���÷��̾�� �����Ͱ� �ٸ����ִ�
    public List<Player> players; // �÷��̾ �������������� �ִٴ� ����


    //���̺� ���� �� ������� ���� ���� or �����ҽ� ���� ���� int�� ����Ű
    //�÷��̾� ui ��� �ֱ�

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
