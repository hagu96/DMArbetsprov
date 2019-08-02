using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PowerupSpawner powerupSpawner;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject powerupPickupPanel;
    [SerializeField] private Text ammoText;

    private Text powerupPickupText;

    private void Start()
    {
        player.equippedGun.OnFire += UpdateAmmoText;
        player.equippedGun.OnGunLoad += UpdateAmmoText;

        powerupPickupText = powerupPickupPanel.GetComponentInChildren<Text>();
    }

    // Update ammo text on gun load and gun fire
    private void UpdateAmmoText(int remainingAmmo)
    {
        ammoText.text = remainingAmmo.ToString();
    }


    public void PowerupPickup(Powerup.PowerupType _type)
    {
        StartCoroutine(DispalyPowerupPickup(_type));
    }

    private IEnumerator DispalyPowerupPickup(Powerup.PowerupType _type)
    {
        powerupPickupPanel.SetActive(true);
        powerupPickupText.text = "Picked up " + _type.ToString() + " powerup!";

        yield return new WaitForSeconds(2.5f);
        powerupPickupPanel.SetActive(false);
    }

}
