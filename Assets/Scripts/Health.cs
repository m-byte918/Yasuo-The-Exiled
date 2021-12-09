using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {

    public Slider playerSlider3D;
    public Slider playerSlider2D;

    public float health = 250f;
    public float damageReductionMult = 0;

    void Start() {
        playerSlider2D = GetComponent<Slider>();
    }

    void Update() {
        playerSlider2D.value = health;
        playerSlider3D.value = health;
    }

    public void heal(float value) {
        health = Mathf.Min(playerSlider2D.maxValue, health + value);
    }

    public void takeDamage(float value) {
        // Reduce health
        health -= value - value * damageReductionMult;

        if (health <= 0f) {
            // Game over
            SceneManager.LoadScene("Gameover Screen");
        }
    }
}
