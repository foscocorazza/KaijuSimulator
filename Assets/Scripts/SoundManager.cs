﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;
    public static int score = 0;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    [System.Serializable]
    public class sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<sound> sounds;

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
    public int getScore()
    {
        return score;
    }

    public AudioClip GetSound(string name)
    {
        foreach (sound s in sounds)
        {
            if (s.name == name)
            {
                return s.clip;
            }
        }
        return null;
    }
}
