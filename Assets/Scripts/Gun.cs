using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private GlobalVariables globals;
    
    private Bullet ammoType;
    private int maxAmmoCount;
    private int currentAmmoCount;
    private float currentShootCooldown;

    private AudioSource audio;
   // private int maxClipAmmoCount;
   // private int currentClipAmmoCount;

    // Events for UI updates
    public System.Action<int> OnGunLoad;
    public System.Action<int> OnFire;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(currentShootCooldown > 0)
        {
            currentShootCooldown -= Time.fixedDeltaTime;
        }
    }

    public void SetAmmo(Bullet _ammoType, int _ammoCount)
    {
        ammoType = _ammoType;
        currentAmmoCount = _ammoCount;

        OnGunLoad?.Invoke(currentAmmoCount);
    }

    public void AddAmmo(int ammoCount)
    {
        currentAmmoCount += ammoCount;
        OnGunLoad?.Invoke(currentAmmoCount);
    }

    /// <summary>
    /// Shoots a bullet with currently equipped ammo from the camera forwards
    /// </summary>
    public void Fire()
    {
        // Make sure we have bullets
        if(currentAmmoCount > 0 && currentShootCooldown <= 0)
        {
            Debug.Log("Fired with ammo type: " + ammoType);

            // Reset cooldown, decrement ammo and invoke OnFire event
            currentShootCooldown = globals.shootCooldown;
            currentAmmoCount--;
            OnFire?.Invoke(currentAmmoCount);

            // Play animations
            anim.Play("RecoilAnimation");
            // Blend animations
            anim["ShootAnim"].layer = 1;
            anim.Play("ShootAnim");

            // Muzzle flash visual
            GameObject flash = Instantiate(globals.muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(flash, 1f);

            // Play sound
            audio.PlayOneShot(globals.gunShot1);
            audio.PlayOneShot(globals.gunShot2);

            // Raycast from camera
            Vector3 shootOrigin = Camera.main.transform.position;
            Vector3 shootDir = Camera.main.transform.forward;
            LayerMask ignorePlayerMask = ~(1 << LayerMask.NameToLayer("Player"));   // Bitshit to make mask ignore this layer only
            RaycastHit hit;

            // Shoot ray from camera in camera direction that ignores the "Player" layer
            if (Physics.Raycast(shootOrigin, shootDir, out hit, 100f, ignorePlayerMask))
            {
                ammoType.OnHit(hit, shootOrigin);
                Debug.Log(hit.transform.name);
            }
        }

        else
        {
            // Play some sound/error message
        }
    }


    public int GetCurrentAmmoCount()
    {
        return currentAmmoCount;
    }

    public Bullet GetCurrentAmmo()
    {
        return ammoType;
    }
}
