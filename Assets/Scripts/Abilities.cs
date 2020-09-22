using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour {
    [Header("Q Ability")]
    public Image qAbilityImage;
    public float qCooldown = 2f;

    /*[Header("W Ability")]
    public Image wAbilityImage;
    public float wCooldown = 0f;*/

    [Header("E Ability")]
    public Image eAbilityImage;
    public float eCooldown = 0.5f;

    [Header("R Ability")]
    public Image rAbilityImage;
    public float rCooldown = 30f;

    void Start() {
        qAbilityImage.fillAmount = 0;
        //wAbilityImage.fillAmount = 0;
        eAbilityImage.fillAmount = 0;
        rAbilityImage.fillAmount = 0;
    }

    // Update all abilities
    void Update() {
        qAbility();
        //wAbility();
        eAbility();
        rAbility();
    }

    void qAbility() {
        if (Input.GetKey(KeyCode.Q) && qAbilityImage.fillAmount == 0) {
            qAbilityImage.fillAmount = 1; // Cooldown started
            return;
        }
        if (qAbilityImage.fillAmount > 0) {
            qAbilityImage.fillAmount -= 1 / qCooldown * Time.deltaTime;

            if (qAbilityImage.fillAmount <= 0)
                qAbilityImage.fillAmount = 0; // Cooldown finished
        }
    }

    /*void wAbility() {
        if (Input.GetKey(KeyCode.W) && wAbilityImage.fillAmount == 0) {
            wAbilityImage.fillAmount = 1; // Cooldown started
            return;
        }
        if (wAbilityImage.fillAmount > 0) {
            wAbilityImage.fillAmount -= 1 / wCooldown * Time.deltaTime;

            if (wAbilityImage.fillAmount <= 0)
                wAbilityImage.fillAmount = 0; // Cooldown finished
        }
    }*/

    void eAbility() {
        if (Input.GetKey(KeyCode.E) && eAbilityImage.fillAmount == 0) {
            eAbilityImage.fillAmount = 1; // Cooldown started
            return;
        }
        if (eAbilityImage.fillAmount > 0) {
            eAbilityImage.fillAmount -= 1 / eCooldown * Time.deltaTime;

            if (eAbilityImage.fillAmount <= 0)
                eAbilityImage.fillAmount = 0; // Cooldown finished
        }
    }

    void rAbility() {
        if (Input.GetKey(KeyCode.R) && rAbilityImage.fillAmount == 0) {
            rAbilityImage.fillAmount = 1; // Cooldown started
            return;
        }
        if (rAbilityImage.fillAmount > 0) {
            rAbilityImage.fillAmount -= 1 / rCooldown * Time.deltaTime;

            if (rAbilityImage.fillAmount <= 0)
                rAbilityImage.fillAmount = 0; // Cooldown finished
        }
    }
}
