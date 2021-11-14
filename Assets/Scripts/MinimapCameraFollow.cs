using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour {
    public Transform player;

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        //transform.eulerAngles = new Vector3(90, 180, -90);
    }
}
