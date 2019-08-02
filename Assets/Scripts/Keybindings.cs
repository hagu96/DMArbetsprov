using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Keybindings : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    void Update()
    {
        // Restart scene
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);
        }

        // Cheat: give yourself 30 ammo of currently equipped ammo
        if(Input.GetKeyDown(KeyCode.F2))
        {
            player.equippedGun.AddAmmo(30);
        }

        // Quit
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
