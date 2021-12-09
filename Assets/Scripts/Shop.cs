using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    GameObject player = null;
    public Button swordSlot = null;
    public Button shieldSlot = null;
    public Button potionSlot = null;
    private bool swordPurchased = false;
    private bool shieldPurchased = false;
    
    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        Transform itemList = GameObject.Find("ItemList").transform;

        int btnCount = itemList.childCount;

        for (int i = 0; i < btnCount; ++i) {
            // ItemList -> Item
            Transform child = itemList.GetChild(i);
            if (child.childCount == 0) {
                continue;
            }
            // Item -> Button
            Transform btnTransform = child.Find("Button");

            // Button -> Text
            string btnText = btnTransform.GetChild(0).GetComponent<Text>().text;
            string price = btnText.Substring(1); // Remove dollar sign
            int priceParsed = int.Parse(price); // Convert price string to integer
            
            btnTransform.GetComponent<Button>().onClick.AddListener(delegate { ButtonHandler(priceParsed); });
        }
    }

    void onPotionPurchase(int price) {
        if (!subtractPrice(price)) {
            return;
        }
        // Add to inventory
        // Restore health (+50)
        Debug.Log("Potion purchased");
        potionSlot.transform.Find("RawImage").gameObject.SetActive(true);

        GameObject.Find("2D Health Slider").GetComponent<Health>().heal(50f);
    }

    void onSwordPurchase(int price) {
        if (swordPurchased || !subtractPrice(price)) {
            return;
        }
        swordPurchased = true;
        Debug.Log("Sword purchased");

        // Make button unclickable
        //GameObject.Find("Potion").transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();

        // Make visible in inventory
        swordSlot.transform.Find("RawImage").gameObject.SetActive(true);

        // Increase offense (+50 for all abilities)
        Abilities abilities = player.GetComponent<Abilities>();
        abilities.qDamage += 50f;
        abilities.qLastDamage += 50f;
        abilities.eDamage += 50f;
        abilities.rDamage += 50f;
        abilities.autoAttackDamage += 5f;
    }

    void onShieldPurchase(int price) {
        if (shieldPurchased || !subtractPrice(price)) {
            return;
        }
        shieldPurchased = true;
        Debug.Log("Shield purchased");

        // Make button unclickable
        //GameObject.Find("Shield").transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();

        // Make visible in inventory
        shieldSlot.transform.Find("RawImage").gameObject.SetActive(true);

        // Increase defense (-30% enemy damage)
        GameObject.Find("2D Health Slider").GetComponent<Health>().damageReductionMult = 0.3f;
    }

    bool subtractPrice(int price) {
        PlayerStats stats = player.GetComponent<PlayerStats>();

        if (stats.getCoinCount() < price) {
            return false; // Broke lmao
        }
        // Subtract price from the wallet
        stats.setCoinCount(stats.getCoinCount() - price);
        return true;
    }

    void ButtonHandler(int price) {
        // Custom item actions
        // Hardcoding these for now
        switch (price) {
            case 50: onPotionPurchase(price); break;
            case 100: onSwordPurchase(price); break;
            case 150: onShieldPurchase(price); break;
        }
    }
}
