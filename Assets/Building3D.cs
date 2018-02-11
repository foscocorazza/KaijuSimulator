using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building3D : MonoBehaviour {
    public Story storyPrefab;
    public int startStory = 4;

    [Range(1, 8)]
    public int storiesCount = 4;
    public Color tint;
    public Texture2D texture;

    private ParticleSystem _emitter;
    private ParticleSystem _burster;

    private List<Story> _stories;

    public float destructionDelay = 2.0f;
    private float _destructionTimer = 0;

    private Color grayscale;

    private void Awake() {
        _emitter = GetComponentsInChildren<ParticleSystem>()[0];
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
        s.SetTexture(texture);
        _stories.Add(s);


        _emitter.transform.localPosition = s.transform.localPosition;
        //_emitter.transform.position = s.transform.position; 
    }

    private void DestroyStory () {
        Story s = _stories[_stories.Count - 1];

        _stories.Remove(s);
        Destroy(s.gameObject);
        
        Vector3 pos = _emitter.transform.localPosition;
        pos.y--;

        _emitter.transform.localPosition = pos;
        SoundManager.Instance.AddScore(20);
        ExplosionBurst();
    }

    private void ExplosionBurst () {
        
    }
    
    public void SetTint (Color c) {
        tint = c;

        if (_stories != null)
            foreach (var i in _stories)
                i.SetTint(c);
    }

    public void SetTexture(Texture2D t) {
        texture = t;

        if (_stories != null)
            foreach (var i in _stories)
                i.SetTexture(t);
    }

    public void Collision (Collision collision) {
        if (storiesCount > 0 && _destructionTimer <= 0) {
            storiesCount--;
            _destructionTimer = destructionDelay;
        }
    }

}
