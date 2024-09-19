using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : CharacterClass
{
    

    [SerializeField]
    Rigidbody EnemyRigi;
   
    [SerializeField]
    EnemyUI EnemyInterface;

    //How far the Enemy can see when not chasing
    [SerializeField]
    float ViewRange;

    [SerializeField]
    public int experienceYield;
    public static bool isAttacking;
    
    [SerializeField]
    GameObject FloatingText;
    [SerializeField]
    bool Chasing;
    enum EnemyStates { Idle, Attacking };
    EnemyStates State;
    [SerializeField]
    Animator EnemyAnimation;
    [SerializeField]
    public AudioSource EnemyAudio;

    [SerializeField]
    public AudioClip Groan, hit, Attacking;


    
    // Start is called before the first frame update
    void Start()
    {
        
        isAttacking = false;
        Chasing = false;
        State = EnemyStates.Idle;
        GM.GlobalGameManager.CountEnemy(1);
        StartCoroutine("ChasePlayer");
        //print("Enum Number is " +((int)EnemyStates.Idle));


    }
    public override void takeDamage(int damage)
    {
        if (!IsDead) { 
            //Display Damage number above enemy.
            GameObject BigText = Instantiate(FloatingText, EnemyInterface.transform.position - Vector3.up * 2, new Quaternion(0,0,0,1));
        BigText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
            //Play Damage Audio
        EnemyAudio.PlayOneShot(hit);
        EnemyAudio.pitch = Random.Range(0.9f, 1.2f);
        base.takeDamage(damage);


        if(!Chasing) StartCoroutine("ChasePlayer");
        //update Health bar of enemy.
        EnemyInterface.UpdateHealthBar(ReturnHealth());
            EnemyInterface.UpdateHealthText(health, maxHealth);
        }
    }


    public float ReturnHealth()
    {
        float temp = health;

        return temp / maxHealth;
    }

    
    // Update is called once per frame
    void Update()
    {
     //   if(!Chasing)LookAtPlayer();
    }

    public override void die()
    {
        //Play death animation
        EnemyAnimation.SetBool("IsDead", true);
        Destroy(gameObject, 3);
        PlayerChar.Instance.RecieveEXP(experienceYield);
        EnemyInterface.UpdateEnemyState(2);
        GM.GlobalGameManager.CountEnemy(-1);
    }
    void LookAtPlayer()
    {
        if(Vector3.Distance(transform.position, PlayerChar.Player.transform.position) <= ViewRange && !Chasing) {
    
            StartCoroutine("ChasePlayer");
        }

       
    }

    public IEnumerator ChasePlayer()
    {
        if (!Chasing) {
            
            EnemyAnimation.SetBool("IsChasing", true);
            Chasing = true;
            State = EnemyStates.Attacking;
            EnemyInterface.UpdateEnemyState((int)EnemyStates.Attacking);

        while (PlayerChar.Instance.IsAlive && health > 0)
        {
                //While the enemy's target is still alive, or the enemy itself is still alive
         
            transform.LookAt(PlayerChar.Player.transform);
                if(Vector3.Distance(gameObject.transform.position, PlayerChar.Player.transform.position) < 2    )
                {
                    EnemyAnimation.SetBool("Attack", true);
                    isAttacking = true;
                } else
                {   
                    EnemyAnimation.SetBool("Attack", false);
                    isAttacking = false;              
                        transform.position = Vector3.MoveTowards(transform.position, PlayerChar.Player.transform.position, BaseSpeed * Time.deltaTime);
                    
                }

               
              //  yield return new WaitForSeconds(0.2f);
            yield return null;
        }
        //All this occurs when either the player dies, or the enemy dies.
            Chasing = false;
            EnemyAnimation.SetBool("IsChasing", false);
            EnemyAnimation.SetBool("Attack", false);
            

            if (health > 0) EnemyInterface.UpdateEnemyState((int)EnemyStates.Idle);
            else if (health <= 0) EnemyInterface.UpdateEnemyState(2);
        }
    }

    //Displays the radius of how far an enemy can see for ease of understanding.
   // private void OnDrawGizmosSelected()
    //{
      //  Gizmos.color = Color.yellow;
       // Gizmos.DrawSphere(transform.position, ViewRange);
    //}
}
