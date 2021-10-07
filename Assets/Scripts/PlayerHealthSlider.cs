using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthSlider : MonoBehaviour {
    void LateUpdate() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }

    public void takeDamage(float value) {
        // Reduce health
        Slider health = transform.GetChild(0).GetComponent<Slider>();
        health.value -= value;

        if (health.value <= 0f) {
            // Game over
            SceneManager.LoadScene("Title Screen");
        }
    }
}