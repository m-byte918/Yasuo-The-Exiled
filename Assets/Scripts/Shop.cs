using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
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
        // Subtract price from the wallet and add item to inventory
        Debug.Log("The price is: " + price);
    }
}
