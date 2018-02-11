using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHeavy : MonoBehaviour {

    public float force = 50;

    private Rigidbody myBody;

	void Start () {
        myBody = GetComponent<Rigidbody>();
        myBody.AddExplosionForce(force, Vector3.zero, 1);
    }
	
	void Update () {
		
	}
}
