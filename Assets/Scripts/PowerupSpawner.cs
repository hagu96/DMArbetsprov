using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private Powerup powerupPrefab;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private int spawnInterval;     // Time in seconds between power-up spawn

    private float spawnTimeRemaining;
    private Powerup spawnedPowerup;

    private void Start()
    {
        SpawnRandomPowerup();
    }

    private void FixedUpdate()
    {
        if(spawnTimeRemaining > 0)
        {
            spawnTimeRemaining -= Time.fixedDeltaTime;
        }

        if(spawnTimeRemaining <= 0)
        {
            SpawnRandomPowerup();
        }
    }

    private void SpawnRandomPowerup()
    {
        spawnTimeRemaining = spawnInterval;
        

        // If previous powerup has not been picked up --> destroy it
        if (spawnedPowerup != null)
            Destroy(spawnedPowerup.gameObject);

        // Randomize type of powerup
        int powerupCount = System.Enum.GetValues(typeof(Powerup.PowerupType)).Length;
        int randomTypeIndex = Random.Range(0, powerupCount);
        Powerup.PowerupType randomType = (Powerup.PowerupType)randomTypeIndex;
        Debug.Log(randomType);

        // Randomize location
        int randomLocationIndex = Random.Range(0, spawnPositions.Count);

        // Spawn and set type
        // Offset yPos
        Vector3 spawnPos = new Vector3(spawnPositions[randomLocationIndex].position.x, 
            spawnPositions[randomLocationIndex].position.y + 0.5f, 
            spawnPositions[randomLocationIndex].position.z);

        spawnedPowerup = Instantiate(powerupPrefab, spawnPos, Quaternion.identity);
        spawnedPowerup.SetPowerupType(randomType);

    }
}
