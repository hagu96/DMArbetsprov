using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRagdollSpawner : MonoBehaviour
{
    [SerializeField] FlyingRagdoll ragdollPrefab;
    [SerializeField] GameObject ragdollMachine;
    Vector3 initialDirection;

    [SerializeField] private float xMult, yMult, zMult;

    void Start()
    {
        initialDirection = new Vector3(1, 1, 1);
    }


    /// <summary>
    /// Spawns a flying ragdoll
    /// </summary>
    public void OnHit()
    {
        // Spawn flying ragdoll
        FlyingRagdoll ragdoll = Instantiate(ragdollPrefab, ragdollMachine.transform.position, Quaternion.identity);

        Vector3 addedForce = initialDirection;
        addedForce.x *= xMult;
        addedForce.y *= yMult;
        addedForce.z *= zMult;
        ragdoll.AddInitialForce(addedForce);
    }
}
