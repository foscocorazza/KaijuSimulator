using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorStomp : MonoBehaviour {

    private PlayerController myPlayer;
    private KaijuCameraController myCam;

    void Start() {
        myPlayer = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PlayerController>();
        myCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KaijuCameraController>();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Player")) {
            if (myPlayer.isJumping) {
                myPlayer.isJumping = false;
                myCam.magnitude = 0.5f;
                myCam.initShake();
            } else {
                myCam.magnitude = 0.1f;
                myCam.initShake();
            }
        }
    }
}
