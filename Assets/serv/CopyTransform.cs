using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour {

    public Transform targetGO = null;
    public Vector3 offset = Vector3.zero;

    private Rigidbody myBody;

    void Start() {
        myBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (targetGO != null) {
            myBody.position = targetGO.position + offset;
            myBody.rotation = targetGO.rotation;
        }
	}
}
