using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour {
    public Destructible buildingPrefab;

	// Use this for initialization
	void Start () {
        //TMP lot of magic numbers but whatever
        for (int i = 0; i < 10; i++) {
            Destructible building = Instantiate<Destructible>(buildingPrefab);
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-20, 35);
            pos.y = -5 + building.GetHeight() / 2 + Random.Range(-0.2f, 0.2f);
            pos.z = 0;
            building.transform.position = pos;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
