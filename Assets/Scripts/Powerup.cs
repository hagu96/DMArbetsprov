using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private GlobalVariables globals;
    public enum PowerupType { Ammo, Agility, ExplodingBullets}
    private PowerupType powerupType;

    private Collider col;
    private MeshRenderer mesh;

    private PlayerUI playerUI;

    private void Start()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
        playerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            switch(powerupType)
            {
                case PowerupType.Ammo:
                    AddAmmoPowerup(player);
                    break;

                case PowerupType.Agility:
                    StartCoroutine(AgilityPowerup(player));
                    break;

                case PowerupType.ExplodingBullets:
                    StartCoroutine(ExplodingBulletsPowerup(player));
                    break;

            }
        }
    }

    public void SetPowerupType(Powerup.PowerupType _type)
    {
        powerupType = _type;
    }

    private void AddAmmoPowerup(PlayerController player)
    {
        // Notify PlayerUI that we picked up this type of powerup
        playerUI.PowerupPickup(Powerup.PowerupType.Ammo);

        player.equippedGun.AddAmmo(globals.powerupAddedAmmoAmount);
        Destroy(this.gameObject);
    }

    private IEnumerator AgilityPowerup(PlayerController player)
    {
        // Notify PlayerUI that we picked up this type of powerup
        playerUI.PowerupPickup(Powerup.PowerupType.Agility);

        // 'Disable' the object by disabling colliders and make it invisible
        col.enabled = false;
        mesh.enabled = false;

        // Set move speed
        player.SetMoveSpeed(globals.powerupMoveSpeed);

        // Wait according to powerup duration
        yield return new WaitForSeconds(globals.powerupMoveSpeedDur);

        // Reset and destroy object
        player.SetMoveSpeed(globals.playerMoveSpeed);
        Destroy(this.gameObject);
    }

    private IEnumerator ExplodingBulletsPowerup(PlayerController player)
    {
        // Notify PlayerUI that we picked up this type of powerup
        playerUI.PowerupPickup(Powerup.PowerupType.ExplodingBullets);

        // 'Disable' the object by disabling colliders and make it invisible
        col.enabled = false;
        mesh.enabled = false;

        // Save previous used bullet and set current bullet to exploding
        Bullet savedAmmo = player.equippedGun.GetCurrentAmmo();

        // Replace normal bullets with exploding bullets 
        ExplodingBullet newAmmo = new ExplodingBullet();
        newAmmo.Init(globals);
        player.equippedGun.SetAmmo(newAmmo, player.equippedGun.GetCurrentAmmoCount());

        // Wait according to powerup duration
        yield return new WaitForSeconds(globals.explodingBulletDuration);

        // Reset and destroy object
        player.equippedGun.SetAmmo(savedAmmo, player.equippedGun.GetCurrentAmmoCount());
        Destroy(this.gameObject);

    }
}
