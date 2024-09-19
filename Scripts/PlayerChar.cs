using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : CharacterClass
{
    public static bool IsHaste;
    public static bool IsInvincible;
    public static bool IsTripleDamage;
    
    public int PlayerLevel;
    [SerializeField]
    ParticleSystem LevelupBeam;
    public float DamageMultiplyer, BaseReloadSpeed;
    //[SerializeField]
    public static PlayerChar Instance;
    public bool IsAlive;
    public static bool maxLevel;
    [SerializeField]
    AudioSource PlayerSounds;
    [SerializeField]
    AudioClip Attacked, LevelUpSound;
    public int currentEXP, TargetEXP, Level2Target, Level3Target, Level4Target, Level5Target;
    public static GameObject Player;
    [SerializeField]
    Animator PlayerAnimation;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerLevel = 1;
        maxLevel = false;
        TargetEXP = Level2Target;
        Player = this.gameObject;
        Instance = this;
    }
    void Start()
    {
        IsAlive = true;
        uiManager.PlayerUIState.UpdatePlayerHealth(health, maxHealth);
    }
    

    //override PlayerDamage for UI;
    public override void takeDamage(int Damage)
    {
        if (!IsInvincible) { 
        base.takeDamage(Damage);
            PlayerSounds.pitch = 1;
        PlayerSounds.PlayOneShot(Attacked);
        uiManager.PlayerUIState.StartCoroutine("FlashDamage");
        uiManager.PlayerUIState.UpdatePlayerHealth(health, maxHealth);
        }
    }

     

    public override void die()
    {
        PlayerAnimation.SetBool("Dead", true);
        IsAlive = false;
        GM.PlayerDead = true;
        uiManager.PlayerUIState.DisplayDeath();
    }
   

    public void RecieveEXP(int exp)
    {
        if (!maxLevel) { 
        currentEXP += exp;
        uiManager.PlayerUIState.UpdateLevelGauge();
        if(currentEXP >= TargetEXP)
        {
            LevelUp();
        }
        }
    }

    void LevelUp()
    {
        PlayerLevel++;
        PlayerSounds.pitch = 1;
        LevelupBeam.Play();
        PlayerSounds.PlayOneShot(LevelUpSound);
        if (PlayerLevel == 2)
        {
            BaseSpeed += 1;
            maxHealth += 40;
            DamageMultiplyer += 1;
            BaseReloadSpeed += 1;
            TargetEXP = Level3Target;

        }
        if (PlayerLevel == 3)
        {
            BaseSpeed += 2;
            maxHealth += 150;
            DamageMultiplyer += 2;
            BaseReloadSpeed += 1;
            TargetEXP = Level4Target;
        }
        if (PlayerLevel == 4)
        {
            BaseSpeed += 2;
            maxHealth += 300;
            DamageMultiplyer += 2;
            BaseReloadSpeed += 1;
            TargetEXP = Level5Target;
        }
        if (PlayerLevel == 5)
        {
            BaseSpeed += 2;
            maxHealth += 500;
            DamageMultiplyer += 3;
            BaseReloadSpeed += 2;
            maxLevel = true;
            TargetEXP = 99999;
            currentEXP = 99999;
        }
        if(!maxLevel)currentEXP = 0;
        health = maxHealth;

        //Update interface for Player
        uiManager.PlayerUIState.UpdatePlayerHealth(health, maxHealth);
        uiManager.PlayerUIState.UpdateLevel();
        uiManager.PlayerUIState.StartCoroutine("LevelUpDisplay");
        uiManager.PlayerUIState.UpdateLevelGauge();

    }

}
