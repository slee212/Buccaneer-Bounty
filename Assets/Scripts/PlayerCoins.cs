using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    public static int coins = 0;
    public TextMeshProUGUI coinText;

    public AudioSource audioSource;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        // Attempt to automatically find the coinText object by tag
        GameObject coinTextObj = GameObject.FindWithTag("coinCounter");
        if (coinTextObj != null)
        {
            coinText = coinTextObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Object with tag 'CoinText' not found.");
        }

        UpdateCoinUI();
    }

    public void AddCoin()
    {
        audioSource.PlayOneShot(deathSound);
        coins += 2;
        UpdateCoinUI();
    }

    public void SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCoinUI();
        } 
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText)
        {
            coinText.text = coins.ToString();
        }
        else
        {
            Debug.LogError("coinText is null. Please assign the TextMeshProUGUI component in the Inspector, or make sure the object with tag 'CoinText' has a TextMeshProUGUI component.");
        }
    }

    public static void ResetCoins()
    {
        coins = 0;
    }
}
