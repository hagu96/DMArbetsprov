using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : Bullet
{
    public override void Init(GlobalVariables _globals)
    {
        initialized = true;
        globals = _globals;
        damage = globals.singleBulletDamage;

    }


    /// <summary>
    /// Called when this bullet hits a collider
    /// </summary>
    public override void OnHit(RaycastHit hitInfo, Vector3 origin)
    {
        if(!initialized)
        {
            Debug.LogError("Init must be called on " + this);
        }

        AIController agent = hitInfo.transform.GetComponentInParent<AIController>();
        FlyingRagdoll ragdoll = hitInfo.transform.GetComponentInParent<FlyingRagdoll>();
        FlyingRagdollSpawner spawner = hitInfo.transform.GetComponentInParent<FlyingRagdollSpawner>();
        

        if(agent != null)
        {
            // Do damage
            agent.TakeDamage(damage, hitInfo.rigidbody, origin, hitInfo);
            // Possibly spawn visual
        }

        else if(ragdoll != null)
        {
            ragdoll.OnHit(hitInfo.rigidbody, origin, hitInfo);
        }

        // Ragdoll spawner
        else if (spawner != null)
        {
            spawner.OnHit();
        }

    }


}
