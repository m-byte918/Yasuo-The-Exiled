using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthSlider : MonoBehaviour {
    public Slider healthSlider;

    void Start() {
        healthSlider.value = 0;
    }

    void LateUpdate() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }

    public void takeDamage(float value) {
        // Reduce health
        //Debug.Log(healthSlider.value);
        //healthSlider.value -= value;

        if (false){//healthSlider.value <= 0f) {
            // Game over
            SceneManager.LoadScene("Title Screen");
        }
    }
}