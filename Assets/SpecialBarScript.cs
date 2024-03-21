using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialBarScript : MonoBehaviour
{
    public Image specialBarImage;

    public void UpdateSpecialhBar(float currentHealth, float maxHealth)
    {
        specialBarImage.fillAmount = currentHealth / maxHealth;
    }

}
