using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : Bullet
{
    public override void Init(GlobalVariables _globals)
    {
        initialized = true;
        globals = _globals;
        damage = globals.explodingBulletDamage;
    }


    /// <summary>
    /// Called when this bullet hits a collider
    /// </summary>
    public override void OnHit(RaycastHit hitInfo, Vector3 origin)
    {
        if (!initialized)
        {
            Debug.LogError("Init must be called on " + this);
        }

        Collider[] hits = Physics.OverlapSphere(hitInfo.point, globals.explodingBulletRadius);
        List<AIController> damagedAgents = new List<AIController>();
        List<FlyingRagdoll> hitRagdolls = new List<FlyingRagdoll>();

        // Instantiate visual
        GameObject visual = Object.Instantiate(globals.explodingBulletEffectPrefab, hitInfo.point, Quaternion.identity);
        Object.Destroy(visual, 1f);

        // Set origin for proper physics interaction
        origin = hitInfo.point;

        foreach (Collider hitObj in hits)
        {
            AIController agent = hitObj.transform.GetComponentInParent<AIController>();
            FlyingRagdoll hitRagdoll = hitObj.transform.GetComponentInParent<FlyingRagdoll>();
            FlyingRagdollSpawner spawner = hitInfo.transform.GetComponentInParent<FlyingRagdollSpawner>();


            // Make sure there is only one damage instance per agent
            // If this agent has not been damaged by this AoE --> do so
            if (!damagedAgents.Contains(agent) && agent != null)
            {
                damagedAgents.Add(agent);
                agent.TakeDamage(damage, hitObj.attachedRigidbody, origin, hitInfo);
                Debug.Log("AOE Hit on agent");
            }


            // Ragdolls only
            else if(!hitRagdolls.Contains(hitRagdoll) && hitRagdoll != null)
            {
                hitRagdolls.Add(hitRagdoll);
                hitRagdoll.OnHit(hitObj.attachedRigidbody, origin, hitInfo);
            }

            // Ragdoll spawner
            else if (spawner != null)
            {
                spawner.OnHit();
            }

        }
    }
}
