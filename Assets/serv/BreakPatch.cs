using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPatch : MonoBehaviour {

    public float maxSpeed = 10f;

    private Rigidbody myBody;

	void Start () {
        myBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        if (myBody.velocity.magnitude > maxSpeed) {
            myBody.velocity = myBody.velocity.normalized * maxSpeed;
        }
    }
}
