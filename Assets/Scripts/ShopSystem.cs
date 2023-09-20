using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{

    public int fireRateLevel = 1;
    public int speedLevel = 1;
    public int damageLevel = 1;
    public int shipLevel = 1;

    public int fireRateCost = 2;
    public int speedCost = 1;
    public int damageCost = 2;
    public int shipCost = 3;

    public Button upgradeFireRate;
    public Button upgradeDamage;
    public Button upgradeSpeed;
    public Button upgradeShip;
    public ShipShooting fireRate;
    public ShipMovement speed;
    public ShipMovement boostedSpeed;
    public PlayerBullet damage;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI fireRateUpgradeText;
    public TextMeshProUGUI speedUpgradeText;
    public TextMeshProUGUI damageUpgradeText;
    public TextMeshProUGUI shipUpgradeText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shop initiated");
        UpdateCoinUI();
        UpdateButtonInteractivity();
        UpdateShopTexts();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Updating buttons");
        UpdateButtonInteractivity();
    }

    public bool CanAffordUpgrade(int cost)
    {
        return PlayerCoins.coins >= cost;
    }

    public void PurchaseFireRateUpgrade()
    {
        Debug.Log("Upgrade purchased");
        if (CanAffordUpgrade(fireRateCost) && fireRateLevel < 5)
        {
            PlayerCoins.coins -= fireRateCost;
            fireRateLevel++;
            
            // Apply the fire rate upgrade logic
            fireRate.shootCooldown -= 0.25f;

            UpdateCoinUI();
            UpdateButtonInteractivity();
            UpdateShopTexts();
        }
    }

    public void PurchaseSpeedUpgrade()
    {
        Debug.Log("Upgrade purchased");
        if (CanAffordUpgrade(speedCost) && speedLevel < 5)
        {
            PlayerCoins.coins -= speedCost;
            speedLevel++;
            
            // Apply the fire rate upgrade logic
            speed.speed += 2.0f;
            boostedSpeed.boostedSpeed += 2.0f;

            UpdateCoinUI();
            UpdateButtonInteractivity();
            UpdateShopTexts();
        }
    }

    public void PurchaseDamageUpgrade()
    {
        Debug.Log("Upgrade purchased");
        if (CanAffordUpgrade(damageCost) && damageLevel < 5)
        {
            PlayerCoins.coins -= damageCost;
            damageLevel++;
            
            // Apply the fire rate upgrade logic
            damage.damage += 20;

            UpdateCoinUI();
            UpdateButtonInteractivity();
            UpdateShopTexts();
        }
    }

    public void PurchaseShipUpgrade()
    {
        Debug.Log("Upgrade purchased");
        if (CanAffordUpgrade(shipCost) && shipLevel < 3)
        {
            PlayerCoins.coins -= shipCost;
            shipLevel++;
            
            // Apply the fire rate upgrade logic

            UpdateCoinUI();
            UpdateButtonInteractivity();
        }
    }

    // Similar methods for each upgrade

    public void UpdateCoinUI()
    {
        coinText.text = PlayerCoins.coins.ToString();
    }

    public void UpdateButtonInteractivity()
    {
        upgradeFireRate.interactable = CanAffordUpgrade(fireRateCost) && fireRateLevel < 5;
        upgradeSpeed.interactable = CanAffordUpgrade(speedCost) && speedLevel < 5;
        upgradeDamage.interactable = CanAffordUpgrade(damageCost) && damageLevel < 5;
        upgradeShip.interactable = CanAffordUpgrade(shipCost) && shipLevel < 3;
    }

    public void UpdateShopTexts()
    {
        // Fire Rate Text
        if (fireRateLevel < 5)
        {
            float currentFireRateValue = fireRate.shootCooldown;
            float nextFireRateValue = currentFireRateValue - 0.25f;
            fireRateUpgradeText.text = $"{currentFireRateValue}s > {nextFireRateValue}s";
        }
        else
        {
            fireRateUpgradeText.text = "Max Level";
        }

        // Speed Text
        if (speedLevel < 5)
        {
            float currentSpeedValue = speed.speed;
            float nextSpeedValue = currentSpeedValue + 2.0f;
            speedUpgradeText.text = $"{currentSpeedValue} > {nextSpeedValue}";
        }
        else
        {
            speedUpgradeText.text = "Max Level";
        }

        // Damage Text
        if (damageLevel < 5)
        {
            float currentDamageValue = damage.damage;
            float nextDamageValue = currentDamageValue + 20;
            damageUpgradeText.text = $"{currentDamageValue} > {nextDamageValue}";
        }
        else
        {
            fireRateUpgradeText.text = "Max Level";
        }

        // Ship Text
        if (shipLevel < 5)
        {
            float currentFireRateValue = fireRate.shootCooldown;
            float nextFireRateValue = currentFireRateValue - 0.25f;
            fireRateUpgradeText.text = $"{currentFireRateValue}s > {nextFireRateValue}s";
        }
        else
        {
            fireRateUpgradeText.text = "Max Level";
        }
    }
}
