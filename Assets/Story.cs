using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour {
    private Material mat;

    private void Awake() {
        mat = GetComponent<Renderer>().material;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTint (Color c) {
        mat.SetColor("_Color", c);
    }

    private void OnCollisionEnter(Collision collision) {
        transform.parent.gameObject.GetComponent<Building3D>().Collision(collision);
    }
}
