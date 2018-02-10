using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour {
    public Vector3 direction;
    public float force;

    private Rigidbody myBody;

	void Start () {
        myBody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            myBody.AddForce(direction * force);
        }
	}
}
