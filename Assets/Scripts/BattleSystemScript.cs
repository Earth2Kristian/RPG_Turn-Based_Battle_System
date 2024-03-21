using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Random = UnityEngine.Random;


public class BattleSystemScript : MonoBehaviour
{
    private static BattleSystemScript instance = null;

    // Camera
    public GameObject camStart;
    public Animator camAnimate;
    public GameObject camChange;
    public bool camChangeCondition;

    // Commands
    public GameObject playerAttackUI;
    public GameObject playerUsesFireUI;
    public GameObject playerUsesHealUI;
    public GameObject enemyAttackUI;
    public GameObject enemyUsesFireUI;

    // Player
    public bool playerTurn;
    public bool playerTookTheTurn;
    public bool playerAttacked;
    public bool playerAttackedAnimation;
    public bool playerFireAttacked;
    public bool playerFireSpellAnimation;
    public bool playerHeal;
    public bool playerHealAnimation;
    public Animator playerAnimate;
    public GameObject playerTurnUI;
    public GameObject playerSpellUI;

    // Fire (From Player)
    public GameObject FireObject;
    public Rigidbody FireRB;
    public Transform FirePosition;

    // Heal (From Player)
    public GameObject HealObject;
    public Transform HealPosition;
    public GameObject playerHealedUI;

    // Enemy
    public bool enemyTurn;
    public bool enemyAttacked;
    public bool enemyAttackedAnimation;
    public bool enemyFireAttacked;
    public bool enemyFireSpellAnimation;
    public bool enemyHeal;
    public bool enemyHealAnimation;
    public Animator enemyAnimate;
    public int enemyChoice;

    // Fire (From Enemy)
    public GameObject FireObjectEnemy;
    public Rigidbody FireRBEnemy;
    public Transform FirePositionEnemy;

    // Impact Effect on Player
    public GameObject impactEffectOnPlayer;
    public Transform impactPlayerPosition;
    public GameObject impactEffectOnPlayer2;
    public Transform impactPlayerPosition2;
    public float playerDamageTaken;
    public float playerDamageTakenCrit;

    // Impact Effect on Enmey
    public GameObject impactEffectOnEnemy;
    public Transform impactEnemyPosition;
    public GameObject impactEffectOnEnemy2;
    public Transform impactEnemyPosition2;
    public GameObject deathEffectEnemy;
    public Transform deathEnemyPosition;
    public bool deathEffectPlayed;
    public float enemyDamageTaken;
    public float enemyDamageTakenCrit;

        
    // Random Chance of A Cirtial Hit
    public int randomCritChanceForPlayer;
    public int randomCritChanceForEnemy;
    public GameObject CritHitUI;

    // Floating Text
    public Transform playerFloatingTextPosition;
    public Transform enemyFloatingTextPosition;
    public GameObject floatingText;

    void Start()
    {
        camStart.SetActive(true);
        camChange.SetActive(false);
        camChangeCondition = false;

        // Player gets the first move
        playerAttackUI.SetActive(false);
        playerUsesFireUI.SetActive(false); 
        playerUsesHealUI.SetActive(false);
        playerHealedUI.SetActive(false);
        enemyAttackUI.SetActive(false);
        enemyUsesFireUI.SetActive(false);   
        CritHitUI.SetActive(false);

        playerSpellUI.SetActive(false);

        // Player will got first
        playerTurn = true;
        playerTookTheTurn = false;
        enemyTurn = false;

        // Got Hit Animation will be set to false from the start
        playerAnimate.SetBool("gotHit", false);
        enemyAnimate.SetBool("gotHit", false);

        // All Attack and Attacking will be set to false from the start
        playerAttacked = false;
        playerAttackedAnimation = false;
        playerFireAttacked = false;
        playerFireSpellAnimation = false;
        playerHeal = false;
        playerHealAnimation = false;

        enemyAttacked = false;

        // Enemy Has choices to make for a turn from the start
        enemyChoice = Random.Range(1, 3);

        // There could be a chance for the player or the enemy to get a crit from the first turn
        randomCritChanceForPlayer = Random.Range(1, 6);
        randomCritChanceForEnemy = Random.Range(1, 6);
        
        
        deathEffectPlayed = false;                                                                                                    
    }

