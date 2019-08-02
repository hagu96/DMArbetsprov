using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private GlobalVariables globals;
    private List<Transform> patrolPositions = new List<Transform>();
    
    private NavMeshAgent agent;
    private AgentSpawner agentSpawner;

    private int currentTargetIterator = 0;
    private int currentHealth;

    private bool isAlive = true;
    private bool initialized = false;

    private List<Transform> childs = new List<Transform>();
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    public void Init(List<Transform> _patrolPositions, AgentSpawner _agentSpawner)
    {
        patrolPositions = _patrolPositions;
        agentSpawner = _agentSpawner;

        SetInitialPosition();

        agent = GetComponent<NavMeshAgent>();
        agent.destination = patrolPositions[currentTargetIterator].position;
        agent.speed = globals.agentMoveSpeed;

        // Set max health
        currentHealth = globals.agentMaxHealth;

        // Find every child
        FindEveryChild(gameObject.transform);
        for (int i = 0; i < childs.Count; i++)
        {
            FindEveryChild(childs[i]);
        }

        // Disable rigidbodies (ragdoll)
        SetRigidbodiesKinematic(true);

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            Debug.LogError("Init must be called on " + this);

        // If we are in range of current patrol target, go to next patrol pos
        if(IsInRangeOf(this.transform, patrolPositions[currentTargetIterator], agent.stoppingDistance) && isAlive)
        {

            currentTargetIterator++;

            // If we reached the final array element --> reset
            if (currentTargetIterator % patrolPositions.Count == 0)
            {
                currentTargetIterator = 0;
            }

            agent.destination = patrolPositions[currentTargetIterator].position;
        }


    }

    private void SetRigidbodiesKinematic(bool state)
    {
        // Enable/Disable ragdoll by setting rigidbodies' kinematic state
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = state;
        }
    }

    private void FindEveryChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childs.Add(parent.GetChild(i));

            // Add rigidbody components to list if it exists
            Rigidbody rb = parent.GetChild(i).GetComponent<Rigidbody>();

            if(rb != null)
            {
                rigidbodies.Add(rb);
            }
        }
    }


    private void SetInitialPosition()
    {
        // Spawn within bounds. Invalid locations (inside objects) will automatically be solved by NavMeshAgent
        float randomX = Random.Range(-globals.spawnBorders.x, globals.spawnBorders.x);
        float randomZ = Random.Range(-globals.spawnBorders.y, globals.spawnBorders.y);

        transform.position = new Vector3(randomX, 0, randomZ);
    }

    public void TakeDamage(int damage, Rigidbody hitBody, Vector3 forceOrigin, RaycastHit hitInfo)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            Debug.Log("Agent took dmg. Current hp: " + currentHealth);
        }

        // If we below 0 health --> die
        if(currentHealth <= 0) 
        {
            // Ragdoll effect
            if (hitBody != null)
            {
                // Calculate force to apply
                Vector3 forceDir = (hitBody.transform.position - forceOrigin).normalized;
                Vector3 addedForce = forceDir * globals.ragdollForceMult;

                // Enable rigidbodies (ragdoll)
                SetRigidbodiesKinematic(false);

                // Add force from hit direction
                hitBody.AddForceAtPosition(addedForce, hitInfo.point);
            }

            else
            {
                Debug.LogError("NULL RIGIDBODY HIT");
            }

            // Stop further movement
            agent.destination = transform.position;
            isAlive = false;

            // Destroy the agent after 5 seconds
            agentSpawner.RemoveAgent(this);
            Destroy(this.gameObject, 5f);
        }
    }

    // Range check
    private bool IsInRangeOf(Transform t1, Transform t2, float range)
    {
        float distance = Vector3.Distance(t1.position, t2.position);
        return distance < range;
    }


}
