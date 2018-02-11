using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    AudioSource aS;
    public float spawnTime;
	void Start ()
    {
        InvokeRepeating("SpawnEnemy", 1, spawnTime);
        aS = GameObject.FindGameObjectWithTag("Player").AddComponent<AudioSource>();

        InvokeRepeating("GruntSounds", 5, 10);
    }


    void SpawnEnemy()
    {
        
        GameObject.Instantiate(enemyPrefab).transform.position = this.transform.position;
        
        enemyPrefab.GetComponentInChildren<PeopleController>().speed = Random.Range(0.1f, 0.5f);
    }

    void GruntSounds()
    {
        int rand = Random.Range(0, 5);
        if (rand == 0) aS.clip = SoundManager.Instance.GetSound("Grunt1");
        if (rand == 1) aS.clip = SoundManager.Instance.GetSound("Grunt2");
        if (rand == 2) aS.clip = SoundManager.Instance.GetSound("Grunt3");
        if (rand == 3) aS.clip = SoundManager.Instance.GetSound("Rage1");
        if (rand == 4) aS.clip = SoundManager.Instance.GetSound("Rage2");
        if (rand == 5) aS.clip = SoundManager.Instance.GetSound("Rage3");
        List<float> generatedNum = FeatureGenerator.GenerateNumbersFromString("asdasda");
        aS.pitch = FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.2f, 0.9f);
        aS.volume = 0.9f;
       
        aS.Play();

    }
}
