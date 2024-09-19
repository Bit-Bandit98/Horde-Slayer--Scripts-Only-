using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : PlayerGun
{
    [SerializeField]
    int Pellets;
    [SerializeField]
    float spread;
    

    void Start()
    {
        CurrentAmmo = MaxAmmo;
        uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);
    }

    
    void Update()
    {
        if (!GM.GlobalGameManager.GameIsPaused && PlayerChar.Instance.IsAlive)
        {
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                ShootBasic();
            }
            if (Input.GetMouseButtonUp(0)) PlayerAnime.SetBool("Fired", false);

            if (Input.GetKeyDown(KeyCode.R) && !Reloading) StartCoroutine("ReloadWeapon");
        }
        }

   
    //Shotgun Varient of the shoot function
    private void ShootBasic()
    {
        if (CurrentAmmo > 0 && !Reloading)
        {
            CameraSystem.Instance.StartCoroutine("ScreenShake", CameraSystem.Instance.ShakeTime );
            CurrentAmmo--;
            if (CurrentAmmo <= 0) uiManager.PlayerUIState.EnableReloadTip(true);
            PlayerAnime.SetBool("Fired", true);
            GunAudio.PlayOneShot(Gunshot);
            GunAudio.pitch = Random.Range(0.9f, 1.2f);
            HitSphere = Physics.OverlapSphere(transform.position, GunVolume, EnemyLayerMask);

            //If an enemy is nearby, they will be alerted should they hear the gunshot
            for (int i = 0; i < HitSphere.Length; i++)
            {

                HitSphere[i].SendMessage("ChasePlayer");

            }

            // Will fire however many raycasts each with a random angle and direction to simulate shotgun pellete spread.
            for (int i = 0; i < Pellets; i++)
            {
                Vector3 RandomizedPos = ((Random.insideUnitSphere * Radius) + Nozzle.transform.position);
               

                RaycastHit[] hits = Physics.RaycastAll(RandomizedPos, nozzle.transform.forward, Mathf.Infinity, EnemyLayerMask);

                //Creates an array of hit objects by a raycast, then sorts it, then only goes through as many as needed to replicate bullet penetration.
                if (hits.Length > 0) {
                    hits = hits.OrderBy(h => h.distance).ToArray();
                    for (int x = 0; x < hits.Length && x < 3; x++)
                {
                   
                        if (PlayerChar.IsTripleDamage) { hits[x].collider.gameObject.SendMessage("takeDamage", ((BaseDamage * PlayerChar.Instance.DamageMultiplyer) * Random.Range(0.8f, 1.2f)) * 3); }
                        else { hits[x].collider.gameObject.SendMessage("takeDamage", (BaseDamage * PlayerChar.Instance.DamageMultiplyer) * Random.Range(0.8f, 1.2f)); }

                    }
                }


            }
            MuzzleFlash.Play();
            uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);
        } else
        {
            GunAudio.PlayOneShot(OutOfAmmoClick);
            }
        }
    }

