using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    [SerializeField]
    uiManager PlayerUI;
    int enemyCount;
    public static bool Victory = false;
    public static bool PlayerDead = false;
    public static GM GlobalGameManager;
    public static int Wave;
    public bool GameIsPaused;

    [SerializeField]
    GameObject  Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8, Wave9, Wave10;

    // Start is called before the first frame update
    void Awake()
    {
        Wave = 1;
        GameIsPaused = false;
        GlobalGameManager = this;
        PlayerDead = false;
        Victory = false;
        
    }

    private void Update()
    {
        if(Victory || PlayerDead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
               
            }

        }
    }


    public void CountEnemy(int num)
    {
        enemyCount += num;
        uiManager.PlayerUIState.UpdateEnemyCount(enemyCount);
        if (enemyCount <= 0 && Wave == 10)
        {
            Victory = true;
            uiManager.PlayerUIState.DisplayVictory(true);
        } else if (enemyCount == 0)
        {
            StartCoroutine("NextWave");
        }
    }

    public IEnumerator NextWave()
    {
        if(Wave == 10)
        {
            GM.Victory = true;
            uiManager.PlayerUIState.DisplayVictory(true);
            yield break;
        }
        uiManager.PlayerUIState.DisplayVictory(true);
        yield return new WaitForSeconds(5);
        Wave++;
        if (Wave == 2) Wave2.SetActive(true);
        if (Wave == 3) Wave3.SetActive(true);
        if (Wave == 4) Wave4.SetActive(true);
        if (Wave == 5) Wave5.SetActive(true);
        if (Wave == 6) Wave6.SetActive(true);
        if (Wave == 7) Wave7.SetActive(true);
        if (Wave == 8) Wave8.SetActive(true);
        if (Wave == 9) Wave9.SetActive(true);
        if (Wave == 10) Wave10.SetActive(true);
        uiManager.PlayerUIState.DisplayVictory(false);
        uiManager.PlayerUIState.UpdateWaveText();
    }
    

}
