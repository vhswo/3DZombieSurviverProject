using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //other.CompareTag("Player") 디커플링을 위해 move를 가진 모든 객체는 하우스에 반응할수 있도록
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
