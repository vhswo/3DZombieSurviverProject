using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{

    RaycastHit hit;
    [SerializeField] Transform equip;
    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 5f, Color.blue);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f, ~LayerMask.GetMask("Player")))
        {
            Transform g = hit.transform;
            if(Input.GetKeyDown("e"))
            {
                g.SetParent(equip);
                g.localEulerAngles = Vector3.zero;
                g.localPosition = Vector3.zero;
                //g.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                //g.TransformPoint(Vector3.zero);
                //g.eulerAngles = Vector3.zero;
            }
        }

        if(Input.GetKeyDown("s"))
        {
            transform.eulerAngles += new Vector3(10f, 0, 0);
        }

        if(Input.GetKeyDown("a"))
        {
            transform.eulerAngles += new Vector3(0, 10f, 0);
        }
    }

}
