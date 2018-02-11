using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour {
    public Building3D buildingPrefab;
    public List<Texture2D> textures;

	// Use this for initialization
	void Start () {
        //TMP lot of magic numbers but whatever
        for (int i = 0; i < 40; i++) {
            SpawnBuilding();
        }

        SpawnBuilding(new Vector3(-30, -4.2f, 0));
	}

    void SpawnBuilding()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(-25, 45);
        pos.y = -4.2f + Random.Range(-0.1f, 0.1f);
        pos.z = 0;

        SpawnBuilding(pos);
    }
    void SpawnBuilding(Vector3 pos)
    {
        

        int st = Random.Range(4, 8);
        Color tint = Random.ColorHSV();
        Texture2D tex = textures[Random.Range(0, textures.Count - 1)];
        for (int j = 0; j < 3; j++)
        {
            Building3D building = Instantiate(buildingPrefab);
            building.transform.position = pos;

            building.startStory = st;
            building.storiesCount = building.startStory;
            building.SetTint(tint);
            building.SetTexture(tex);
            pos.x++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