    void Update()
    {
       

        // Player's Turn 
        if (playerTurn)
        {
            playerTurnUI.SetActive(true);
            playerAnimate.SetBool("isAttacking", false);
            playerAnimate.SetBool("isFireAttack", false);
            randomCritChanceForPlayer = Random.Range(1, 6);
         

            // Genreating of how much damage that the enemy will take when the player uses its turn
            enemyDamageTaken = Random.Range(8, 12);
            enemyDamageTakenCrit = Random.Range(16, 24);

            if (playerTookTheTurn == false)
            {
                // Camera will change when player is still waiting to attack

                if (camChangeCondition == false)
                {
                    StartCoroutine(CamChange());
                    camChange.SetActive(false);
                    camStart.SetActive(true);
                    
                    
                }
                else if (camChangeCondition == true)
                {
                    StartCoroutine(CamReset());
                    camChange.SetActive(true);
                    camStart.SetActive(false);
                    
                }
                
            }
            else if (playerTookTheTurn == true)  
            {
                camChange.SetActive(false);
                camStart.SetActive(true);
                
            }

           
        }

        // If player has choose to physically attack
        if (playerAttacked == true)
        {
            playerAttackUI.SetActive(true);

            StartCoroutine(ImpactEffectOnEnemy());

            playerTookTheTurn = true;
            playerAttacked = false;
            playerTurn = false;
            StartCoroutine(TurnDelayPlayer());
        }

        // If player has choose to cast a spell
        if (playerFireAttacked == true)
        {
            playerUsesFireUI.SetActive(true) ;
            ShootFire();
            playerTookTheTurn = true;
            GameManager.Instance.playerSpecial -= 8;
            GameManager.Instance.playerSPText.text = "" + Mathf.Round(GameManager.Instance.playerSpecial);
            GameManager.Instance.specialBar.UpdateSpecialhBar(GameManager.Instance.playerSpecial, GameManager.Instance.playerMaxSpecial);
            playerFireAttacked = false;
            playerTurn = false;
            StartCoroutine(TurnDelayPlayer());
        }
        if (playerHeal == true)
        {
            playerUsesHealUI.SetActive(true) ;
            HealOnPlayer();
            playerTookTheTurn = true;
            GameManager.Instance.playerSpecial -= 8;
            GameManager.Instance.playerSPText.text = "" + Mathf.Round(GameManager.Instance.playerSpecial);
            GameManager.Instance.specialBar.UpdateSpecialhBar(GameManager.Instance.playerSpecial, GameManager.Instance.playerMaxSpecial);
            playerHeal = false;
            playerTurn = false;
            StartCoroutine(TurnDelayPlayer());
        }

        // Enemy's Turn
        if (enemyTurn)
        {
            // Genreating of how much damage that the player will take when the enemy uses its turn
            playerDamageTaken = Random.Range(8, 12);
            playerDamageTakenCrit = Random.Range(16, 24);

            // Enemy will do something different for each turn
            if (enemyChoice <= 1 || GameManager.Instance.enemySpecial < 8)
            {
                enemyAttackUI.SetActive(true);
                enemyAttacked = true;
                enemyAttackedAnimation = true;
                enemyAnimate.SetBool("isAttacking", false);
            }
            if (enemyChoice >= 2 && GameManager.Instance.enemySpecial >= 8)
            {
                enemyUsesFireUI.SetActive(true);    
                enemyFireAttacked = true;
                enemyFireSpellAnimation = true;
                enemyAnimate.SetBool("isFireAttack", false);
            }
            enemyChoice = Random.Range(1, 3);
            randomCritChanceForEnemy = Random.Range(1, 6);

        }

        // If enemy has choose to physically attack
        if (enemyAttacked)
        {
            StartCoroutine(ImpactEffectOnPlayer());

            enemyAttacked = false;
            enemyTurn = false;
            StartCoroutine(TurnDelayEnemy());
        }

        // If enemy has choose to cast a spell
        if (enemyFireAttacked == true)
        {
            ShootFireEnemy();
            GameManager.Instance.enemySpecial -= 8;
            GameManager.Instance.enemySPText.text = "SP: " + Mathf.Round(GameManager.Instance.enemySpecial);
            enemyFireAttacked = false;
            enemyTurn = false;
            StartCoroutine(TurnDelayEnemy());
        }

        // Animations Plays if commanded for Player
        if (playerAttackedAnimation == true)
        {
            StartCoroutine(PlayerAttacked());
            playerTurnUI.SetActive(false);
            playerAnimate.SetBool("isAttacking", true);
        }
        if (playerFireSpellAnimation == true)
        {
            StartCoroutine(PlayerAttacked());
            playerTurnUI.SetActive(false);
            playerSpellUI.SetActive(false);
            playerAnimate.SetBool("isFireAttack", true );
        }
        if (playerHealAnimation == true)
        {
            StartCoroutine(PlayerAttacked());
            playerTurnUI.SetActive(false);
            playerSpellUI.SetActive(false);
            playerAnimate.SetBool("isHealing", true);
        }

        // Animations Plays if commanded for Enemy
        if (enemyAttackedAnimation == true)
        {
            StartCoroutine(EnemyAttacked());
            enemyAnimate.SetBool("isAttacking", true);
        }
        if (enemyFireSpellAnimation == true)
        {
            StartCoroutine(EnemyAttacked());
            enemyAnimate.SetBool("isFireAttack", true);
        }

        // When hit animations has ended for either the player or the enemy
        if (playerAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && playerAnimate.GetCurrentAnimatorStateInfo(0).IsName("gotHit"))
        {
            playerAnimate.SetBool("gotHit", false);

        }
        if (enemyAnimate.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && enemyAnimate.GetCurrentAnimatorStateInfo(0).IsName("gotHit"))
        {
            enemyAnimate.SetBool("gotHit", false);

        }

    }

