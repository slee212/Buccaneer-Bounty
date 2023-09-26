using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthbarImage;
    public const float MaxHealthbarWidth = 601f; // Set this to the maximum width of your enemy's health bar

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float currentHealthPercentage = currentHealth / maxHealth;
        float newWidth = MaxHealthbarWidth * currentHealthPercentage;
        float currentHeight = healthbarImage.rectTransform.rect.height;
        healthbarImage.rectTransform.sizeDelta = new Vector2(newWidth, currentHeight);

        // Change color based on health percentage
        if (currentHealthPercentage <= 0.25f)
        {
            healthbarImage.color = new Color(255f / 255f, 88f / 255f, 94f / 255f, 1f); // Red
        }
        else
        {
            healthbarImage.color = new Color(134f / 255f, 255f / 255f, 141f / 255f, 1f); // Green
        }
    }
}
