using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building3D : MonoBehaviour {
    public Story storyPrefab;
    public int startStory = 4;

    [Range(1, 7)]
    public int storiesCount = 4;
    public Color tint;

    private ParticleSystem _emitter;
    private List<Story> _stories;

    public float destructionDelay = 2.0f;
    private float _destructionTimer = 0;

    private Color grayscale;

    private void Awake() {
        _emitter = GetComponentInChildren<ParticleSystem>();
    }

    // Use this for initialization
    void Start () {
        storiesCount = startStory;
        _stories = new List<Story>();
        for (int i = 0; i < storiesCount; i++)
            CreateStory();

        grayscale = new Color(1.0f / 0x3E, 1.0f / 0x3E, 1.0f / 0x3E);
    }
	
	// Update is called once per frame
	void Update () {
        _destructionTimer -= Time.deltaTime;

        while (_stories.Count < storiesCount - 1)
            CreateStory();

        while (_stories.Count > storiesCount)
            DestroyStory();

        if (storiesCount == startStory)
            _emitter.Stop();
        else if (_emitter.isStopped)
            _emitter.Play();

        var colOverLifetime = _emitter.colorOverLifetime;
        Color startColor = storiesCount == 1 ? grayscale : Color.red;
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(startColor, 0.25f), new GradientColorKey(grayscale, 0.37f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });

    }

    private void CreateStory () {
        Story s = Instantiate(storyPrefab);
        s.transform.SetParent(transform, true);

        Vector3 pos = transform.position;
        pos.y += _stories.Count;

        s.transform.position = pos;

        s.SetTint(tint);

        _stories.Add(s);


        _emitter.transform.position = new Vector3(0, _stories.Count - 1, 0);
    }

    private void DestroyStory () {
        Story s = _stories[_stories.Count - 1];

        _stories.Remove(s);
        Destroy(s.gameObject);
        
        Vector3 pos = _emitter.transform.position;
        pos.y = storiesCount - 1;
        _emitter.transform.position = pos;
    }

    private void OnTriggerEnter(Collider other) {
        if (storiesCount > 0 && _destructionTimer <= 0) {
            storiesCount--;
            _destructionTimer = destructionDelay;
        }
    }

    public void SetTint (Color c) {
        tint = c;

        if (_stories != null)
            foreach (var i in _stories)
                i.SetTint(c);
    }

}
