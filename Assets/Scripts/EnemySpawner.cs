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

        InvokeRepeating("GruntSounds", 10, 10);
    }


    void SpawnEnemy()
    {
        
        GameObject.Instantiate(enemyPrefab);
        enemyPrefab.GetComponent<PeopleController>().speed = Random.Range(0.1f, 0.5f);
    }

    void GruntSounds()
    {
        //int rand = Random.Range(0, 3);
        //if ( rand == 1)  
        //if (rand == 2) aS.clip = SoundManager.Instance.GetSound("Grunt2");
        //if (rand == 3) aS.clip = SoundManager.Instance.GetSound("Grunt3");
        aS.clip = SoundManager.Instance.GetSound("Grunt1");
        aS.volume = 0.5f;
        
        aS.Play();

    }
}
