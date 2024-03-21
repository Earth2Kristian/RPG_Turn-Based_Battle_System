using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollideScript : MonoBehaviour
{
    public GameObject afterEffect;
    public Transform afterEffectPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpellEnemy"))
        {
            if (BattleSystemScript.Instance.randomCritChanceForEnemy < 5)
            {
                GameManager.Instance.playerHealth -= BattleSystemScript.Instance.playerDamageTaken;
                GameManager.Instance.playerHPText.text = "" + Mathf.Round(GameManager.Instance.playerHealth);
                GameManager.Instance.healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
                GameObject explosion = Instantiate(afterEffect, afterEffectPosition.position, Quaternion.identity);
                BattleSystemScript.Instance.playerAnimate.SetBool("gotHit", true);

                // Floating text will appear above the player
                Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
                GameObject ft = Instantiate(BattleSystemScript.Instance.floatingText, BattleSystemScript.Instance.playerFloatingTextPosition.position, floatingTextRotation);
                ft.GetComponent<TextMesh>().text = BattleSystemScript.Instance.playerDamageTaken.ToString();
            }
            if (BattleSystemScript.Instance.randomCritChanceForEnemy >= 5)
            {
                GameManager.Instance.playerHealth -= BattleSystemScript.Instance.playerDamageTakenCrit;
                GameManager.Instance.playerHPText.text = "" + Mathf.Round(GameManager.Instance.playerHealth);
                GameManager.Instance.healthBar.UpdateHealthBar(GameManager.Instance.playerHealth, GameManager.Instance.playerMaxHealth);
                GameObject explosion = Instantiate(afterEffect, afterEffectPosition.position, Quaternion.identity);
                BattleSystemScript.Instance.playerAnimate.SetBool("gotHit", true);
                BattleSystemScript.Instance.CritHitUI.SetActive(true);
                BattleSystemScript.Instance.camAnimate.SetBool("CameraShakeTrigger", true);
                StartCoroutine(BattleSystemScript.Instance.CameraShakeDelay());
                Debug.Log("Crit Hit");

                // Floating text will appear above the player
                Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
                GameObject ft = Instantiate(BattleSystemScript.Instance.floatingText, BattleSystemScript.Instance.playerFloatingTextPosition.position, floatingTextRotation);
                ft.GetComponent<TextMesh>().text = BattleSystemScript.Instance.playerDamageTaken.ToString();
            }
            
        }
    }
}
