using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Abilities : MonoBehaviour {
    /**
     * Description:
     *  Q:
     *      - Can use anywhere
     *      - First two are a stab
     *          - Deals damage if enemy touches arrow(?)
     *      - Third is a whirlwind
     *          - Shoots projectile that deals damage to the enemy
     *          - When hits, knocks up all enemies in larger-than-whirlwind spherical range
     *  E:
     *      - Can use anywhere
     *      - Dash attack: Dash forward, deal damage to all enemies in cone-shaped range
     *  R:
     *      - Spin attack
     *      - Knock up enemies
     *
     *  Arrow indicators modes:
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

    private float rotateVelocity;
    private float nextAutoAttackTime = 0f;
    public float autoAttackDuration = 1f; // seconds
    public float autoAttackDamage = 10f;

    private Animator anim;

    //AudioSouce for abilities
    private AudioSource audioSourceSound;
    private AudioSource audioSourceVoice;

    [Header("Q Ability")]
    public float qRange = .002f;
    public float qLastRange = .004f;
    public float qCooldown = 2f;
    public float qStackCooldown = 4f;
    public Image qAbilityImg;
    public AudioClip qOneAbilitySound, qThreeAbilitySound;
    public AudioClip qOneAbilityVoice, qThreeAbilityVoice;
    

    private int qStackCounter = 1;
    private float lastQTime = 0f;

    [Header("E Ability")]
    public float eRangeX = .002f;
    public float eRangeZ = 1f;
    public float eCooldown = 0.5f;
    public Image eAbilityImg;
    public AudioClip eAbilitySound;
    public AudioClip eAbilityVoice;

    [Header("R Ability")]
    public float rRangeX = .004f;
    public float rRangeZ = 2f;
    public float rCooldown = 30f;
    public Image rAbilityImg;
    public AudioClip rAbilitySound;
    public AudioClip rAbilityVoice;

    void Start() {
        qAbilityImg.fillAmount = 0f;
        eAbilityImg.fillAmount = 0f;
        rAbilityImg.fillAmount = 0f;

        arrowIndicatorCanvas.enabled = false;
        circleIndicatorCanvas.enabled = false;

        audioSourceSound = gameObject.GetComponent<AudioSource>();
        audioSourceVoice = gameObject.GetComponent<AudioSource>();

        anim = GetComponent<Animator>();

        //audioSourceSound.volume = 0.5f;
        //audioSourceVoice.volume = 0.5f;
    }

    // Update all abilities
    void Update() {
        qAbility();
        eAbility();
        rAbility();

        GameObject[] enemiesUnfiltered = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemies = new List<GameObject>();

        // Filter out enemies with unactive or disabled nav mesh agents
        foreach (GameObject enemy in enemiesUnfiltered) {
            NavMeshAgent n = enemy.GetComponent<NavMeshAgent>();
            if (n.isActiveAndEnabled)
                enemies.Add(enemy);
        }

        if (enemies.Count > 0) {
            // Automatically face the nearest enemy
            enemies.Sort((e1, e2) =>
                    (int)e1.GetComponent<NavMeshAgent>().remainingDistance - (int)e2.GetComponent<NavMeshAgent>().remainingDistance);

            if (enemies[0].GetComponent<NavMeshAgent>().remainingDistance > 3)
                return;

            if (GetComponent<NavMeshAgent>().remainingDistance < 0.5) {
                Quaternion targetRotation = Quaternion.LookRotation(enemies[0].transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y, targetRotation.eulerAngles.y, ref rotateVelocity, 1 * (Time.deltaTime * 10)
                );
                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
        }

        if (Time.time >= nextAutoAttackTime) {
            stab(autoAttackDamage);
            nextAutoAttackTime = Time.time + autoAttackDuration;
        }
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

    void stab(float damage) {
        /**
         * Play stab animation here
        **/
        // Stab attack
        Transform b = arrowIndicatorCanvas.transform.GetChild(1);
        RaycastHit[] hits = Physics.BoxCastAll(b.position, b.lossyScale / 2f, transform.forward, b.rotation);
        foreach (RaycastHit h in hits) {
            if (h.transform.CompareTag("Enemy"))
                h.transform.GetComponent<Enemy>().takeDamage(damage);
        }
    }
    
    void qAttack() {
        if (qStackCounter < 3) {
            stab(25);
            // <play jab sound>
            audioSourceSound.PlayOneShot(qOneAbilitySound);
            audioSourceVoice.PlayOneShot(qOneAbilityVoice);
            StartCoroutine(qAbilityAnimation());
           
            
        } else {
            // Whirlwind attack
            audioSourceVoice.PlayOneShot(qThreeAbilityVoice);
            StartCoroutine(qAbilityAnimation());
            GetComponent<Whirlwind>().launch();
        }
    }

    void circleAttack() {
        Collider[] collisions = Physics.OverlapSphere(transform.position, 8f * circleIndicatorCanvas.transform.localScale.z);

        foreach (Collider c in collisions) {
            if (c.CompareTag("Enemy"))
                c.GetComponent<Enemy>().takeDamage(20 * circleIndicatorCanvas.transform.localScale.z);
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

            
            // Play attack audio
            if (code == KeyCode.E) {
                
                audioSourceVoice.PlayOneShot(eAbilityVoice);
                audioSourceSound.PlayOneShot(eAbilitySound);
                // set animation state
                StartCoroutine(eAbilityAnimation());
            } else {
            
                audioSourceVoice.PlayOneShot(rAbilityVoice);
                audioSourceSound.PlayOneShot(rAbilitySound);
                // set animation state
                StartCoroutine(rAbilityAnimation());
            }
            circleAttack();
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

    IEnumerator qAbilityAnimation()
    {
        anim.SetBool("QAttack", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("QAttack", false);
    }

    IEnumerator eAbilityAnimation()
    {
        anim.SetBool("EAttack", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("EAttack", false);
    }
    IEnumerator rAbilityAnimation()
    {
        anim.SetBool("RAttack", true);
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("RAttack", false);
    }
}
