using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletHole : InGameObjects
{
    public void OnBulletHole(Vector3 location, Vector3 normal)
    {
        transform.position = location;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);

        gameObject.SetActive(true);

        StartCoroutine(ShowBulletHole());
    }

    IEnumerator ShowBulletHole()
    {

        yield return new WaitForSeconds(1.3f);

        gameObject.SetActive(false);
        GameManager.Instance.m_GbjMgr.m_Objects[OBJTYPE.BULLETHOLE].m_PoolingObjs.Enqueue(this);
    }
}
