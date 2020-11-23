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

    [Header("Q Ability")]
    public float qRange = .002f;
    public float qLastRange = .004f;
    public float qCooldown = 2f;
    public float qStackCooldown = 4f;
    public Image qAbilityImg;

    private int qStackCounter = 1;
    private float lastQTime = 0f;

    [Header("E Ability")]
    public float eRangeX = .002f;
    public float eRangeZ = 1f;
    public float eCooldown = 0.5f;
    public Image eAbilityImg;

    [Header("R Ability")]
    public float rRangeX = .004f;
    public float rRangeZ = 2f;
    public float rCooldown = 30f;
    public Image rAbilityImg;

    void Start() {
        qAbilityImg.fillAmount = 0f;
        eAbilityImg.fillAmount = 0f;
        rAbilityImg.fillAmount = 0f;

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
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        if (qStackCounter == 3) {
            // Whirlwind --> double the range
            arrowIndicatorCanvas.transform.localScale = new Vector3(qLastRange, .002f, 1f);
        } else {
            // Normal
            arrowIndicatorCanvas.transform.localScale = new Vector3(qRange, .002f, 1f);
        }
        arrowIndicatorCanvas.transform.rotation = Quaternion.Euler(0f, 100f - angle, 0f);
    }

    void qAttack() {
        Transform b = arrowIndicatorCanvas.transform.GetChild(1);
        RaycastHit[] hits = Physics.BoxCastAll(b.localPosition, b.localScale / 2f, transform.forward, b.rotation);

        foreach (RaycastHit h in hits) {
            if (h.transform.name != "Enemy")
                continue;
            Debug.Log(Time.time);
        }
        if (qStackCounter < 3) {
            //Debug.Log("Stab");
        } else {
            //Debug.Log("Whirlwind");
        }
    }

    void qAbility() {
        lastQTime += Time.deltaTime;
        if (fillImage(qAbilityImg, qCooldown)) {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Q)) {
            // Q is no longer being pressed, start the cooldown.
            qAbilityImg.fillAmount = 1f;
            arrowIndicatorCanvas.enabled = false;
            qAttack();

            if (lastQTime % 60f < qStackCooldown) {
                // Only stack if last Q time was less than 6 seconds
                if (++qStackCounter > 3)
                    qStackCounter = 1; // Max 3 stacks
            } else {
                // Otherwise, reset
                qStackCounter = 1;
            }
            lastQTime = 0;
        } else if (Input.GetKey(KeyCode.Q)) {
            // Q is being held down, move the arrow indicator but do not start the cooldown
            arrowIndicatorCanvas.enabled = true;
            moveArrowIndicator();
        }
    }

    void eAbility() {
        if (!fillImage(eAbilityImg, eCooldown))
            updateCircleAbility(KeyCode.E, eAbilityImg, eRangeX, eRangeZ);
    }

    void rAbility() {
        if (!fillImage(rAbilityImg, rCooldown))
            updateCircleAbility(KeyCode.R, rAbilityImg, rRangeX, rRangeZ);
    }

    void updateCircleAbility(KeyCode code, Image abilityImg, float rx, float rz) {
        if (Input.GetKeyUp(code)) {
            // Key is no longer being pressed, start the cooldown.
            abilityImg.fillAmount = 1f;
            circleIndicatorCanvas.enabled = false;
        } else if (Input.GetKey(code)) {
            // Key is being held down
            circleIndicatorCanvas.transform.localScale = new Vector3(rx, .002f, rz);
            circleIndicatorCanvas.enabled = true;
            arrowIndicatorCanvas.enabled = false;
        }
    }

    bool fillImage(Image abilityImg, float cooldown) {
        if (abilityImg.fillAmount == 0f) {
            return false;
        }
        abilityImg.fillAmount -= Time.deltaTime / cooldown;
        if (abilityImg.fillAmount <= 0f) {
            abilityImg.fillAmount = 0f; // Cooldown finished
        }
        return true;
    }
}
