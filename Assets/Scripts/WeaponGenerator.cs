using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour {


    public GameObject prefab1, prefab2;
    public GameObject prevObject;

    int probability;
	void Start ()
    {
        prevObject = null;

        List<float> num = FeatureGenerator.GenerateNumbersFromString("rugard");
        probability = (int)FeatureGenerator.remap(num[9], 0.0f, 1.0f, 0.0f, 100.0f);
        CreateDivision((int)FeatureGenerator.remap(num[9], 0.0f, 1.0f, 5.0f, 14.0f), 0);
    }

    public int CreateDivision(int iterations, float offset)
    {
        if (iterations == 0) return 0;
        GameObject newObj =  new GameObject();
        if (Random.Range(0, 100) < probability)
            newObj = GameObject.Instantiate(prefab1, this.gameObject.transform);
        else
            newObj = GameObject.Instantiate(prefab2, this.gameObject.transform);
        newObj.transform.position += new Vector3(offset, 0, 0);
        
        if(prevObject!=null)
        {
            if(newObj.transform.localScale.x > 0.3f) newObj.transform.localScale = newObj.transform.localScale + new Vector3(Random.Range(0.0f, 0.2f), 0.0f, 0.0f);
            newObj.GetComponent<HingeJoint>().connectedBody = prevObject.GetComponent<Rigidbody>();
        }
        prevObject = newObj;
        iterations--;
        CreateDivision(iterations--, offset + newObj.GetComponent<Collider>().bounds.size.x);
        return 1;
    }
}
