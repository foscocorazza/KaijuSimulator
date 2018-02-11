using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour {

    public Transform targetGO = null;
    public Vector3 offset = Vector3.zero;
    public int mirrorY = 1;

    private Rigidbody myBody;

    void Start() {
        myBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (targetGO != null) {
            myBody.position = targetGO.position + offset;
            myBody.rotation = new Quaternion(targetGO.rotation.x,
                                            targetGO.rotation.y,
                                            targetGO.rotation.z * mirrorY,
                                            targetGO.rotation.w * 1);
        }
	}
}
