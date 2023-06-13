using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ActivateObjDetect : Detecting
{
    Activate _activate;

    public Activate GetActivate()
    {
        Collider[] value = Detect();

        if (value == null) return null;

        _activate = null;

        foreach (Collider activate in value)
        {
            if (activate.TryGetComponent(out Activate acti))
            {
                if (_activate == null) _activate = acti;
                else
                {
                    Vector3 now = _activate.transform.position - transform.position;
                    Vector3 compare = acti.transform.position - transform.position;

                    float fnow = now.sqrMagnitude;
                    float fcompare = compare.sqrMagnitude;

                    if(fnow > fcompare) _activate = acti;
                }
            }
        }

        return _activate;
    }

}
