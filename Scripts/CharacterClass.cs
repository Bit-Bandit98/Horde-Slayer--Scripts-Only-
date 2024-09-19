using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [SerializeField]
   public int health, maxHealth;
    [SerializeField]
    public float BaseSpeed;
    protected bool IsDead;
    [SerializeField]
    protected Renderer CharacterModel;
    Material[] mat;

    private void Awake()
    {
        mat = CharacterModel.materials;
    }

    //Constructor 
    public CharacterClass()
    {
        health = 10;
        BaseSpeed = 5;
    }
    //Custom Constructor
    public CharacterClass(int Health, int Speed)
    {
        health = Health;
        BaseSpeed = Speed;
    }

    public virtual void takeDamage(int damage)
    {
         
        health -= damage;
        if(mat != null) StartCoroutine("FlashRed");
        if(health <= 0)
        {
            IsDead = true;
            die();    
        }
        
    }

    public virtual void die()
    {
        Destroy(gameObject);
    }

    public virtual IEnumerator FlashRed()
    {
        for(int i = 0; i<mat.Length; i++) { 
        mat[i].EnableKeyword("_EMISSION");
        mat[i].SetColor("_EmissionColor", Color.red);
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i< mat.Length; i++) { 
        mat[i].SetColor("_EmissionColor", Color.black);
        mat[i].DisableKeyword("_EMISSION");
        }

    }
   
}
