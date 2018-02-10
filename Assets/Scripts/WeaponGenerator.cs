using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour {


    public GameObject whipBase, whipSomething, swordBase;
    private GameObject prevObject;

    int probability;
	void Start ()
    {
        prevObject = null;

        List<float> num = FeatureGenerator.GenerateNumbersFromString("rugard");
        probability = (int)FeatureGenerator.remap(num[9], 0.0f, 1.0f, 0.0f, 100.0f);
        //CreateWhip((int)FeatureGenerator.remap(num[9], 0.0f, 1.0f, 5.0f, 14.0f), 0);
        CreateSword(0.1f, 3.0f, 1.0f);
        //CreateStick(0.1f, 1.5f, 1.0f);
    }

    public int CreateWhip(int iterations, float offset)
    {
        if (iterations == 0) return 0;
        GameObject newObj =  new GameObject();
        if (Random.Range(0, 100) < probability)
            newObj = GameObject.Instantiate(whipBase, this.gameObject.transform);
        else
            newObj = GameObject.Instantiate(whipSomething, this.gameObject.transform);
        newObj.transform.position += new Vector3(offset, 0, 0);
        
        if(prevObject!=null)
        {
            if(newObj.transform.localScale.x > 0.3f) newObj.transform.localScale = newObj.transform.localScale + new Vector3(Random.Range(0.0f, 0.2f), 0.0f, 0.0f);
            newObj.GetComponent<HingeJoint>().connectedBody = prevObject.GetComponent<Rigidbody>();
        }
        prevObject = newObj;
        iterations--;
        CreateWhip(iterations--, offset + newObj.GetComponent<Collider>().bounds.size.x);
        return 1;
    }
    public void CreateSword(float width, float height, float mass)
    {
        GameObject sB = GameObject.Instantiate(swordBase);
        Transform blade = sB.transform.GetChild(2);
        blade.localScale = new Vector3(width, height, 0.05f);
        blade.localPosition += new Vector3(0, blade.gameObject.GetComponent<Collider>().bounds.size.y/2 - 0.5f, 0); 
        sB.GetComponent<Rigidbody>().mass = mass;
    }
    public void CreateStick(float width, float height, float mass)
    {
        GameObject sB = GameObject.Instantiate(swordBase);
        Transform top = sB.transform.GetChild(1);
        Transform bottom = sB.transform.GetChild(2);
        top.localScale = new Vector3(width, height, 0.05f);
        top.localPosition += new Vector3(0, top.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);

        bottom.localScale = new Vector3(width, height, 0.05f);
        bottom.localPosition -= new Vector3(0, bottom.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);
        sB.GetComponent<Rigidbody>().mass = mass;
    }
}