    // When it's Player's Turn
    private IEnumerator PlayerAttacked()
    {
        yield return new WaitForSeconds(1);
        playerAttackUI.SetActive(false);
        playerUsesFireUI.SetActive(false);
        playerUsesHealUI.SetActive(false);
        playerAttackedAnimation = false;
        playerFireSpellAnimation = false;
        playerHealAnimation = false;
        playerAnimate.SetBool("isAttacking", false);
        playerAnimate.SetBool("isFireAttack", false);
        playerAnimate.SetBool("isHealing", false);
    }

    private IEnumerator ImpactEffectOnEnemy()
    {
        yield return new WaitForSeconds(1);
        if (randomCritChanceForPlayer < 5)
        {
            GameManager.Instance.enemyHealth -= enemyDamageTaken;
            GameManager.Instance.enemyHPText.text = "HP: " + Mathf.Round(GameManager.Instance.enemyHealth);
            GameManager.Instance.enemyHealthBar.UpdateEnemyHealthBar(GameManager.Instance.enemyHealth, GameManager.Instance.enemyMaxHealth);
            enemyAnimate.SetBool("gotHit", true) ;

            // Floating text will appear above the enemy
            Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
            GameObject ft = Instantiate(floatingText, enemyFloatingTextPosition.position, floatingTextRotation);
            ft.GetComponent<TextMesh>().text = enemyDamageTaken.ToString();
        }
        if (randomCritChanceForPlayer >= 5)
        {
            GameManager.Instance.enemyHealth -= enemyDamageTakenCrit;
            GameManager.Instance.enemyHPText.text = "HP: " + Mathf.Round(GameManager.Instance.enemyHealth);
            GameManager.Instance.enemyHealthBar.UpdateEnemyHealthBar(GameManager.Instance.enemyHealth, GameManager.Instance.enemyMaxHealth);
            enemyAnimate.SetBool("gotHit", true);
            CritHitUI.SetActive(true);
            camAnimate.SetBool("CameraShakeTrigger", true);
            StartCoroutine(CameraShakeDelay());
            Debug.Log("Crit Hit");

            // Floating text will appear above the enemy
            Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
            GameObject ft = Instantiate(floatingText, enemyFloatingTextPosition.position, floatingTextRotation);
            ft.GetComponent<TextMesh>().text = enemyDamageTakenCrit.ToString();
        }
        // Impact effect will play
        GameObject impact = Instantiate(impactEffectOnEnemy, impactEnemyPosition.position, Quaternion.identity);
        GameObject impact2 = Instantiate(impactEffectOnEnemy2, impactEnemyPosition2.position, Quaternion.identity); 

        


    }

    // When it's Enemy turn
    private IEnumerator EnemyAttacked()
    {
        yield return new WaitForSeconds(1);
        enemyAttackUI.SetActive(false);
        enemyUsesFireUI.SetActive(false);  
        enemyAttackedAnimation = false;
        enemyFireSpellAnimation = false;
        enemyHealAnimation = false;
        enemyAnimate.SetBool("isAttacking", false);
        enemyAnimate.SetBool("isFireAttack", false) ;
        enemyAnimate.SetBool("isHealing", false);
    }

