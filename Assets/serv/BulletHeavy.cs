using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHeavy : MonoBehaviour {

    public float force = 30;

    private Rigidbody myBody;

	void Start () {
        myBody = GetComponent<Rigidbody>();
        myBody.AddForce(transform.right * force, ForceMode.Impulse);
    }
}
