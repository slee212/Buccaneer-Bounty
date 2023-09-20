using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject shopUI;
    public PauseMenu pause;
    private bool isPlayerInRange = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // AudioListener.pause = false; // Resume all sounds
            pause.TogglePause();
            ToggleShopUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In range");
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // shopPromptUI.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            isPlayerInRange = false;
            // shopPromptUI.SetActive(false);
            shopUI.SetActive(false);
        }
    }

    private void ToggleShopUI() 
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
}