    private IEnumerator ImpactEffectOnPlayer()
    {
        yield return new WaitForSeconds(1);
        if (randomCritChanceForEnemy < 5)
        {
            GameManager.Instance.playerHealth -= playerDamageTaken;
            GameManager.Instance.playerHPText.text = "" + Mathf.Round(GameManager.Instance.playerHealth);
            GameManager.Instance.healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
            playerAnimate.SetBool("gotHit", true);

            // Floating text will appear above the player
            Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
            GameObject ft = Instantiate(floatingText, playerFloatingTextPosition.position, floatingTextRotation) ;
            ft.GetComponent<TextMesh>().text = playerDamageTaken.ToString();

        }

        if (randomCritChanceForEnemy >= 5)
        {
            GameManager.Instance.playerHealth -= playerDamageTakenCrit;
            GameManager.Instance.playerHPText.text = "" + Mathf.Round(GameManager.Instance.playerHealth);
            GameManager.Instance.healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
            playerAnimate.SetBool("gotHit", true);
            CritHitUI.SetActive(true);
            camAnimate.SetBool("CameraShakeTrigger", true);
            StartCoroutine(CameraShakeDelay());
            Debug.Log("Crit Hit");

            // Floating text will appear above the player
            Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
            GameObject ft = Instantiate(floatingText, playerFloatingTextPosition.position, floatingTextRotation);
            ft.GetComponent<TextMesh>().text = playerDamageTakenCrit.ToString();
        }

        GameObject impact = Instantiate(impactEffectOnPlayer, impactPlayerPosition.position, Quaternion.identity);
        GameObject impact2 = Instantiate(impactEffectOnPlayer2, impactPlayerPosition2.position, Quaternion.identity);

    }

    private IEnumerator TurnDelayPlayer()
    {
        yield return new WaitForSeconds(2);
        CritHitUI.SetActive(false);
        playerHealedUI.SetActive(false);
        enemyTurn = true;   
    }
    private IEnumerator TurnDelayEnemy()
    {
        yield return new WaitForSeconds(2);
        CritHitUI.SetActive(false);
        playerTurn = true;
        playerTookTheTurn = false;
         
    }
    public void PlayerAttack()
    {
        playerAttacked = true;
        playerAttackedAnimation = true;

        // Camera will reset when player do attacks
        playerTookTheTurn = true;

    }
    public void PlayerSpell()
    {
        if (GameManager.Instance.playerSpecial >= 8)
        {
            playerSpellUI.SetActive(true);
        }
        
    }

    public void PlayerFireSpell()
    {
        playerFireAttacked = true;
        playerFireSpellAnimation = true;

        // Camera will reset when player attacks
        playerTookTheTurn = true;
    }
    public void ShootFire()
    {
        GameObject fireSpell = Instantiate(FireObject, FirePosition.position, Quaternion.identity);
        fireSpell.GetComponent<Rigidbody>().AddForce(FirePosition.forward * 600);
    }
    public void ShootFireEnemy()
    {
        GameObject fireSpell = Instantiate(FireObjectEnemy, FirePositionEnemy.position, Quaternion.identity);
        fireSpell.GetComponent<Rigidbody>().AddForce(FirePositionEnemy.forward * 600);
    }

    public void PlayerHealSpell()
    {
        playerHeal = true;
        playerHealAnimation = true;

        // Camera will reset when player heals
        playerTookTheTurn = true;
    }
    public void HealOnPlayer()
    {
        // Will Heal the Player
        Quaternion healRotation = Quaternion.Euler(-90, 0, 0);
        GameObject healSpell = Instantiate(HealObject, HealPosition.position, healRotation);
        healSpell.transform.localScale = new Vector3(8, 8, 8);
        StartCoroutine(HealingDelay());
        Debug.Log("Player Healed");
    }

    public IEnumerator HealingDelay()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.playerHealth += Random.Range(15, 20);
        GameManager.Instance.playerHPText.text = "" + Mathf.Round(GameManager.Instance.playerHealth);
        GameManager.Instance.healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
        playerHealedUI.SetActive(true);
    }

    public void RemoveSpell()
    {
        playerSpellUI.SetActive(false);
    }

    public IEnumerator CamResetWhenAttacking()
    {
        yield return new WaitForSeconds(0f);
        camChangeCondition = false;
    }

    public IEnumerator CamChange()
    {
        yield return new WaitForSeconds(10f);
        camChangeCondition = true;
    }
    public IEnumerator CamReset()
    {
        yield return new WaitForSeconds(10f);
        camChangeCondition = false; 
    }

    public IEnumerator CameraShakeDelay()
    {
        yield return new WaitForSeconds(0.5f);
        camAnimate.SetBool("CameraShakeTrigger", false);
    }

    void Awake()
    {
        instance = this;
    }

    public static BattleSystemScript Instance
    {
        get
        {
            return instance;
        }
    }
}
