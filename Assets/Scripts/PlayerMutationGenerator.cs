using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationGenerator : MonoBehaviour {

    public GameObject whipBase, whipSomething, swordBase;
    public Rigidbody p1Hand;
    public Rigidbody p2Hand;

    public GameObject p1Weapon;
    public GameObject p2Weapon;

    private int probability;
    private List<float> generatedNum;
    private string abc = "qwertyuioplkjhgfdsazxcvbnm";

    void Awake() {
        //kirzde
        generatedNum = FeatureGenerator.GenerateNumbersFromString(getRandomString(6));
        probability = (int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.0f, 100.0f);
        mutateBody();
        mutateWeapons();
    }

    private string getRandomString(int length) {
        string result = "";
        for (int i = 0; i < length; i++) {
            result += abc[Random.Range(0, abc.Length)];
        }
        //Debug.Log(result);
        return result;
    }

    private void mutateBody() {


    }

    private void mutateWeapons() {
        //TODO based on something create weapon :v
        //Debug.Log(generatedNum[8] + " " + generatedNum[9]);

        //PLAYER 1
        //p1Weapon = CreateWhip((int)FeatureGenerator.remap(generatedNumbers[9], 0.0f, 1.0f, 5.0f, 14.0f), 0, null, null);
        p1Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));
        //p1Weapon = CreateStick(0.1f, 1.5f, 1.0f);

        float mass1 = FeatureGenerator.remap(generatedNum[8]+generatedNum[9], 0f, 2f, 0.15f, 0.01f);
        //Debug.Log("mass " + mass);
        GetComponent<PlayerController>().delayValues[1] = mass1;
        
        p1Weapon.transform.SetParent(transform);
        p1Weapon.transform.position = p1Hand.transform.position;
        p1Weapon.transform.localRotation = Quaternion.identity;
        p1Weapon.transform.GetChild(0).GetComponent<CopyTransform>().targetGO = p1Hand.transform;

        //PLAYER 2
        p2Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));
        float mass2 = FeatureGenerator.remap(generatedNum[8] + generatedNum[9], 0f, 2f, 0.15f, 0.01f);
        GetComponent<PlayerController>().delayValues[1] = mass2;

        p2Weapon.transform.SetParent(transform);
        p2Weapon.transform.position = p2Hand.transform.position;
        p2Weapon.transform.localRotation = Quaternion.identity;
        p2Weapon.transform.GetChild(0).GetComponent<CopyTransform>().targetGO = p2Hand.transform;
    }

    public GameObject CreateWhip(int iterations, float offset, GameObject prevGO, GameObject rootGO) {
        if (iterations == 0) {
            return rootGO;
        }
        GameObject newObj = new GameObject();
        if (Random.Range(0, 100) < probability)
            newObj = GameObject.Instantiate(whipBase, this.gameObject.transform);
        else
            newObj = GameObject.Instantiate(whipSomething, this.gameObject.transform);
        newObj.transform.position += new Vector3(offset, 0, 0);

        if (prevGO != null) {
            if (newObj.transform.localScale.x > 0.3f) newObj.transform.localScale = newObj.transform.localScale + new Vector3(Random.Range(0.0f, 0.2f), 0.0f, 0.0f);
            newObj.GetComponent<HingeJoint>().connectedBody = prevGO.GetComponent<Rigidbody>();
        } else {
            rootGO = newObj;
        }
        prevGO = newObj;
        iterations--;

        return CreateWhip(iterations--, offset + newObj.GetComponent<Collider>().bounds.size.x, prevGO, rootGO);

    }

    public GameObject CreateSword(float width, float height) { 
        Debug.Log(width+" "+height);
        GameObject sB = GameObject.Instantiate(swordBase, Vector3.up * 10, Quaternion.identity);
        Transform sBase = sB.transform.GetChild(1);
        Transform blade = sBase.transform.GetChild(0);
        sBase.localScale = new Vector3(width, sBase.localScale.y, sBase.localScale.z);
        blade.localScale = new Vector3(blade.localScale.x, height, blade.localScale.z);
        blade.localPosition = new Vector3(0, height/2, 0);
        return sB;
    }

    public GameObject CreateStick(float width, float height, float mass) {
        GameObject sB = GameObject.Instantiate(swordBase, Vector3.up *10, Quaternion.identity);
        Transform top = sB.transform.GetChild(1);
        Transform bottom = sB.transform.GetChild(2);
        top.localScale = new Vector3(width, height, 0.05f);
        top.localPosition += new Vector3(0, top.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);

        bottom.localScale = new Vector3(width, height, 0.05f);
        bottom.localPosition -= new Vector3(0, bottom.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);
        sB.GetComponent<Rigidbody>().mass = mass;
        return sB;
    }
}
