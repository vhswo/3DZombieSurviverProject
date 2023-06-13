using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionObj : Activate
{
    //아이템을 미리 다 보낸다 그리고 e 키 활성화 및 누르면 작동 
    // false 가 되면  e키 비활성화 초기화는 다음 오브젝트에 들어왔을때 한다

   // [SerializeField] ItemData[] data;
   //
   // public List<ItemObject> itemList { get; private set;} = new();
   //
   // //sprite , 아이템 정보,수량
   //
   // private void Start()
   // {
   //     for (int i = 0; i < data.Length; i++)
   //     {
   //         ItemData _Data = data[i];
   //         itemList.Add(_Data.CreateItem());
   //     }
   // }
   //
   // private void OnTriggerEnter(Collider other)
   // {
   //     RaycastHit hit;
   // }
   // private void OnTriggerExit(Collider other)
   // {
   //     //굳이 없어도 될듯?
   //     if (other.TryGetComponent(out Player ui))
   //     {
   //     }
   // }
   public override void activate(Vector3 Pushdir)
   {
       //아이템 실시간 빼기
   }
}
