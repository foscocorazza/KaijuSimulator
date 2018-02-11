using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaijuCameraController : MonoBehaviour {

    public float shakeTime = 0.3f;
    public float shakeDT = 0.02f;
    public float magnitude = 0.4f;

    private float originalShakeTime;
    private float originalShakeDT;
    private float originalMagnitude;

    private bool isShaking = false;
    private Vector3 originalPos;
    
	void Start () {
        originalPos = transform.localPosition;
        originalShakeTime = shakeTime;
        originalShakeDT = shakeDT;
        originalMagnitude = magnitude;
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            initShake();
        }
	}

    public void initShake() {
        if (isShaking) {
            return;
        }
        isShaking = true;
        StartCoroutine(doShake());
    }

    private IEnumerator doShake() {
        float auxTime = 0f;
        while (auxTime < shakeTime) {
            Vector2 shakeVal = Random.insideUnitCircle * magnitude;
            transform.localPosition = originalPos + new Vector3(shakeVal.x, shakeVal.y, 0);
            auxTime += shakeDT;
            yield return new WaitForSeconds(shakeDT);
        }
        transform.localPosition = originalPos;
        isShaking = false;
        //Setting original values
        shakeTime = originalShakeTime;
        shakeDT = originalShakeDT;
        magnitude = originalMagnitude;
    }
}
