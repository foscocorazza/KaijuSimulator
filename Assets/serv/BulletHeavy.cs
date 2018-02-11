using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHeavy : MonoBehaviour {

    public float force = 30;
    public float lifeTime = 0.9f;

    private Rigidbody myBody;

	void Start () {
        myBody = GetComponent<Rigidbody>();
        myBody.AddForce(transform.right * force, ForceMode.Impulse);
        StartCoroutine(ggBullet());
    }

    private IEnumerator ggBullet() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
