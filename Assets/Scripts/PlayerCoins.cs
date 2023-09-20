using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    public static int coins = 10;

    public TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinUI();
    }
    public void AddCoin()
    {
            Debug.Log("AddCoin method called!");  // Debug lo
        coins += 1;
        Debug.Log("Coin collected! Current coins: " + coins);
        UpdateCoinUI();
    }
    // Update is called once per frame
    public void SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCoinUI();
        } else
        {
            Debug.Log("Not enough coins");
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText){
            coinText.text = coins.ToString();
        }
    }
}
