using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    [Range(0, 4)]
    public int Level = 4;
    private ParticleSystem emitter;
    private Renderer renderer;
    Color grayscale;

    private void Awake() {
        emitter = GetComponentInChildren<ParticleSystem>();
        renderer = GetComponent<Renderer>();
    }
    // Use this for initialization
    void Start () {
         grayscale = new Color (1.0f / 0x3E, 1.0f / 0x3E, 1.0f / 0x3E);
    }
	
	// Update is called once per frame
	void Update () {
        SetLevel();
	}

    private void SetLevel () {
        float _level = (float)Level / 4;
        renderer.material.SetFloat("_Cutoff", _level);
        if (_level == 1)
            emitter.Stop();
        else if (emitter.isStopped)
            emitter.Play();

        var colOverLifetime = emitter.colorOverLifetime;
        Color startColor = _level == 0 ? grayscale : Color.red;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(startColor, 0.25f), new GradientColorKey(grayscale, 0.37f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

        colOverLifetime.color = grad;

        Vector3 newPos = renderer.bounds.center;

        newPos.z += 0.5f;
        //tweak pos
        if (_level == 0)
            newPos.y += 0.5f;
        newPos.y += (renderer.bounds.size.y / 4) * (_level * 4) - renderer.bounds.size.y / 2;


        emitter.transform.position = newPos;
    }
}
