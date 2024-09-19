using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeListener : MonoBehaviour
{
    //Get Enemy and their damage hitbox.
    [SerializeField]
    GameObject Hitbox, EnemyRef;

    
    public void DisplayHitBox()
    {
        Hitbox.gameObject.SetActive(true);
    }

    public void HideHitBox()
    {
        Hitbox.gameObject.SetActive(false);
    }

    //Play an audio cue should the hit land.
   public void PlayHitSound()
    {
        BasicEnemy ScriptRef = EnemyRef.GetComponent<BasicEnemy>();
        ScriptRef.EnemyAudio.pitch = Random.Range(0.8f, 1.20f);
        ScriptRef.EnemyAudio.PlayOneShot(ScriptRef.Attacking);
    }
}
