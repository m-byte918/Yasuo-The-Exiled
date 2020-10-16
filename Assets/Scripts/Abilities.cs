using System;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour {
    /**
     * Arrow indicators modes:
     *     mouse lock (arrow rotates around player with mouse):
     *         - when player holds down ability key
     *             - indicator dissapears when key is released or player right clicks
     *         - when player left clicks on ability image
     *             - indicator dissapears when player left clicks again or right clicks
     *     character lock (arrow points to direction player is facing):
     *         - when mouse hovers over ability image
     *             - indicator dissapears when mouse leaves ability image
     */

    // Indicator images
    public Canvas arrowIndicatorCanvas;
    public Canvas circleIndicatorCanvas;
    public Transform player;

    [Header("Q Ability")]
    public float qRange;
    public float qMaxRange;
    public float qCooldown;
    public Image qAbilityImg;

    [Header("E Ability")]
    public float eRange;
    public float eCooldown;
    public Image eAbilityImg;

    [Header("R Ability")]
    public float rRange;
    public float rCooldown;
    public Image rAbilityImg;

    void Start() {
        qAbilityImg.fillAmount = 0;
        eAbilityImg.fillAmount = 0;
        rAbilityImg.fillAmount = 0;

        arrowIndicatorCanvas.enabled = false;
        circleIndicatorCanvas.enabled = false;
    }

    // Update all abilities
    void Update() {
        qAbility();
        eAbility();
        rAbility();
    }

    void moveArrowIndicator() {
        circleIndicatorCanvas.enabled = false;

        // Rotate indicator along y axis relative to mouse and player position
        Vector3 pos = Camera.main.WorldToScreenPoint(player.transform.position);
        pos = Input.mousePosition - pos;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        arrowIndicatorCanvas.transform.rotation = Quaternion.Euler(0f, 100f - angle, 0f);
    }

    void qAbility() {
        if (qAbilityImg.fillAmount == 0) {
            if (Input.GetKeyUp(KeyCode.Q)) {
                // Q is no longer being pressed, start the cooldown.
                qAbilityImg.fillAmount = 1;
                arrowIndicatorCanvas.enabled = false;
            } else if (Input.GetKey(KeyCode.Q)) {
                // Q is being held down, move the arrow indicator but do not start the cooldown
                arrowIndicatorCanvas.enabled = true;
                moveArrowIndicator();
            }
        } else {
            qAbilityImg.fillAmount -= 1 / qCooldown * Time.deltaTime;
            if (qAbilityImg.fillAmount <= 0)
                qAbilityImg.fillAmount = 0; // Cooldown finished
        }
    }

    void eAbility() {
        if (eAbilityImg.fillAmount == 0) {
            if (Input.GetKeyUp(KeyCode.E)) {
                // E is no longer being pressed, start the cooldown.
                eAbilityImg.fillAmount = 1;
                circleIndicatorCanvas.enabled = false;
            } else if (Input.GetKey(KeyCode.E)) {
                // E is being held down
                circleIndicatorCanvas.transform.localScale = new Vector3(0.002f, 0.002f, 1f);
                circleIndicatorCanvas.enabled = true;
                arrowIndicatorCanvas.enabled = false;
            }
        } else {
            eAbilityImg.fillAmount -= 1 / eCooldown * Time.deltaTime;
            if (eAbilityImg.fillAmount <= 0)
                eAbilityImg.fillAmount = 0; // Cooldown finished
        }
    }

    void rAbility() {
        if (rAbilityImg.fillAmount == 0) {
            if (Input.GetKeyUp(KeyCode.R)) {
                // R is no longer being pressed, start the cooldown.
                rAbilityImg.fillAmount = 1;
                circleIndicatorCanvas.enabled = false;
            } else if (Input.GetKey(KeyCode.R)) {
                // R is being held down
                circleIndicatorCanvas.transform.localScale = new Vector3(0.004f, 0.002f, 2f);
                circleIndicatorCanvas.enabled = true;
                arrowIndicatorCanvas.enabled = false;
            }
        } else {
            rAbilityImg.fillAmount -= 1 / rCooldown * Time.deltaTime;
            if (rAbilityImg.fillAmount <= 0)
                rAbilityImg.fillAmount = 0; // Cooldown finished
        }
    }
}
