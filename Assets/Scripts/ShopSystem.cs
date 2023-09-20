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
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Shop initiated");
        UpdateCoinUI();
        UpdateButtonInteractivity();
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
}
