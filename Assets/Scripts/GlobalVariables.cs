using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global variables")]
public class GlobalVariables : ScriptableObject
{
    /* Would be useful for a bigger project where variables would be used by many different scripts, not really essential to have globals variables
     * for such a small project 
     */

    [Header("Player")]
    public float mouseXRotationSpeed;
    public float mouseYRotationSpeed;
    public float playerMoveSpeed;
    public float cameraYOffset;
    public int singleBulletDamage;
    public float shootCooldown;
    public float jumpMultiplier;

    [Header("AI")]
    public int agentMaxHealth;
    public float agentMoveSpeed;
    public float ragdollForceMult;
    public int spawnedAgents;

    [Header("Powerups")]
    public int powerupAddedAmmoAmount;
    public float powerupMoveSpeed;
    public float powerupMoveSpeedDur;
    public int explodingBulletDamage;
    public float explodingBulletDuration;
    public float explodingBulletRadius;

    [Header("Misc")]
    public Vector2 spawnBorders;    // Use Y for Z-coordinates. Values extend both +- thus a value of 30 is in fact 60 units.
    public GameObject muzzleFlashPrefab;
    public GameObject explodingBulletEffectPrefab;
    public AudioClip gunShot1;
    public AudioClip gunShot2;

}
