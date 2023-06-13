using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public Vector3 AttackRange = new Vector3(1, 1, 1);

    public Collider[] MeleeAttack(LayerMask layer)
    {
        return Physics.OverlapBox(transform.position, AttackRange * 0.5f, transform.rotation, layer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, AttackRange);

    }
}
