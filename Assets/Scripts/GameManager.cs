using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    // Game Settings
    public bool gameOver;
    public bool playerWins;
    public bool enemyWins;
    public GameObject resetUI;

    // Player's Stats
    // Player's Health Point
    public float playerHealth;
    public float playerMaxHealth = 100;
    public TMP_Text playerHPText;
    public TMP_Text playerMaxHPText;
    public HealthBarScript healthBar;

    // Player's Special Point
    public float playerSpecial;
    public float playerMaxSpecial = 100;
    public TMP_Text playerSPText;
    public SpecialBarScript specialBar;
    public GameObject playerObject;

    // Enemy's Stats
    public float enemyHealth;
    public float enemyMaxHealth = 100;
    public TMP_Text enemyHPText;
    public float enemySpecial;
    public TMP_Text enemySPText;
    public GameObject enemyObject;
    public EnemyHealthBarScript enemyHealthBar;


    void Start()
    {
        Time.timeScale = 1f;

        playerHealth = playerMaxHealth;
        playerHPText.text = "" + Mathf.Round(playerHealth);
        healthBar.UpdateHealthBar(playerHealth, playerMaxHealth);
        playerMaxHPText.text = "/" + Mathf.Round(playerMaxHealth);

        playerSpecial = playerMaxSpecial;
        playerSPText.text = "" + Mathf.Round(playerSpecial);
        specialBar.UpdateSpecialhBar(playerSpecial, playerMaxSpecial);

        enemyHealth = enemyMaxHealth;
        enemyHPText.text = "HP: " + Mathf.Round(enemyHealth);
        enemyHealthBar.UpdateEnemyHealthBar(enemyHealth, enemyMaxHealth);

        enemySpecial = 100;
        enemySPText.text = "SP: " + Mathf.Round(enemySpecial);

        playerWins = false;
        enemyWins = false;

        gameOver = false;
        resetUI.SetActive(false);  
    }

    void Update()
    {
        if (playerHealth >= 100)
        {
            playerHealth = 100;
            playerHPText.text = "" + Mathf.Round(playerHealth);
            healthBar.UpdateHealthBar(playerHealth, playerMaxHealth);
        }
        if (playerHealth > 0)
        {
            BattleSystemScript.Instance.playerAnimate.SetBool("noHealth", false);
        }
        if (playerSpecial >= 100)
        {
            playerSpecial = 100;
            playerSPText.text = "" + Mathf.Round(playerSpecial);
            specialBar.UpdateSpecialhBar(playerSpecial, playerMaxSpecial);
        }
        if (playerSpecial <= 0)
        {
            playerSpecial = 0;
            playerSPText.text = "" + Mathf.Round(playerSpecial);
            specialBar.UpdateSpecialhBar(playerSpecial, playerMaxSpecial);
        }

        if (enemyHealth >= 100)
        {
            enemyHealth = 100;
            enemyHPText.text = "HP " + Mathf.Round(enemyHealth);
            enemyHealthBar.UpdateEnemyHealthBar(enemyHealth, enemyMaxHealth);
        }
        if (enemyHealth > 0)
        {
            BattleSystemScript.Instance.enemyAnimate.SetBool("noHealth", false);
        }
        if (enemySpecial >= 100)
        {
            enemySpecial = 100;
            enemySPText.text = "SP: " + Mathf.Round(enemySpecial);
        }
        if (enemySpecial <= 0)
        {
            enemySpecial = 0;
            enemySPText.text = "SP: " + Mathf.Round(enemySpecial);
            
        }

        // Win conidtions
     
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            playerHPText.text = "" + Mathf.Round(playerHealth);
            healthBar.UpdateHealthBar(playerHealth, playerMaxHealth);
            enemyWins = true;

            BattleSystemScript.Instance.playerAnimate.SetBool("noHealth", true);
            BattleSystemScript.Instance.playerTurn = false;
            BattleSystemScript.Instance.enemyTurn = false; 

            //Destroy(playerObject);
        }
        if (enemyHealth <= 0)
        {
            enemyHealth = 0;
            enemyHPText.text = "HP: " + Mathf.Round(enemyHealth);
            GameManager.Instance.enemyHealthBar.UpdateEnemyHealthBar(GameManager.Instance.enemyHealth, GameManager.Instance.enemyMaxHealth);
            playerWins = true;

            BattleSystemScript.Instance.enemyAnimate.SetBool("noHealth", true);
            BattleSystemScript.Instance.playerTurn = false;
            BattleSystemScript.Instance.enemyTurn = false;

            // Death Impact Effect will be created
            //GameObject deathImpact = Instantiate(BattleSystemScript.Instance.deathEffectEnemy, BattleSystemScript.Instance.deathEnemyPosition.position, Quaternion.identity);
         
            

            //Destroy(enemyObject);
        }

        if (playerWins == true)
        {
            BattleSystemScript.Instance.playerAnimate.SetBool("victoryDance", true);
            StartCoroutine(GameOverWaitDelay());

            // Player Victory Music
        }
        if (enemyWins == true)
        {
            BattleSystemScript.Instance.enemyAnimate.SetBool("victoryDance", true);
            StartCoroutine(GameOverWaitDelay());

            // Enemy Victory Music
        }

        if (gameOver == true)
        {
            Time.timeScale = 0f;
            resetUI.SetActive(true);
        }

  

    }

    private IEnumerator GameOverWaitDelay()
    {
        yield return new WaitForSeconds(4);
        gameOver = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
    void Awake()
    {
        instance = this;
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
}
