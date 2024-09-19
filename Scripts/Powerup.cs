using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Powerup : MonoBehaviour
{
   // [SerializeField]
    enum Orb {Invincibility, TripleDamage, Haste, Health, Shotgun };
    [SerializeField]
     Orb OrbType;
    Vector3 Movement, StartPos;
    [SerializeField]
    AudioSource PlayerAudio;
    [SerializeField]
    AudioClip PickUp;
    [SerializeField]
    float Frequency, duration, RespawnTime;
    [SerializeField]
    PlayerChar PlayerStats;
    [SerializeField]
    GameObject PowerModel;
    [SerializeField]
    TMPro.TextMeshProUGUI TripleText, InvinText, HasteText, HPText, ShotgunText;
    
    //Image TripleOverlay, InvincibleOverlay, HasteOverlay;
    
    // Start is called before the first frame update
    void Start()
    {
        
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement = new Vector3(0, Mathf.Sin(Time.time * Frequency) * 0.5f, 0);
        transform.position = Movement + StartPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

            if (OrbType == Orb.Health && PlayerChar.Instance.health < PlayerChar.Instance.maxHealth && PlayerChar.Instance.IsAlive)
            {
                StartCoroutine("RefillHealth", false);
                AudioAndModelChange(true);
            } else if( OrbType == Orb.Health && PlayerChar.Instance.IsAlive && PlayerChar.Instance.health == PlayerChar.Instance.maxHealth)
            {
                StartCoroutine("RefillHealth", true);
            }
            else if (OrbType == Orb.Invincibility && !PlayerChar.IsInvincible)
            {
                StartCoroutine("ActivateInvincibility");
                AudioAndModelChange(true);
            }
            else if (OrbType == Orb.Haste && !PlayerChar.IsHaste)
            {
                StartCoroutine("Haste");
                AudioAndModelChange(true);
            }
            else if (OrbType == Orb.TripleDamage && !PlayerChar.IsTripleDamage)
            {
                StartCoroutine("TripleDamage");
                AudioAndModelChange(true);
            } else if (OrbType == Orb.Shotgun)
            {
                StartCoroutine("Shotgun");
                AudioAndModelChange(false);
            }

         
        }
    }

    private void AudioAndModelChange(bool respawn)
    {
        PlayerAudio.pitch = 1.66f;
        PlayerAudio.PlayOneShot(PickUp);
        PowerModel.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        if(respawn)StartCoroutine("RespawnPowerup");
    }

    IEnumerator Haste()
    {
        HasteText.gameObject.SetActive(true);
        PlayerStats.BaseSpeed *= 2;
        PlayerChar.IsHaste = true;
        yield return new WaitForSeconds(duration);
        PlayerStats.BaseSpeed /= 2;
        PlayerChar.IsHaste = false;
        HasteText.gameObject.SetActive(false);
        //Destroy(gameObject);
    }
      IEnumerator ActivateInvincibility()
    {
        //int tempHealth = PlayerStats.health;
        InvinText.gameObject.SetActive(true);
        PlayerChar.IsInvincible = true;
        yield return new WaitForSeconds(duration);
        PlayerChar.IsInvincible = false;
        InvinText.gameObject.SetActive(false);
        
        //Destroy(gameObject);
    }

    IEnumerator TripleDamage()
    {
        PlayerChar.IsTripleDamage = true;
        TripleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        PlayerChar.IsTripleDamage = false;
        TripleText.gameObject.SetActive(false);
        //Detroy(gameObject);
    }

    IEnumerator RefillHealth(bool Full)
    {
        if (!Full) {
            HPText.text = "Full Health";
        HPText.gameObject.SetActive(true);
        PlayerChar.Instance.health = PlayerChar.Instance.maxHealth;
        uiManager.PlayerUIState.UpdatePlayerHealth(PlayerChar.Instance.health, PlayerChar.Instance.maxHealth);
        yield return new WaitForSeconds(3);
        HPText.gameObject.SetActive(false);
        } else
        {
            HPText.text = "Health Already Full";
            HPText.gameObject.SetActive(true);
                   
            yield return new WaitForSeconds(3);
            HPText.gameObject.SetActive(false);
        
        }
        //Destroy(gameObject);
    }
    IEnumerator Shotgun()
    {
        ShotgunText.text = "Shotgun Aquired, press [E] to Swap Weapons.";
        PlayerWeapons.HasShotgun = true;
        yield return new WaitForSeconds(5); 
        ShotgunText.text = "";
        ShotgunText.gameObject.SetActive(false);
    }

    IEnumerator RespawnPowerup() {

        
        yield return new WaitForSeconds(RespawnTime);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        PowerModel.SetActive(true);

    }
}
