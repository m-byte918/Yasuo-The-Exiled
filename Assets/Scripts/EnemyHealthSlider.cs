using UnityEngine;

public class EnemyHealthSlider : MonoBehaviour {
    public Canvas playerHealthCanvas;

    void LateUpdate() {
        transform.rotation = playerHealthCanvas.transform.rotation;
    }
}
