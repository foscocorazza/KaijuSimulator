using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;

    public float spawnTime;
	void Start ()
    {
        InvokeRepeating("SpawnEnemy", 1, spawnTime);
	}


    void SpawnEnemy()
    {
        
        GameObject.Instantiate(enemyPrefab);
        enemyPrefab.GetComponent<PeopleController>().speed = Random.Range(0.1f, 0.5f);
    }
}
