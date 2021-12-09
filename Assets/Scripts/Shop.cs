using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    PlayerStats stats = null;
    
    // Start is called before the first frame update
    void Start() {
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Transform itemList = GameObject.Find("ItemList").transform;

        int btnCount = itemList.childCount;

        for (int i = 0; i < btnCount; ++i) {
            // ItemList -> Item -> Button
            Transform btnTransform = itemList.GetChild(i).GetChild(0);

            // Button -> Text
            string btnText = btnTransform.GetChild(0).GetComponent<Text>().text;
            string price = btnText.Substring(1); // Remove dollar sign
            int priceParsed = int.Parse(price); // Convert price string to integer
            
            btnTransform.GetComponent<Button>().onClick.AddListener(delegate { ButtonHandler(priceParsed); });
        }
    }

    void onPotionPurchase() {
        // Add to inventory
        Debug.Log("Potion purchased");
    }

    void onSwordPurchase() {
        // Add to inventory
        Debug.Log("Sword purchased");
    }

    void onShieldPurchase() {
        // Add to inventory
        Debug.Log("Shield purchased");
    }

    void ButtonHandler(int price) {
        if (stats.getCoinCount() < price) {
            return; // Broke lmao
        }
        // Subtract price from the wallet
        stats.setCoinCount(stats.getCoinCount() - price);

        // Custom item actions
        // Hardcoding these for now
        switch (price) {
            case 50: onPotionPurchase(); break;
            case 100: onSwordPurchase(); break;
            case 150: onShieldPurchase(); break;
        }
    }
}
