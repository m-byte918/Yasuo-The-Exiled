using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Slider playerSlider3D;
    public Slider playerSlider2D;

    public int health;

    void Start() {
        playerSlider2D = GetComponent<Slider>();
    }

    void Update() {
        playerSlider2D.value = health;
        playerSlider3D.value = health;
    }
}
