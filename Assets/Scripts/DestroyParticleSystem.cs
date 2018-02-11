using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour {

    ParticleSystem ps;
    AudioSource aS;
    void Start ()
    {
        ps = this.GetComponent<ParticleSystem>();
        aS = this.GetComponent<AudioSource>();
	}
	
	void Update ()
    {
        if (ps!=null && !ps.isPlaying)
        {
            Destroy(this.gameObject);
        }
        if (aS != null && !aS.isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
