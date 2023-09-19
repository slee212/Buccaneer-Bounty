using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    public int coins = 10;

    public TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
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
