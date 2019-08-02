using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRagdoll : MonoBehaviour
{
    [SerializeField] private GlobalVariables globals;
    [SerializeField] private Rigidbody initialForceRb;
    [SerializeField] private float forceMult;

    private void Start()
    {
        // Destroy object after 15 seconds
        Destroy(this.gameObject, 15f);
    }

    public void AddInitialForce(Vector3 force)
    {
        initialForceRb.AddForce(force * globals.ragdollForceMult);
    }

    public void OnHit(Rigidbody hitBody, Vector3 forceOrigin, RaycastHit hitInfo)
    {
        // Ragdoll effect
        if (hitBody != null)
        {
            // Calculate force to apply
            //Vector3 forceDir = new Vector3(transform.position.x - forceOrigin.x, 0f, transform.position.z - forceOrigin.z).normalized;
            Vector3 forceDir = (hitBody.transform.position - forceOrigin).normalized;

            Vector3 addedForce = forceDir * globals.ragdollForceMult * forceMult;

            // Add force from hit direction
            hitBody.AddForceAtPosition(addedForce, hitInfo.point);
        }
    }

}
