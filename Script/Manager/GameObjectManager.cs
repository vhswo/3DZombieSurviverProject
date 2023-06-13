using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OBJTYPE
{
    BULLET,
    BULLETHOLE,
    GRANDER,
    OBJECT_END
}

 public class GameObjectManager : MonoBehaviour
{
    [SerializeField] InGameObjects[] ObjectsPrefab;
    public Dictionary<OBJTYPE, ObjPooling<InGameObjects>> m_Objects = new();
   

    public void init()
    {
        for(OBJTYPE i = OBJTYPE.BULLET; i < OBJTYPE.OBJECT_END; i++)
        {
            m_Objects[i] = new();
            //CreateObj(i);
        }
        
    }

   // public void CreateObj(OBJTYPE type)
   // {
   //     for (int j = 0; j < 5; j++)
   //     {
   //         InGameObjects obj = Instantiate(ObjectsPrefab[(int)type]);
   //         if (type == OBJTYPE.GRANDER)
   //         {
   //             (obj as ThrowingObject).m_bombParticle = GameManager.Instance.FindParticle(EFFECTTYPE.GRANDERBOMB, obj.transform);
   //         }
   //         m_Objects[type].AddObj(obj);
   //
   //     }
   // }
}
