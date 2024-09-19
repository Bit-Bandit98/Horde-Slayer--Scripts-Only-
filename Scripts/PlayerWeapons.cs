using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{

    public GameObject[] Weapons;
    public GameObject Pistol, Shotgun;
    [SerializeField]
    Animator PlayerAnimation;
    public static bool HasShotgun;

 

    private void Start()
    {
        ChangeWeapon();
        HasShotgun = false;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && (!PlayerGun.isReloading &&  !GM.GlobalGameManager.GameIsPaused) && HasShotgun) ChangeWeapon();
        
    }

    void ChangeWeapon()
    {
        foreach (GameObject weapon in Weapons)
        {
            if(weapon.gameObject.activeSelf == true)
            {
                weapon.gameObject.SetActive(false);
                if (weapon.name == "BasicGun")
                {
                    Pistol.SetActive(false);
                    PlayerAnimation.SetBool("PistolEquip", false);
                    
                }else
                {
                    Shotgun.SetActive(false);
                    PlayerAnimation.SetBool("ShotgunEquip", false);
                }

                
            } else
            {
                weapon.gameObject.SetActive(true);
                if(weapon.name == "BasicGun")
                {
                    Pistol.SetActive(true);
                    PlayerAnimation.SetBool("PistolEquip", true);
                    uiManager.PlayerUIState.UpdatePlayerAmmunition(weapon.GetComponent<PlayerGun>().CurrentAmmo, weapon.GetComponent<PlayerGun>().MaxAmmo);
                    if(weapon.GetComponent<PlayerGun>().CurrentAmmo <= 0)
                    {
                        uiManager.PlayerUIState.EnableReloadTip(true);
                    } else
                    {
                        uiManager.PlayerUIState.EnableReloadTip(false);
                    }
                    

                } else
                {
                    Shotgun.SetActive(true);
                    PlayerAnimation.SetBool("ShotgunEquip", true);
                        uiManager.PlayerUIState.UpdatePlayerAmmunition(weapon.GetComponent<Shotgun>().CurrentAmmo, weapon.GetComponent<Shotgun>().MaxAmmo);
                    if (weapon.GetComponent<PlayerGun>().CurrentAmmo <= 0)
                    {
                        uiManager.PlayerUIState.EnableReloadTip(true);
                    } else
                    {
                        uiManager.PlayerUIState.EnableReloadTip(false);
                    }
                }

            }
        }
      //  uiManager.PlayerUIState.UpdatePlayerAmmunition();
    }

   

  
}


