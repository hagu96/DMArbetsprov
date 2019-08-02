using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] private AIController agentPrefab;
    [SerializeField] private GlobalVariables globals;
    [SerializeField] private List<Transform> patrolPositions = new List<Transform>();
    private List<AIController> aliveAgents = new List<AIController>();


    void Start()
    {
        SpawnAgents();
    }

    private void SpawnAgents()
    {
        for (int i = 0; i < globals.spawnedAgents; i++)
        {
            AIController agent = Instantiate(agentPrefab);
            aliveAgents.Add(agent);
            agent.Init(patrolPositions, this);
        }
    }

    public void RemoveAgent(AIController _agent)
    {
        aliveAgents.Remove(_agent);

        // If every agent is dead, spawn new ones
        if(aliveAgents.Count <= 0)
        {
            SpawnAgents();
        }
    }
}
