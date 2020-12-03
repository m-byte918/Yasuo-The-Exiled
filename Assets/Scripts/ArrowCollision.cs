using UnityEngine;
using UnityEngine.UI;

public class ArrowCollision : MonoBehaviour {
    public Image qAbilityImg;

    private void OnTriggerExit(Collider other) {
        // Stop whirlwind movement if reached destination
        if (other.name == "Whirlwind")
            other.gameObject.SetActive(false);
    }
}
