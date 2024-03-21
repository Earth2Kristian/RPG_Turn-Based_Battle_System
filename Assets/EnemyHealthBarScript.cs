using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{
    public Image enemyHealthBarImage;

    public void UpdateEnemyHealthBar(float currentHealth, float maxHealth)
    {
        enemyHealthBarImage.fillAmount = currentHealth / maxHealth;
    }
}
