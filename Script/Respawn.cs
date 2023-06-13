using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Respawn : MonoBehaviour
{
    private Collider GroundCollider;
    private Vector3 RandomPositon;
    private float Ground_X;
    private float Ground_Z;
    private void Awake()
    {
        GroundCollider = GetComponent<Collider>();
    }

    public Vector3 RandomRespawnPosition()
    {
        while(true)
        {
            Ground_X = GroundCollider.bounds.size.x;    
            Ground_Z = GroundCollider.bounds.size.z;

            float max = Ground_X * Ground_Z;
            Ground_X = Random.Range((Ground_X / 2) * -1, Ground_X / 2);
            Ground_Z = Random.Range((Ground_Z / 2) * -1, Ground_Z / 2);

           
            RandomPositon = new Vector3(Ground_X, 0.0f, Ground_Z);
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(RandomPositon, out navHit, max, NavMesh.AllAreas))
            {
                if (navHit.mask == 1) return navHit.position;
            };
        }
    }
}
