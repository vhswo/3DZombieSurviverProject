using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionObj : Activate
{
    //�������� �̸� �� ������ �׸��� e Ű Ȱ��ȭ �� ������ �۵� 
    // false �� �Ǹ�  eŰ ��Ȱ��ȭ �ʱ�ȭ�� ���� ������Ʈ�� �������� �Ѵ�

   // [SerializeField] ItemData[] data;
   //
   // public List<ItemObject> itemList { get; private set;} = new();
   //
   // //sprite , ������ ����,����
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
   //     //���� ��� �ɵ�?
   //     if (other.TryGetComponent(out Player ui))
   //     {
   //     }
   // }
   public override void activate(Vector3 Pushdir)
   {
       //������ �ǽð� ����
   }
}
