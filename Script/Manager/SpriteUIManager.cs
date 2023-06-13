using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IMGUITYPE
{
    QUICKUI,
    ITEMUI,
    END
}

public class SpriteUIManager : MonoBehaviour
{
    [Header("ƒ¸ΩΩ∑‘UI")]
    [SerializeField] SpriteRenderer[] QuickUI;
    [Header("æ∆¿Ã≈€UI")]
    [SerializeField] SpriteRenderer[] ItemUI;

    Dictionary<IMGUITYPE, SpriteRenderer[]> m_ImgUI =new();
    public void init()
    {
        m_ImgUI[IMGUITYPE.QUICKUI] = QuickUI;
        m_ImgUI[IMGUITYPE.ITEMUI] = ItemUI;
    }

    public SpriteRenderer FindQuickImgUI(IMGUITYPE type,string name)
    {
        for(int i =0; i< m_ImgUI[type].Length; i++)
        {
            if(m_ImgUI[type][i].name == name)
            {
                return m_ImgUI[type][i];
            }
        }

        return null;
    }

}
