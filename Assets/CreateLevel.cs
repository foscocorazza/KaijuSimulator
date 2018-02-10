using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour {
    public Building3D buildingPrefab;

	// Use this for initialization
	void Start () {
        //TMP lot of magic numbers but whatever
        for (int i = 0; i < 25; i++) {
            Building3D building = Instantiate(buildingPrefab);
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-20, 35);
            pos.y = -4.2f + Random.Range(-0.1f, 0.1f);
            pos.z = 0;
            building.transform.position = pos;

            building.storiesCount = Random.Range(3, 7);
            building.SetTint(Random.ColorHSV());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
