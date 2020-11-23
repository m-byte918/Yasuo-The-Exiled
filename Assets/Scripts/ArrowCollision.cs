using UnityEngine;
using UnityEngine.UI;

public class ArrowCollision : MonoBehaviour {
    public Image qAbilityImg;

    void OnTriggerEnter(Collider other) {
        if (other.name != "Enemy")
            return;
        //Debug.Log(Time.time);
    }
}
