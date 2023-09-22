using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopType
{
    FirstShop,
    SecondShop,
    ThirdShop
}

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
    public ShipShooting player;
    public ShipMovement speed;
    public ShipMovement boostedSpeed;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI fireRateUpgradeText;
    public TextMeshProUGUI speedUpgradeText;
    public TextMeshProUGUI damageUpgradeText;
    public TextMeshProUGUI shipUpgradeText;

    public ShopType currentShopType;

    public GameObject level1Ship;
    public GameObject level2Ship;
    public GameObject currentShipInstance;

    Vector3 startPosition = new Vector3(93, 1, 69);

    // Start is called before the first frame update
    void Start()
    {   
        Debug.Log(player.damage);
        Debug.Log("Shop initiated");
        UpdateCoinUI();
        UpdateButtonInteractivity();
        UpdateShopTexts();
    }

    // Update is called once per frame
    void Update()
    {
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
            player.shootCooldown -= 0.25f;

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
            player.damage += 20;

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
            
            if (shipLevel == 1)
            {

                // ShipShooting oldShipShooting = currentShipInstance.GetComponent<ShipShooting>();
                // ShipMovement oldShipMovement = currentShipInstance.GetComponent<ShipMovement>();

                Destroy(currentShipInstance);

                currentShipInstance = Instantiate(level2Ship, currentShipInstance.transform.position, currentShipInstance.transform.rotation);

                // ShipShooting newShipShooting = currentShipInstance.GetComponent<ShipShooting>();
                // ShipMovement newShipMovement = currentShipInstance.GetComponent<ShipMovement>();

                // newShipShooting.damage = oldShipShooting.damage;
                // newShipShooting.shootCooldown = oldShipShooting.shootCooldown;
                // newShipMovement.speed = oldShipMovement.speed;

                shipLevel++;
            }

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

        switch (currentShopType)
        {
            case ShopType.FirstShop:
                upgradeShip.interactable = false;
                break;
            case ShopType.SecondShop:
                upgradeShip.interactable = CanAffordUpgrade(shipCost) && shipLevel == 1;
                break;
            case ShopType.ThirdShop:
                upgradeShip.interactable = CanAffordUpgrade(shipCost) && shipLevel == 2;
                break;
        }
    }

    public void UpdateShopTexts()
    {
        // Fire Rate Text
        if (fireRateLevel < 5)
        {
            float currentFireRateValue = player.shootCooldown;
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
            float currentDamageValue = player.damage;
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
            float currentFireRateValue = player.shootCooldown;
            float nextFireRateValue = currentFireRateValue - 0.25f;
            fireRateUpgradeText.text = $"{currentFireRateValue}s > {nextFireRateValue}s";
        }
        else
        {
            fireRateUpgradeText.text = "Max Level";
        }
    }

    public void SetCurrentShopType(ShopType type)
    {
        currentShopType = type;
        UpdateButtonInteractivity();
    }
}
