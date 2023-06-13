using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //other.CompareTag("Player") ��Ŀ�ø��� ���� move�� ���� ��� ��ü�� �Ͽ콺�� �����Ҽ� �ֵ���
        if (other.TryGetComponent(out Player move))
        {
            move.InHousePlayer(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player move))
        {
            move.InHousePlayer(false);
        }
    }

}
