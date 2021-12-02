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

    void ButtonHandler(int price) {
        if (stats.getCoinCount() < price) {
            return; // Broke lmao
        }
        // Subtract price from the wallet and add item to inventory
        stats.setCoinCount(stats.getCoinCount() - price);
    }
}
