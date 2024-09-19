using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class uiManager : MonoBehaviour
{
    [SerializeField]
    float padding;
    [SerializeField]
    TextMeshProUGUI PauseText, PlayerHealthText, AmmoText, EnemiesLeftText, VictoryText, LevelText, GaugeText, LevelUpBonusText, WaveText, ReloadText;
    [SerializeField]
    Slider PlayerHealthSlider, ReloadingBar, EXPBar;
    [SerializeField]
    Image HealthBarColour, DamageOverlay;
    [SerializeField]
    GameObject PauseMenu;
    

   


    public static uiManager PlayerUIState;

    private void Awake()
    {
        PlayerUIState = this;
        
    }

    private void Start()
    {
        PauseTextDisplay(false);
        ShowPauseMenu(false);
        UpdateEnemyCount(0);
        UpdateLevelGauge();
        UpdateLevel();
        UpdateWaveText();
    }

    public IEnumerator FlashDamage()
    {
        for (int i = 0; i < 5; i++) { 
        DamageOverlay.gameObject.SetActive(true);
            yield return null;
        }
        DamageOverlay.gameObject.SetActive(false);
    }

    private void Update()
    {
        ReloadingBar.transform.position = Input.mousePosition + new Vector3(20,padding,0);
    }
    public void PauseTextDisplay(bool enabled)
    {
        if (enabled)
        {
            PauseText.text = "PAUSED";
        } else
        {
            PauseText.text = "";
        }
    }


    public void UpdateReloadingBar(float Percentage)
    {
        ReloadingBar.value = Percentage;
       
    }

    public void DisplayReloadingBar(bool State)
    {
        ReloadingBar.gameObject.SetActive(State);
    }
   
    public  void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
       PlayerHealthText.text = "HP: " + currentHealth + "/" + maxHealth;
        PlayerHealthSlider.value = (float)currentHealth / maxHealth;
        if (PlayerHealthSlider.value > 0.75f) HealthBarColour.color = Color.green;
        if (PlayerHealthSlider.value < 0.51f) HealthBarColour.color = Color.yellow;
        if (PlayerHealthSlider.value < 0.26f) HealthBarColour.color = Color.red;
    }

    public void UpdatePlayerAmmunition(int currentAmmo, int maxAmmo)
    {
        AmmoText.text = currentAmmo + "/" + maxAmmo;
    }

    public void EnableReloadTip(bool EnableState)
    {
        ReloadText.gameObject.SetActive(EnableState);
    }

    public void UpdateEnemyCount(int num)
    {
        EnemiesLeftText.text = "Enemies Left: " +num.ToString();
    }

    public void DisplayDeath()
    {
        VictoryText.text = "YOU DIED. HIT [E] TO TRY AGAIN!";
    }
    public void DisplayVictory(bool Switch)
    {

        if (Switch) {
            if (GM.Wave != 10) VictoryText.text = "WAVE " + GM.Wave + " COMPLETED!";
            else
            {
                VictoryText.text = "ALL WAVES COMPLETED, GREAT JOB! HIT [E] TO PLAY AGAIN!";
                GM.Victory = true;
            }
    } else
        {
            VictoryText.text = "";
        }
}

    public void UpdateLevel()
    {
        LevelText.text = "Level " +PlayerChar.Instance.PlayerLevel;
    }

    public void UpdateLevelGauge()
    {
        float temp = (float)PlayerChar.Instance.currentEXP / (float)PlayerChar.Instance.TargetEXP;

        GaugeText.text = PlayerChar.Instance.currentEXP + "/" + PlayerChar.Instance.TargetEXP;
        EXPBar.value = temp;
    }
    
    public void UpdateWaveText()
    {
        WaveText.text = "Wave "+GM.Wave;
    }
    public void ShowPauseMenu(bool Display)
    {
        PauseMenu.SetActive(Display);
    }

    public IEnumerator LevelUpDisplay() {

        LevelUpBonusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        LevelUpBonusText.gameObject.SetActive(false);
    }
}
