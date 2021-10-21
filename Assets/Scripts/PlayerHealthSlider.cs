using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour {
    void LateUpdate() {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}