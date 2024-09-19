using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    public int MaxAmmo, CurrentAmmo, BaseDamage;
    
    [SerializeField]
    float ReloadTime;
    [SerializeField]
    protected GameObject nozzle;
    [SerializeField]
    protected float range, GunVolume;
    [SerializeField]
    float radius;
    [SerializeField]
    protected bool infiniteAmmo, Reloading;
    [SerializeField]
    protected ParticleSystem MuzzleFlash;
    [SerializeField]
    protected AudioClip Reload, OutOfAmmoClick, ReloadingSound, Gunshot;
    [SerializeField]
    protected AudioSource GunAudio;
    protected Collider[] HitSphere;
    public static bool isReloading;
    [SerializeField]
    protected Animator PlayerAnime;

    protected int EnemyLayerMask = 1 << 9;
   

    public float Radius
    {
        get {
            return radius;
        }
        set {
            radius = value;
        }
    }

    public GameObject Nozzle
    {
        get
        {
            return nozzle;
        }
        set
        {
            nozzle = value;
        }
    }

    private void Start()
    {
        CurrentAmmo = MaxAmmo;
        Reloading = false;
        uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);
    }
    private void Update()
    {
        //print(Time.time);
        if(!GM.GlobalGameManager.GameIsPaused && PlayerChar.Instance.IsAlive) { 
        if(Input.GetKeyDown(KeyCode.R) && !Reloading) StartCoroutine("ReloadWeapon");
        if (Input.GetMouseButtonUp(0)) PlayerAnime.SetBool("Fired", false);
        ShootBasic();
        }
    }
    private void ShootBasic()
    {
  
        if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0 && !Reloading)
        {
            CameraSystem.Instance.StartCoroutine("ScreenShake", CameraSystem.Instance.ShakeTime / 4);
            //RaycastHit SphereHit;
            PlayerAnime.SetBool("Fired", true);
            HitSphere = Physics.OverlapSphere(transform.position, GunVolume, EnemyLayerMask);

            //If an enemy is nearby, they will be alerted should they hear the gunshot
            for (int i = 0; i < HitSphere.Length; i++)
            {
                HitSphere[i].SendMessage("ChasePlayer");
            }




            if (!infiniteAmmo) CurrentAmmo -= 1;
            if(CurrentAmmo <= 0)uiManager.PlayerUIState.EnableReloadTip(true);
            GunAudio.pitch = Random.Range(0.80f, 1.20f);
            GunAudio.PlayOneShot(Gunshot);
            uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);

            Vector3 RandomizedPos = ((Random.insideUnitSphere * radius) + nozzle.transform.position);
            MuzzleFlash.transform.position = RandomizedPos;
            MuzzleFlash.Play();

            RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll(RandomizedPos, nozzle.transform.forward, Mathf.Infinity, EnemyLayerMask);
            //Debug.DrawRay(RandomizedPos, nozzle.transform.forward * 100f, Color.red, 100f);
            //Creates an array of hit objects by a raycast, then sorts it, then only goes through as many as needed to replicate bullet penetration.
            if (hits.Length > 0)
            {
                hits = hits.OrderBy(h => h.distance).ToArray();
                for (int i = 0; i < hits.Length && i < 2; i++)
                {
                    if (PlayerChar.IsTripleDamage) { hits[i].collider.gameObject.SendMessage("takeDamage", ((BaseDamage * PlayerChar.Instance.DamageMultiplyer) * Random.Range(0.8f, 1.2f)) * 3); }
                    else { hits[i].collider.gameObject.SendMessage("takeDamage", (BaseDamage * PlayerChar.Instance.DamageMultiplyer) * Random.Range(0.8f, 1.2f)); }

                }
               
            }
            

        }
        else if (Input.GetMouseButtonDown(0) && CurrentAmmo <= 0)
        {
            GunAudio.pitch = 1;
            GunAudio.PlayOneShot(OutOfAmmoClick);
        }

    }

    

   protected IEnumerator ReloadWeapon()
    {
        if (PlayerChar.IsHaste)
        {
            isReloading = false;
            CurrentAmmo = MaxAmmo;
            GunAudio.PlayOneShot(Reload);
            Reloading = false;
            uiManager.PlayerUIState.EnableReloadTip(false);
            uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);
            yield break;
        }
        isReloading = true;
        GunAudio.pitch = 1;
        GunAudio.PlayOneShot(ReloadingSound);
        uiManager.PlayerUIState.DisplayReloadingBar(true);
        Reloading = true;
        // float TargetTime = ReloadTime + Time.time;
       
        float StartTime = Time.time;
        float TargetTime = (ReloadTime / PlayerChar.Instance.BaseReloadSpeed) + StartTime;
        float ElapsedTime = 0;

        while( Time.time < TargetTime)
        {
            if (!PlayerChar.Instance.IsAlive) { break; }
            //ElapsedTime =  (TargetTime - Time.time);
            ElapsedTime = ((TargetTime - StartTime) - (Time.time - StartTime));
            uiManager.PlayerUIState.UpdateReloadingBar(1 - (ElapsedTime / (TargetTime - StartTime)));
          
            yield return null;
        }

        if (PlayerChar.Instance.IsAlive) { 
        uiManager.PlayerUIState.UpdateReloadingBar(0);
        uiManager.PlayerUIState.DisplayReloadingBar(false);
        isReloading = false;
        CurrentAmmo = MaxAmmo;
        GunAudio.PlayOneShot(Reload);
        Reloading = false;
            uiManager.PlayerUIState.EnableReloadTip(false);
            uiManager.PlayerUIState.UpdatePlayerAmmunition(CurrentAmmo, MaxAmmo);
        } else
        {
            uiManager.PlayerUIState.UpdateReloadingBar(0);
            uiManager.PlayerUIState.DisplayReloadingBar(false);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, GunVolume);
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(nozzle.transform.position, radius);
    }
}
