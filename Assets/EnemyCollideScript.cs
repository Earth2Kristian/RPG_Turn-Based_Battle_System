using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollideScript : MonoBehaviour
{
    public GameObject afterEffect;
    public Transform afterEffectPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spell"))
        {

            if (BattleSystemScript.Instance.randomCritChanceForPlayer < 5)
            {
                GameManager.Instance.enemyHealth -= BattleSystemScript.Instance.enemyDamageTaken;
                GameManager.Instance.enemyHPText.text = "HP: " + Mathf.Round(GameManager.Instance.enemyHealth);
                GameManager.Instance.enemyHealthBar.UpdateEnemyHealthBar(GameManager.Instance.enemyHealth, GameManager.Instance.enemyMaxHealth);
                GameObject explosion = Instantiate(afterEffect, afterEffectPosition.position, Quaternion.identity);
                BattleSystemScript.Instance.enemyAnimate.SetBool("gotHit", true);

                Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
                GameObject ft = Instantiate(BattleSystemScript.Instance.floatingText, BattleSystemScript.Instance.enemyFloatingTextPosition.position, floatingTextRotation);
                ft.GetComponent<TextMesh>().text = BattleSystemScript.Instance.enemyDamageTaken.ToString();
            }
            if (BattleSystemScript.Instance.randomCritChanceForPlayer >= 5)
            {
                GameManager.Instance.enemyHealth -= BattleSystemScript.Instance.enemyDamageTakenCrit;
                GameManager.Instance.enemyHPText.text = "HP: " + Mathf.Round(GameManager.Instance.enemyHealth);
                GameManager.Instance.enemyHealthBar.UpdateEnemyHealthBar(GameManager.Instance.enemyHealth, GameManager.Instance.enemyMaxHealth);
                GameObject explosion = Instantiate(afterEffect, afterEffectPosition.position, Quaternion.identity);
                BattleSystemScript.Instance.enemyAnimate.SetBool("gotHit", true);
                BattleSystemScript.Instance.CritHitUI.SetActive(true);
                BattleSystemScript.Instance.camAnimate.SetBool("CameraShakeTrigger", true);
                StartCoroutine(BattleSystemScript.Instance.CameraShakeDelay());
                Debug.Log("Crit Hit");

                Quaternion floatingTextRotation = Quaternion.Euler(0, -90, 0);
                GameObject ft = Instantiate(BattleSystemScript.Instance.floatingText, BattleSystemScript.Instance.enemyFloatingTextPosition.position, floatingTextRotation);
                ft.GetComponent<TextMesh>().text = BattleSystemScript.Instance.enemyDamageTakenCrit.ToString();
            }

        }
    }

}
