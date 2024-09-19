using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
   
    [SerializeField]
    Slider EnemyHealthBar;
    [SerializeField]
    BasicEnemy EnemyRef;
    [SerializeField]
     Image AttackIcon, IdleIcon, DeadIcon;
    [SerializeField]
    TMPro.TextMeshProUGUI EnemyHealth;
    Quaternion DefaultRotation = new Quaternion(0, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthBar(EnemyRef.ReturnHealth());
        UpdateHealthText(EnemyRef.health, EnemyRef.maxHealth);
       
    }

    // Update is called once per frame
    void Update()
    {
        //Billboards the Enemy's Interface.
        BillBoardUI();



        // EnemyHealthBar.transform.LookAt(Camera.main.transform);
    }

    private void BillBoardUI()
    {
        EnemyHealthBar.transform.rotation = DefaultRotation;
        AttackIcon.transform.rotation = DefaultRotation;
        IdleIcon.transform.rotation = DefaultRotation;
        DeadIcon.transform.rotation = DefaultRotation;
    }

    public void UpdateEnemyState(int State)
    {
        if(State == 0)
        {
            AttackIcon.gameObject.SetActive(false)  ;
            IdleIcon.gameObject.SetActive(true);
        } else if(State == 1)
        {
            AttackIcon.gameObject.SetActive(true);
            IdleIcon.gameObject.SetActive(false);
        } else if (State == 2)
        {
            AttackIcon.gameObject.SetActive(false);
            IdleIcon.gameObject.SetActive(false);
            DeadIcon.gameObject.SetActive(true);
        }
    }
    public void UpdateHealthBar(float percentage)
    {
        EnemyHealthBar.value = percentage;

    }
    public void UpdateHealthText(int CurrentHP, int MaxHP)
    {
        EnemyHealth.text = CurrentHP + "/" + MaxHP;

    }

   
}
